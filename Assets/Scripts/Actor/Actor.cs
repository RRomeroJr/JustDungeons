using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Mirror;
/*
     Container many for any RPG related elements.
*/

public enum Role
{
    Healer,
    Tank,
    Melee,
    Ranged
}

public enum StatusEffectState
{
    None = 0,
    Casting,
    Stunned,
    Silenced,
    Dizzy,
    Feared,
}

public enum ActorState
{
    Alive,
    Dead
}
public enum ActorStats
{
    Health, MaxHealth, MainStat, HealingPower
    // MissingHealth?, CurrentHealth?, PercentHealth?
}

public class Actor : NetworkBehaviour
{
    [Header("Set Manually in Prefab if Needed")]
    public bool showDebug = false;
    [SyncVar]
    [SerializeField] private string actorName;
    [SyncVar]
    [SerializeField] private int health; // 0
    [SyncVar]
    [SerializeField] private int maxHealth; // 1
    [field: SerializeField] public Role Role { get; set; }
    public Color unitColor;
    public int mobId = -1;
    [SyncVar]
    public CombatClass combatClass;
    [SyncVar]
    public float mainStat = 100.0f;
    [SyncVar]
    public float healingPower = 10.0f;

    [Header("Debug Values. Do NOT change in editor.")]
    public Actor target;
    public IAllBuffs buffHandler = null;
    private List<OldBuff.Buff> buffs;
    public AbilityHandler abilityHandler;
    public Nameplate nameplate;
    [SerializeField] private List<ClassResource> classResources;

    // Status Effects
    [SyncVar]
    private int silenced = 0;
    [SyncVar]
    private int feared = 0;

    [Header("Actor State")]
    public ActorState state = ActorState.Alive;
    [SyncVar]
    private bool canMove = true;
    [SyncVar]
    private bool canAttack = true;
    [SyncVar]
    private bool canCast = true;
    public bool inCombat = false; //  in/ out of combat
    public bool combatLocked = false;

    // Events
    public event EventHandler PlayerIsDead;
    public event EventHandler PlayerIsAlive;
    
    //Attacker List
    [SerializeField]
    private List<Actor> attackerList = new List<Actor>();

    #region Properties
    public string ActorName
    {
        get => actorName;
        set => actorName = value;
    }

    public bool CanMove => canMove;
    public bool CanAttack => canAttack;
    public bool CanCast => canCast;

    public int Health
    {
        get => health;
        set
        {
            if (state == ActorState.Dead)
            {
                return;
            }
            if (value <= 0)
            {
                health = 0;
                PlayerDead();
                return;
            }
            if (value > maxHealth)
            {
                health = maxHealth;
                return;
            }
            health = value;
        }
    }
    public int MaxHealth => maxHealth;
    public int Feared
    {
        get => feared;
        set
        {
            feared = value;
        }
    }
    public int Silenced
    {
        get => silenced;
        set
        {
            silenced = value;
        }
    }

    #endregion

    public float resourceTickTime = 0.0f;
    public float resourceTickMax = 1.0f;

    /* 
        Combat Events: 
        
        I know that I already have similar events in GameManager. I thought I would save resources by having
        a single event instead of one for every mob.
        
        But, I think in the long run it might end up being worse. For ex OnEnterCombat here gets hooked into
        by EnemyController and aggros a NPC when invoked. If I did this with the old events anytime a mob 
        entered combat it every loaded mob would need to check if it was them that entered combat then aggro.
        
        If there was like 100 mobs in the level this probably gets stupid really fast.
        
        So I added these two events */
    public UnityEvent OnEnterCombat = new UnityEvent();
    public UnityEvent OnLeaveCombat = new UnityEvent();
    public UnityEvent<EffectInstruction> OnEffectRecieved = new UnityEvent<EffectInstruction>();
    public Controller controller;
    

    #region UnityMethods
    void OnReceivetest(EffectInstruction _ei)
    {
        Debug.Log("OnEffectRecieved: " + _ei.effect.effectName);
    }
    void OnEnable()
    {
        if (nameplate == null)
        {
            nameplate = Nameplate.Create(this);
        }
        else if(nameplate.gameObject.active == false)
        {
            nameplate.gameObject.active = true;
        }
    }
    private void Awake()
    {
        buffHandler = GetComponent<IAllBuffs>();
        abilityHandler = GetComponent<AbilityHandler>();
        OnEnterCombat = new UnityEvent();
        OnLeaveCombat = new UnityEvent();
        buffs = new List<OldBuff.Buff>();
        controller = GetComponent<Controller>();
        // OnEffectRecieved.AddListener(OnReceivetest);
    }

    void Start()
    {
        SpawnClickBox();
        InitializeUI();

        if (gameObject.layer == LayerMask.NameToLayer("Enemy") && GameManager.instance != null)
        {
            //Buff stats here from GameManager?
            bool scaleCurrentHealth = (maxHealth == Health);

            maxHealth = (int)(maxHealth * (1 + (GameManager.instance.dungeonHealthScaling * GameManager.instance.dungeonScalingLevel)));
            if (scaleCurrentHealth)
            {
                Health = maxHealth;
            }
            mainStat = (int)(mainStat * (1 + (GameManager.instance.dungeonDamageScaling * GameManager.instance.dungeonScalingLevel)));
        }

        if (combatClass != null)
        {

            // Old setting up of keybinds
            // foreach (Ability_V2 abi in combatClass.abilityList){
            //     GetComponent<Controller>().abilities[counter] = abi;
            //     counter = counter + 1;
            // }
            // New hobutton spawn
            classResources = combatClass.GetClassResources();

            if (combatClass.classStats != null)
            {
                setUpStats(combatClass.classStats);

            }
            Debug.Log("Before RAC set up");
            if (combatClass.rac != null)
            {
                Debug.Log("RAC found in class " + combatClass.name);
                GetComponent<Animator>().runtimeAnimatorController = combatClass.rac;
            }
            else
            {
                Debug.Log("NO RAC found in, " + combatClass.name);

            }
        }

        if (!isServer) { return; }
        // Server only logic below

        if (buffHandler is BuffHandler b)
        {
            b.StatusEffectChanged += HandleStatusEffectChanged;
            b.Interrupted += abilityHandler.InterruptCast;
            b.DamageTaken += HandleDamageTaken;
            b.HealTaken += HandleHealTaken;
        }
    }

    void Update()
    {
        UpdateCombatState();
        ClassResourceCheckRegen();

        if (!isServer) { return; }
        // Server only logic below  

        if(Input.GetKeyDown("0"))
        {
        /* This should probably be a var in UIManager, but I couldn't think of an 
            easy way to disable all the nameplates. I could use an event but that
            seemed like it would be annoying to set up rn.
            So for now it's here 
        */ 
            if(HBCTools.areHostle(UIManager.playerActor.transform, transform) == false)
            {
                Debug.Log("Friendly nameplates disabled");
                nameplate.gameObject.SetActive(!nameplate.gameObject.active);
            }
        }
    }

    void FixedUpdate()
    {
        HandlleBuffs();
    }

    #endregion

    private void InitializeUI()
    {

        if(isLocalPlayer == false)
        {
            return;
        }
        if (UIManager.Instance == null)
        {
            return;
        }
        UIManager.playerActor = this;
        UIManager.Instance.SpawnBuffBar();
        // if (tag != "Player")
        // {
        //     nameplate = Nameplate.Create(this);
        // }
        if (combatClass != null)
        {
            UIManager.Instance.SetUpHotbars();
            if(combatClass.name == "Mender")
            {
                UIManager.Instance.SpawnHealingGauge();
            }
        }
    }

    // Casting: Target finders---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    public Transform tryFindTarget(Ability_V2 _ability, Transform _target)
    {
        /*
            run function from Ability that returns a code for how to find a target
            ex.  1 == this actor's target so..
                    _target = target
        */
        
        if (target != null && HostiltyMatch(_ability, _target))
        { //This actor's target
          //Debug.Log(_ability.getName() + " using current target as target");

            return _target;
        }

        if(isLocalPlayer)
        { // Check for mouse over if enabled and this is the local player
            PlayerController _pc = controller as PlayerController;

            if( UIManager.Instance.useMouseOver && 
                _pc.hoverActor != null &&
                HostiltyMatch(_ability, _pc.hoverActor.transform))
            {
                return _pc.hoverActor.transform;
            }
        }
        
        if (_ability.isFriendly())
        { // auto selfcast if ability is friendly if nothing else found

            return transform;
        }
        
        Debug.Log("No target found");
        return null;
        
    }

    public Actor tryFindTarget(EffectInstruction _eInstruct)
    {
        if (_eInstruct.targetArg == 0)
        {
            return target;
        }
        else if (_eInstruct.targetArg == 1)
        {
            return this;
        }
        else
        {
            return null;
        }
    }

    public NullibleVector3 tryFindTargetWP(Ability_V2 _ability, Transform passedTarget = null)
    {
        /* In the future I might make a method in the player controller
            to display a graphic and wait for a mouse click to get the 
            world point target but for now I'll just do it immediately */
        NullibleVector3 toReturn = new NullibleVector3();
        if (passedTarget != null)
        {
            toReturn.Value = passedTarget.position;
        }
        else
        {
            if (tag == "Player")
            {
                toReturn.Value = gameObject.GetComponent<PlayerController>().getRelWPTarget();
            }
            else
            {
                Debug.LogError("NPC: " + actorName + " is trying to cast a WP ability with no WP");
            }
        }

        return toReturn;
    }

    public NullibleVector3 GetRealWPOrNull(NullibleVector3 _input = null)
    {
        if (_input == null)
        {
            Debug.Log("GetRealWPOrNull INPUT WAS NULL.. sending null");
            return null;
        }

        
        /*  
            Enable cond below if not solution to the QueuedRelWP problem
            and they are serialized
        */

        // if (_input.Value.magnitude == 0.0f)
        // { //workaround for selecting an actor in editor breaking some channeled spells;
        //     Debug.Log("GetRealWPOrNull sending null");
        //     return null;
        // }

        return new NullibleVector3(_input.Value + transform.position);
    }

    // Casting: AbilityEff handling---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void ReceiveEffect(EffectInstruction _eInstruct, NullibleVector3 _relWP, Actor _caster, Actor _secondaryTarget = null)
    {
        OnEffectRecieved.Invoke(_eInstruct);
        foreach (var buff in buffs)
        {
            var buffhitHooks = buff.onHitHooks;
            if (buffhitHooks != null)
            {
                if (buffhitHooks.GetPersistentEventCount() > 0)
                {
                    Debug.Log("Invokeing onHitHooks: " + buff.getEffectName());
                    buffhitHooks.Invoke(buff, _eInstruct);
                }
            }
        }

        //Debug.Log(actorName + " is starting eI for effect (" + _eInstruct.effect.effectName + ") From: " + (_caster != null ? _caster.actorName : "none"));
        //Debug.Log("recieveEffect " + _eInstruct.effect.effectName +"| caster:" + (_caster != null ? _caster.getActorName() : "_caster is null"));
        _eInstruct.startEffect(transform, _relWP, _caster, _secondaryTarget);
        if (_eInstruct.effect.isHostile && _caster != null)
        {
            CheckStartCombatWith(_caster);
        }
    }

    #region OldBuff

    void HandlleBuffs()
    {
        if (buffs.Count <= 0)
        {
            return;
        }

        int i = 0;
        int lastBuffCount = buffs.Count;
        while (i < buffs.Count)
        {
            //Debug.Log("Buffs[" + i.ToString() + "] = " + buffs[i].getEffectName());
            buffs[i].update();
            if (lastBuffCount == buffs.Count)
            {
                i++;
            }
            else
            {
                ///Debug.Log("After RM Buffs[" + (i - 1).ToString() + "] = " + buffs[i-1].getEffectName());
                lastBuffCount = buffs.Count;
            }
        }
    }
    [Server]
    public void removeBuff(OldBuff.Buff _callingBuff)
    {
        int buffIndex = buffs.FindIndex(x => x == _callingBuff);

        buffs.RemoveAt(buffIndex);
        Debug.Log("Removed index: " + buffIndex);

        RpcRemoveBuffIndex(buffIndex);
    }

    public void ClientRemoveBuff(OldBuff.Buff _callingBuff)
    {
        int buffIndex = buffs.FindIndex(x => x == _callingBuff);

        buffs.RemoveAt(buffIndex);
        Debug.Log("Removed index: " + buffIndex);
    }

    [ClientRpc]
    void RpcRemoveBuffIndex(int hostIndex)
    {
        if (isServer)
        {
            return;
        }
        Debug.Log("Host saying to remove buff index: " + hostIndex);
        buffs[hostIndex].onFinish();
    }

    void AddNewBuff(OldBuff.Buff _buff)
    {
        _buff.setActor(this);
        _buff.setRemainingTime(_buff.getDuration());
        buffs.Add(_buff);
    }

    [ClientRpc]
    void RpcAddNewBuff(OldBuff.Buff _buffFromSever)
    {
        if (isServer)
        {
            return;
        }
        AddNewBuff(_buffFromSever);
    }
    public void applyBuff(OldBuff.Buff _buff)
    {
        //Adding Buff it to this actor's list<Buff>
        if (_buff.getID() >= 0)
        {
            //Check if the buff is already here
            OldBuff.Buff tempBuff_Ref = buffs.Find(b => b.getID() == _buff.getID());

            //if we found something
            if (tempBuff_Ref != null)
            {// Based on circumstances, I might need you do something different

                if ( (tempBuff_Ref.isStackable())&&(tempBuff_Ref.isRefreshable()))
                { // if stackable and refreshable
                    tempBuff_Ref.addStacks(1);
                    tempBuff_Ref.setRemainingTime(tempBuff_Ref.getDuration());
                    //Debug.Log("stting remaing to "+ tempBuff_Ref.getDuration().ToString());
                    return;
                }
                else if (tempBuff_Ref.isStackable())
                {
                    tempBuff_Ref.addStacks(1);
                    return;
                }
                else if (tempBuff_Ref.isRefreshable())
                {
                    //Debug.Log("Refreshable");
                    tempBuff_Ref.setRemainingTime(tempBuff_Ref.getDuration()); // Add pandemic time?
                    return;
                }
            }
        }
        //---------------------If we get this far we are just adding it like normal-------------------------------------
        if (isServer)
        {
            AddNewBuff(_buff);
            RpcAddNewBuff(_buff);
        }
    }

    void CheckBuffToRemoveAtPos(OldBuff.Buff _buff, int listPos)
    {
        // Remove AbilityEffect is it's duration is <= 0.0f

        if (_buff.getRemainingTime() >= 0.0f)
        {
            return;
        }
        if (showDebug)
        {
            Debug.Log(actorName + ": Removing.. " + _buff.getEffectName());
        }
        //buffs[listPos].OnEffectFinish(); // AE has a caster and target now so the args could be null?
        buffs.RemoveAt(listPos);
    }

    #endregion

    // Class Resources---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public bool damageResource(ClassResourceType _crt, int _amount){
        if(_amount < 0){
            Debug.Log("Swapping to dmgResource bc amount was < 0");
            return restoreResource(_crt, -_amount);
        }
        if (_crt == null)
        {
            Debug.Log("Actor.damageResource(): ClassResourceType is null");
            return false;
        }
        // if(_crt == AbilityResourceTypes.Health){
        //     setHealth(actor.getHealth() - _cost.amount);
        //     return true;
        // }
        // else{
        int index = 0;
        foreach (ClassResource cr in classResources)
        {
            if (_crt == cr.crType)
            {
                int diff = cr.amount - _amount;
                if (diff >= 0)
                {
                    updateClassResourceAmount(index, diff);
                }
                else
                {
                    updateClassResourceAmount(index, 0);
                }

                return true;
            }
            index++;
        }
        // }
        return false;
    }
    public bool restoreResource(ClassResourceType _crt, int _amount)
    {
        if (_amount < 0)
        {
            Debug.Log("Swapping to dmgResource bc amount was < 0");
            return damageResource(_crt, -_amount);
        }
        if (_crt == null)
        {
            Debug.Log("Actor.restoreResource(): ClassResourceType is null");
            return false;
        }
        // if(_crt == AbilityResourceTypes.Health){
        //     setHealth(actor.getHealth() - _cost.amount);
        //     return true;
        // }
        // else{
        int index = 0;
        foreach (ClassResource cr in classResources)
        {
            if (_crt == cr.crType)
            { //if _crt is a resource that we have
                int sum = cr.amount + _amount; //Then get the sum of it + what we got

                if (sum <= cr.max)
                {
                    //then add it to our amount
                    //Debug.Log("Adding " + _amount.ToString() + " " + _crt.name);
                    updateClassResourceAmount(index, sum);
                }
                else
                {
                    //set our cr.amount to max
                    updateClassResourceAmount(index, cr.max);
                }
                return true;
            }
            index++;
        }
        // }
        return false;
    }
    public void ClassResourceCheckRegen()
    {
        for(int i = 0; i < classResources.Count; i++)
        {
            if (classResources[i].tickMax <= 0.0f)
            {
                continue;
            }
            classResources[i].tickTime += Time.deltaTime;
            if(isServer)
            {
                if (classResources[i].tickTime >= classResources[i].tickMax)
                {
                    restoreResource(classResources[i].crType, classResources[i].combatRegen);
                    classResources[i].tickTime -= classResources[i].tickMax;
                    RpcResetResourceTime(i);
                }
            }
        }
        
    }
    [ClientRpc]
    public void RpcResetResourceTime(int index){
        classResources[index].tickTime = 0.0f;
    }
    [ClientRpc]
    public void updateClassResourceAmount(int index, int _amount){
        classResources[index].amount = _amount;
    }
    [ClientRpc]
    public void updateClassResourceMax(int index, int _max){
        classResources[index].max = _max;
    }
    [ClientRpc]
    public void updateClassResourceCombatRegen(int index, int _combatRegen){
        classResources[index].combatRegen = _combatRegen;
    }
    [ClientRpc]
    public void updateClassResourceOutOfCombatRegen(int index, int _OutOfCombatRegen){
        classResources[index].outOfCombatRegen = _OutOfCombatRegen;
    }
    
    public bool HasResource(ClassResourceType _crType, int _amount)
    {
        if (classResources == null)
        {
            Debug.LogWarning("Class Resources are null");
            return true;
        }
        foreach (ClassResource cr in classResources)
        {
            if (cr.crType != _crType)
            {
                continue;
            }
            return _amount <= cr.amount;
        }
        return true;
    }

    /// <summary>
    /// Return true if the actor has resource to cast ability. False if not enough resource.
    /// </summary>
    public bool HasTheResources(Ability_V2 _ability)
    {
        if (_ability == null || _ability.resourceCosts == null || _ability.resourceCosts.Count == 0)
        {
            return true;
        }
        foreach (AbilityResource ar in _ability.resourceCosts)
        {
            if (HasResource(ar.crType, ar.amount) == false)
            {
                return false;
            }
        }
        return true;
    }

    // Setter/ Getters---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void damageValue(int amount, int valueType = 0, Actor fromActor = null)
    {
        // Right now this only damages health, but, maybe in the future,
        // This could take an extra param to indicate a different value to "damage"
        // For ex. a Ability that reduces maxHealth or destroys mana

        //Debug.Log("damageValue: " + amount.ToString()+ " on " + actorName);
        if (amount < 0)
        {
            Debug.Log("Amount was Neg calling to restoreValue instead");
            restoreValue(-1 * amount, valueType); //if negative call restore instead with amount's sign flipped
            return;
        }
        int modifiedAmount = amount;
        switch (valueType)
        {
            case 0:
                if(buffHandler != null){
                    modifiedAmount += (int)(amount * (buffHandler.DamageTakenMod - 1.0f));
                }
                if(fromActor != null && fromActor.buffHandler != null){
                    modifiedAmount += (int)(amount * (fromActor.buffHandler.DamageOutMod - 1.0f));
                }
                Health -= (int)modifiedAmount;
                if (fromActor != null)
                {

                    if (fromActor.tag == "Player")
                    {
                        TRpcCreateDamageTextOffensive(fromActor.GetNetworkConnection(), modifiedAmount);
                    }
                    addDamamgeToMeter(fromActor, modifiedAmount);
                }
                if (tag == "Player")
                {
                    TRpcCreateDamageTextSelf(modifiedAmount);
                }

                break;
            case 1:
                maxHealth -= amount;
                if (maxHealth < 1)
                {      // Making this 0 might cause a divide by 0 error. Check later
                    maxHealth = 1;
                }
                break;
            default:
                break;
        }
    }

    public void restoreValue(int amount, int valueType = 0, Actor fromActor = null){
        // This would be the opposite of damageValue(). Look at that for more details
        //  Maybe in the future calcing healing may have diff formula to calcing damage taken

        //Debug.Log("restoreValue: " + amount.ToString()+ " on " + actorName);
        if (amount < 0)
        {
            Debug.Log("Amount was Neg calling to damageValue instead");
            damageValue(-1 * amount, valueType); // if negative call damage instead with amount's sign flipped
        }

        int modifiedAmount = amount;
        switch (valueType)
        {
            case 0:
                if(buffHandler != null){
                    modifiedAmount += (int)(amount * (buffHandler.HealingTakenMod - 1.0f));
                }
                if(fromActor != null && fromActor.buffHandler != null){
                    modifiedAmount += (int)(amount * (fromActor.buffHandler.HealingOutMod - 1.0f));
                }
                Health += (int)modifiedAmount;
                if (fromActor != null)
                {
 
                    if (fromActor.tag == "Player")
                    {
                        TRpcCreateDamageTextOffensive(fromActor.GetNetworkConnection(), modifiedAmount);
                    }
                    addDamamgeToMeter(fromActor, modifiedAmount);
                }
                if (tag == "Player")
                {
                    TRpcCreateDamageTextSelf(modifiedAmount);
                }

                break;
            case 1:
                if (maxHealth + amount > maxHealth)
                { // if int did not wrap around max int
                    maxHealth += amount;
                }
                break;

            default:
                break;
        }
    }

    public int getResourceAmount(int index){
        return classResources[index].amount;
    }
    public int getResourceMax(int index){
        return classResources[index].max;
    }
    public ClassResource getResource(int index){
        return classResources[index];
    }
    public ClassResourceType getResourceType(int index){
        if(index >= classResources.Count)
        {
            return null;
        }
        return classResources[index].crType;
    }
    public int ResourceTypeCount(){
        return classResources.Count;
    }
    public List<OldBuff.Buff> getBuffs(){
        return buffs;
    }
    public void setBuffs(List<OldBuff.Buff> _buffs){
        buffs = _buffs;
    }
    [ClientRpc]
    public void rpcSetTarget(Actor _target){
        target = _target;
    }
    [Command]
    public void cmdReqSetTarget(Actor _target){ //in future this should be some sort of actor id or something
        rpcSetTarget(_target);
    }
    public float getHealthPercent(){
        return (float)health / (float)maxHealth;
    }
    [Command]
    void CmdSetTarget(Actor _ClientTarget){
        target = _ClientTarget;
    }
    [ClientRpc]
    public void RpcSetTarget(Actor _OwnerTarget){
        target = _OwnerTarget;
    }
    
    public void LogClicked()
    {
        Debug.Log(gameObject.name + "If you hook this properly could get the Actor Ref");
    }
    public Vector2 getCastingWPToFace(){
    
        if (abilityHandler.QueuedRelWP2 != null)
        {
            return abilityHandler.QueuedRelWP2.Value + transform.position;
        }
        else if(abilityHandler.QueuedRelWP != null)
        {
            return abilityHandler.QueuedRelWP.Value + transform.position;
        }
       
        return Vector2.zero;
    }
    public void SetTarget(Actor _target){
        if(!isLocalPlayer && !isServer){
            Debug.LogError(name + ": you cannot change this actor's target");
            return;
        }

        try{
            target.nameplate.selectedEvent.Invoke(false);
        }
        catch{

        }

        target = _target;
        LocalPlayerBroadcastTarget();


        try{
            target.nameplate.selectedEvent.Invoke(true);
        }
        catch{

        }
    }
    public float GetStat(ActorStats _stat)
    {
        switch (_stat)
        {
            case ActorStats.Health:
                return health;
                break;
            case ActorStats.MaxHealth:
                return maxHealth;
                break;
            case ActorStats.MainStat:
                return mainStat;
                break;
            case ActorStats.HealingPower:
                return healingPower;
                break;
            default:
                return 0.0f;
                break;
        }
    }

    // Misc---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Buff has interrupted spellcasting
    /// </summary>
    public bool isCastImmobile()
    {
        if (IsCasting == false)
        {
            return false;
        }

        return !abilityHandler.QueuedAbility.castWhileMoving;
    }
    public bool isTauntable(){
        return !(tag =="Player" || (controller as EnemyController).tauntImmune > 0);
    }
    public void OnHoverStart()
    {
        Material _mat = GetComponent<SpriteRenderer>().material;
        Color _color = _mat.GetColor("_HoverHighlight");
        _color.a = 0.24f;
       

        _mat.SetColor("_HoverHighlight", _color);
    }
    public void OnHoverEnd()
    {
        Material _mat = GetComponent<SpriteRenderer>().material;
        Color _color = _mat.GetColor("_HoverHighlight");
        _color.a = 0.0f;

        _mat.SetColor("_HoverHighlight", _color);
    }
    public void CheckStartCombatWith(Actor _actor)
    {
        
        if(CheckAddAttackerList(_actor))
        {
            _actor.CheckAddAttackerList(this);
        }
        
    }
    bool CheckAddAttackerList(Actor _attacker = null)
    {
        if(_attacker == null){
            // Debug.LogError(gameObject.name + " _attacker was null");
            return false;
        }
        if(_attacker == this){
            // Debug.LogError(gameObject.name + " _attacker was itself");
            return false;
        }
        if(attackerList.Contains(_attacker)){
            // Debug.LogError(gameObject.name + " already contains attacker" + _attacker.gameObject.name);
            return false;
        }
        // Debug.Log("Adding attacker, " + _attacker.gameObject.name + ", to " +gameObject.name);
        attackerList.Add(_attacker);
        return true;
    }
    public Actor FirstAliveAttacker()
    {
        // Debug.Log(attackerList.Count);
        foreach(Actor a in attackerList)
        {
            if(a == null)
                continue;
            if(a.state == ActorState.Alive)
            {
                return a;
            }
        }
        return null;
    }
    void UpdateCombatState(){
        if (GameManager.instance == null)
        {
            return;
        }
        bool result = IsInCombat();
        if(inCombat != result){
            inCombat = result;
            if(inCombat)
            {
                OnEnterCombat.Invoke();
                GameManager.instance.OnActorEnterCombat.Invoke(this);
            }
            else
            {
                OnLeaveCombat.Invoke();
                GameManager.instance.OnActorLeaveCombat.Invoke(this);
            }
        }
    }
    
    void CleanUpAttackerList(){

    }
    public bool IsInCombat(){
        if(combatLocked){
            return true;
        }
        if(attackerList.Count <= 0){
            return false;
        }
        
        int i = 0;
        while(i< attackerList.Count){
            if(attackerList[i] == null){
            
                attackerList.RemoveAt(i);
            }
            else{
                if(attackerList[i].state != ActorState.Dead){
                    return true;
                }
                i++;
            }
        }
        
        // foreach(Actor a in attackerList){
        //     if(a.State != ActorState.Dead){
        //         return true;
        //     }
        // }
        return false;

    }
    [TargetRpc]
    void TRpcCreateDamageTextSelf(int amount)
    {
        DamageText.Create(transform.position, amount);
    }

    [TargetRpc]
    void TRpcCreateDamageTextOffensive(NetworkConnection attackingPlayer, int amount)
    {
        //Debug.Log("disiplaying Damage numbers");
        DamageText.Create(transform.position, amount);
    }

    [ClientRpc]
    void addDamamgeToMeter(Actor fromActor, int amount)
    {
        if (TempDamageMeter.entryList != null)
        {
            TempDamageMeter.addToEntry(fromActor, amount);
        }
    }
    
    public void LocalPlayerBroadcastTarget(){
        
        if(isServer){
            RpcSetTarget(target);
        }
        else{
            CmdSetTarget(target);    
        }
        
    }
    
    [ClientRpc]
    public void Knockback(Vector2 _hostVect){
        GetComponent<Rigidbody2D>().AddForce(_hostVect);
    }
    public void setUpStats(ClassStats _classStats){
        //maxHealth = _classStats.health;
        //health = maxHealth;
        
        maxHealth = (int)(maxHealth * _classStats.healthMutliplier);
        health = maxHealth;
        mainStat = _classStats.mainStat;
        healingPower = _classStats.HealingPower;

    }

    private void PlayerDead()
    {
        abilityHandler.InterruptCast();
        state = ActorState.Dead;
        canAttack = false;
        canMove = false;
        canCast = false;

        if(tag != "Player" && GetComponent<DespawnScript>() == null){
            gameObject.AddComponent<DespawnScript>();
        }
        OnPlayerIsDead();
    }

    private void PlayerAlive()
    {
        state = ActorState.Alive;
        canAttack = true;
        canMove = true;
        canCast = true;
        OnPlayerIsAlive();
    }
    private void HandleHealTaken(object sender, HealEventArgs e)
    {
        restoreValue((int)e.Heal);
    }

    private void HandleDamageTaken(object sender, DamageEventArgs e)
    {
        damageValue((int)e.Damage);
    }

    #region CalculateState

    [Server]
    private void HandleStatusEffectChanged(object sender, StatusEffectChangedEventArgs e)
    {
        if (state == ActorState.Dead)
        {
            return;
        }
        canMove = CalculateCanMove(e);
        canAttack = CalculateCanAttack(e);
        canCast = CalculateCanCast(e);
    }

    [Server]
    private bool CalculateCanMove(StatusEffectChangedEventArgs e)
    {
        return e.Feared <= 0 && e.Stunned <= 0;
    }

    [Server]
    private bool CalculateCanAttack(StatusEffectChangedEventArgs e)
    {
        return e.Feared <= 0 && e.Stunned <= 0;
    }

    [Server]
    private bool CalculateCanCast(StatusEffectChangedEventArgs e)
    {
        return e.Feared <= 0 && e.Stunned <= 0 && e.Silenced <= 0;
    }

    #endregion

    #region EventRaised

    protected virtual void OnPlayerIsDead()
    {
        PlayerIsDead?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnPlayerIsAlive()
    {
        PlayerIsAlive?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region CompatabilityMethodsForAbilityHandler

    public bool IsCasting => abilityHandler.IsCasting;
    public List<AbilityCooldown> abilityCooldowns => abilityHandler.abilityCooldowns;
    public UnityEvent<int> onAbilityCastHooks => abilityHandler.onAbilityCastHooks;
    public float castTime => abilityHandler.castTime;

    public void RefundCooldown(Ability_V2 a, float b)
    {
        abilityHandler.RefundCooldown(a, b);
    }
    public void interruptCast()
    {
        abilityHandler.InterruptCast();
    }
    public Ability_V2 getQueuedAbility()
    {
        return abilityHandler.QueuedAbility;
    }
    public bool castAbility3(Ability_V2 _ability, Transform _target = null, NullibleVector3 _relWP = null, NullibleVector3 _relWP2 = null)
    {
        return abilityHandler.CastAbility3(_ability, _target, _relWP, _relWP2);
    }
    public bool checkOnCooldown(Ability_V2 _ability)
    {
        return abilityHandler.CheckOnCooldown(_ability);
    }
    public bool castAbilityRealWPs(Ability_V2 _ability, Transform _target = null, NullibleVector3 _WP = null, NullibleVector3 _WP2 = null)
    {
        return abilityHandler.CastAbilityRealWPs(_ability, _target, _WP, _WP2);
    }
    
    bool HostiltyMatch(Ability_V2 _ability, Transform _target)
    {
        if(_target == null){
            Debug.LogError("Ability hostilty checked on null _target");
            return false;
        }
        if(_ability.isFriendly() && HBCTools.areHostle(transform, _target))
        {
            return false;
        }
        if(_ability.isHostile() && HBCTools.areHostle(transform, _target) == false)
        {
            return false;
        }
        return true;
    }
    void SpawnClickBox()
    {
        GameObject clickBox = new GameObject("ClickBox");
        clickBox.layer = LayerMask.NameToLayer("UI");
        clickBox.transform.parent = transform;
        clickBox.transform.localPosition = Vector3.zero;
        clickBox.AddComponent<BoxCollider2D>().isTrigger = true;
        clickBox.AddComponent<ClickboxManager>();
    }
    #endregion
}
