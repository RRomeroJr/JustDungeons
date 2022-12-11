using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Mirror;
/*
     Container many for any RPG related elements
*/

public enum Role
{
    Healer,
    Tank,
    Melee,
    Ranged
}

public enum ActorState
{
    Free,
    Casting,
    Stunned,
    Silenced,
    Dead
}

public class Actor : NetworkBehaviour
{
    [Header ("Set Manually in Prefab if Needed") ]
    
    
    public bool showDebug = false;
    [SyncVar]
    [SerializeField]protected string actorName;
    [SyncVar]
    [SerializeField]protected int health; // 0
    [SyncVar]
    [SerializeField]protected int maxHealth; // 1
    [SerializeField]protected Role role;
    public Color unitColor;
    public int mobId = -1;
    [SerializeField]protected List<ClassResource> classResources;
    [SyncVar]
    public float mainStat = 100.0f;
    [Header("Automatic")]
    public Actor target;
    
    
    public bool canMove = true;
    [SerializeField]protected List<Buff> buffs;
    
    // When readyToFire is true queuedAbility will fire
    private bool readyToFire = false; // Will True by CastBar for abilities w/ casts. Will only be true for a freme
    private bool isCasting = false; // Will be set False by CastBar
    public bool isChanneling = false;
    public float lastChannelTick = 0.0f;
    //[SerializeField]protected Ability queuedAbility; // Used when Ability has a cast time
    //[SyncVar]
    [SerializeField]protected Ability_V2 queuedAbility; // Used when Ability has a cast time
    //[SyncVar]
    [SerializeField]protected Actor queuedTarget; // Used when Ability has a cast time
    [SerializeField]protected NullibleVector3 queuedTargetWP;
    
    [SerializeField]public List<AbilityCooldown> abilityCooldowns = new List<AbilityCooldown>();
    public UIManager uiManager;
    public float castTime;
    public CastBar castBar;

    // Intentionally made this only pass in the id of the ability bc it shouldn't be
    // used for buffing any effects at the moment. Only, "Did this actor cast the desired ability?"
    // then, do something
    public UnityEvent<int> onAbilityCastHooks = new UnityEvent<int>();
    public Animator animator;

    public event EventHandler PlayerIsDead;
    public event EventHandler PlayerIsAlive;
    [SyncVar]
    private int silenced = 0;
    [SyncVar]
    private int feared = 0;
    private int tauntImmune = 0;
    private ActorState actorState = ActorState.Free;

    #region Properties
    
    public int Feared
    {
        get => feared;
        set
        {
            feared = value;
            checkState = true;
        }
    }
    public int Silenced
    {
        get => silenced;
        set
        {
            silenced = value;
            checkState = true;
        }
    }
    public bool IsCasting
    {
        get => isCasting;
        set
        {
            isCasting = value;
            checkState = true;
        }
    }
    public bool ReadyToFire
    {
        get => readyToFire;
        set
        {
            readyToFire = value;
            checkState = true;
        }
    }
    
    #endregion
    public CombatClass combatClass;
    public float resourceTickTime = 0.0f;
    public float resourceTickMax = 1.0f;
    // Unity Methods---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if ((isLocalPlayer) || (tag != "Player"))
        {
            UIManager.playerActor = this;
            Nameplate.Create(this);
        }
        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Debug.Log("Scaling stats: " + actorName);
            //Buff stats here from GameManager?
            bool scaleCurrentHealth = (maxHealth == health);

            maxHealth = (int) (maxHealth * (1 + (GameManager.instance.dungeonHealthScaling * GameManager.instance.dungeonScalingLevel)));
            if (scaleCurrentHealth)
            {
                health = maxHealth;
            }
            mainStat = (int) (mainStat * (1 + (GameManager.instance.dungeonDamageScaling * GameManager.instance.dungeonScalingLevel)));
        }
        animator = GetComponent<Animator>();
        //gameObject.GetComponent<Renderer>().Color = unitColor;
        //Nameplate.Create(this);

        if (combatClass != null)
        {
            int counter = 0;
            foreach (Ability_V2 abi in combatClass.abilityList){
                GetComponent<Controller>().abilities[counter] = abi;
                counter = counter + 1;
            }
            classResources = combatClass.GetClassResources();
        }


    }
    void Update()
    {
        if (checkState)
        {
            CalculateState();
        }
        if(health <= 0)
        {
            if (!isLocalPlayer)
            {
                GameManager.instance.OnMobDeath.Invoke(mobId);
                Destroy(gameObject);
            }
            OnPlayerIsDead();
        }
        else
        {
            OnPlayerIsAlive();
        }
        updateCast();
        updateCooldowns();
        //HandlleBuffs();
        if(isServer){
            handleCastQueue();
            if(classResources.Count > 0){
                ClassResourceCheckRegen();
            }
            
            if(isChanneling){
                checkChannel();
            }
        }
    }
    void FixedUpdate(){
        HandlleBuffs();
    }
    
    // Casting: Starting a Cast---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public bool castAbility3(Ability_V2 _ability, Actor _target = null, NullibleVector3 _targetWP = null)
    {
        /*  Returns true if a REQUEST to fire was made. NOT if the cast was actually fired
        */
        if (feared > 0)
        {
            Debug.LogFormat("Actor.castAbility3(): {0} cannot use abilities, Feared!", actorName);
            return false;
        }
        //Debug.Log("castAbility3");

        if (CheckCooldownAndGCD(_ability))
        {
            Debug.LogFormat("Actor.castAbility3(): {0}'s {1} ability on cooldown", actorName, _ability);
            return false;
        }
        // MirrorTestTools._inst.ClientDebugLog(_ability.getName() + "| Not on cool down or GCD");
        if (!hasTheResources(_ability))
        {
            Debug.LogFormat("Actor.castAbility3(): {0} does not have the resources", actorName);
            return false;
        }
        //if ability is magical check silence
        // For now silence works on everything including auto attack
        if (silenced > 0) //end if(requestingCast)
        {
            Debug.LogFormat("Actor.castAbility3(): {0} try to cast {1}, but is silenced!", actorName, _ability);
            return false;
        }
        if (readyToFire)
        {
            if (showDebug)
            {
                Debug.Log("Actor.castAbility3(): Something else is ready to fire and blocking this cast");
            }
            return false;
        }
        if (isCasting)
        {
            Debug.LogFormat("Actor.castAbility3(): {0} try to cast {1}, but is already casting!", actorName, _ability);
            return false;
        }
        if (_ability.NeedsTargetActor())
        {
            if (_target == null)
            {
                //Debug.Log("Try find target..");
                _target = tryFindTarget(_ability);
            }
            if (_target == null)
            {
                Debug.Log("No suitable target found");
                return false;
            }
            else
            {
                if (!checkRange(_ability, _target.transform.position))
                {
                    if (showDebug)
                    {
                        Debug.Log("You are out of range");
                    }
                    return false;
                }
            }
        }
        if (_ability.NeedsTargetWP())
        {
            if (_targetWP == null)
            {
                //Debug.Log("Try find target..");
                _targetWP = tryFindTargetWP(_ability, _target);
            }
            if (_targetWP == null)
            {
                Debug.Log("No suitable WP found");
                return false;
            }
            else
            {
                if (!checkRange(_ability, _targetWP.Value))
                {
                    if (showDebug)
                    {
                        Debug.Log("You are out of range");
                    }
                    return false;
                }
            }
        }
        //Debug.Log("castAbility3 inner if");
        // if(isServer){
        //     serverSays(_ability);
        // }
        if (isServer)
        {
            //MirrorTestTools._inst.ClientDebugLog("Starting RPC");
            rpcStartCast(_ability, _target, _targetWP);
        }
        else if (isLocalPlayer)
        { //isLocalPlayer check may not be necessary
            cmdStartCast(_ability, _target, _targetWP);
        }
        //Debug.Log("after Ability_V2 command reached");  
        return true;
    }
    [ClientRpc]
    public void rpcStartCast(Ability_V2 _ability, Actor _target, NullibleVector3 _targetWP){
        //Debug.Log(actorName + " casted " + _ability.getName());
        // if(MirrorTestTools._inst != null)
        //     MirrorTestTools._inst.ClientDebugLog(_ability.getName()+ "| Host Starting RPCStartCast");


        /* This needs to be revised.
            But for right now this need to be a Rpc so that clients start a cast bar 
            for casted abilities.

            But if the ability has no cast time this function is litterally pointless.
            the firecast doesn't and SHOULDN'T be called on a client
        */

        if(!_ability.offGDC){
                GetComponent<Controller>().globalCooldown = Controller.gcdBase; 
            }
        // Debug.Log("rpcStartCast");
        if(_ability.getCastTime() > 0.0f){
                        
            queueAbility(_ability, _target, _targetWP);
            prepCast();
            
        }
        else{
  
            if(isServer){
                fireCast(_ability, _target, _targetWP);
                
            }else{
                //Debug.Log("Client ignoring fireCast");
            }
        }
        
    }
   
    [Command]
    public void cmdStartCast(Ability_V2 _ability, Actor _target, NullibleVector3 _targetWP){
        
        castAbility3(_ability, _target, _targetWP);
        
    }
    void queueAbility(Ability_V2 _ability, Actor _queuedTarget = null, NullibleVector3 _queuedTargetWP = null){
        //Preparing variables for a cast
        queuedAbility = _ability;
        queuedTarget = _queuedTarget;
        queuedTargetWP = _queuedTargetWP;
        
    }
    void prepCast(){
        //Creates castbar for abilities with cast times

        //Debug.Log("Trying to create a castBar for " + _ability.getName())
        IsCasting = true;
        // if(MirrorTestTools._inst != null)
        //             MirrorTestTools._inst.ClientDebugLog("prepcast() isCasting = " + isCasting.ToString());
        // Creating CastBar or CastBarNPC with apropriate variables   
        if( queuedAbility.NeedsTargetActor() && queuedAbility.NeedsTargetWP() ){
            Debug.Log("Spell that needs an Actor and WP are not yet suported");
            IsCasting = false; 
        }
        else if(queuedAbility.NeedsTargetActor()){
            initCastBarWithActor();
        }
        else if(queuedAbility.NeedsTargetWP()){
            initCastBarWithWP();
        }
        else{
            initCastBarWithActor();
        } 
    }
    void initCastBarWithActor(){
        // Creates a CastBar with target being an Actor
        if(gameObject.tag == "Player"){ // For player
                //Creating cast bar and setting it's parent to canvas to display it properly

                GameObject newAbilityCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);
                //                                   v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
                newAbilityCast.GetComponent<CastBar>().Init(queuedAbility.getName(), this, queuedTarget, queuedAbility.getCastTime());
        }
        else{// For NPCs
            if(showDebug)
            Debug.Log(actorName + " starting cast: " + queuedAbility.getName());
            //gameObject.AddComponent<CastBarNPC>().Init(queuedAbility.getName(), this, queuedTarget, queuedAbility.getCastTime());
        }
    }
    void initCastBarWithWP(){
        //   Creates Castbar with target being a world point Vector3

        if(gameObject.tag == "Player"){ // For player
                //Creating cast bar and setting it's parent to canvas to display it properly

                GameObject newAbilityCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);

                //                                   v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
                newAbilityCast.GetComponent<CastBar>().Init(queuedAbility.getName(), this, queuedTargetWP.Value, queuedAbility.getCastTime());
        }
        else{// For NPCs
            if(showDebug)
            Debug.Log(actorName + " starting cast: " + queuedAbility.getName());

            //gameObject.AddComponent<CastBarNPC>().Init(queuedAbility.getName(), this, queuedTargetWP.Value, queuedAbility.getCastTime());
        }
    }
    public void castRelativeToGmObj(Ability_V2 _ability, GameObject _obj, Vector2 _point)
    {
        NullibleVector3 nVect = new NullibleVector3();
        nVect.Value = _obj.transform.position + (Vector3)_point;
        castAbility3(_ability, _targetWP: nVect);
    }
    // Casting: Castbar handling + Firing a cast---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    void updateCast(){
        
        if(IsCasting){
            if(isServer){
                if(GetComponent<Controller>().tryingToMove){
                    resetClientCastVars();
                }
            }
            
            if(isChanneling){
                castTime -= Time.deltaTime;
                lastChannelTick += Time.deltaTime;
            }
            else{
                castTime += Time.deltaTime;
                if(isServer)
                    if(castTime >= queuedAbility.getCastTime()){
                        //Debug.Log("updateCast: readyToFire = true");
                        ReadyToFire = true;
                    }
            }
            
            
                
            
        }
    }
    [Server]
    void handleCastQueue(){
        // Called every Update() to see if queued spell is ready to fire

        if(ReadyToFire){
            //Debug.Log("castCompleted: " + queuedAbility.getName());
            if((queuedAbility.NeedsTargetActor()) && (queuedAbility.NeedsTargetWP())){
                Debug.Log("Cast that requires Actor and WP not yet supported. clearing queue.");
                resetClientCastVars();
            }
            else if(queuedAbility.NeedsTargetWP()){
                fireCast(queuedAbility, null, queuedTargetWP);
            }
            else{
                fireCast(queuedAbility, queuedTarget);
            }
        }
    }

    [Server]
    public void fireCast(Ability_V2 _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        // EI_Clones will be passed into an event that will allow them to be modified as need by other effects, stats, Buffs, etc.
        // Debug.Log("FireCast()");
        if(_ability.isChannel){
            startChannel(_ability, _target, _targetWP);
        }
        else{
            // if(MirrorTestTools._inst != null)
            //     MirrorTestTools._inst.ClientDebugLog(_ability.getName()+ "| Host Starting fireCast");
            List<EffectInstruction> EI_clones = _ability.getEffectInstructions().cloneInstructs();
        if(buffs != null){
            foreach(EffectInstruction eI in EI_clones){
                int i = 0;
                int lastBuffCount = buffs.Count;
                while(i < buffs.Count)
                {
                    var buffCastHooks = buffs[i].onCastHooks;
                    if(buffCastHooks != null){
                        if(buffCastHooks.Count > 0){
                            foreach (var hook in buffCastHooks){
                                hook.Invoke(buffs[i], eI);
                            }
                        }
                    }

                    if(lastBuffCount == buffs.Count)
                        i++;

                }
                
            }
        }
        

        if(isServer){
            //Debug.Log("firecast -> isServer");
            if(hasTheResources(_ability)){
                foreach(AbilityResource ar in _ability.resourceCosts){
                    damageResource(ar.crType, ar.amount);
                }
                // MirrorTestTools._inst.ClientDebugLog(_ability.getName() + " sending effects in fireCast");
                foreach (EffectInstruction eI in EI_clones){
                    eI.sendToActor(_target, _targetWP, this);
                }
                addToCooldowns(_ability);
                if(onAbilityCastHooks != null){
                    onAbilityCastHooks.Invoke(_ability.id);
                }
                if(gameObject.tag == "Player"){
                    animateAbility(_ability);
                }
            }
            
            else{
                Debug.Log(actorName + " doesn't have the resources: fireCast");
            }

            if(_ability.getCastTime() > 0.0f){
                //When the game is running a window seems to break if an instant ability (Like autoattack)
                //goes off closely before a casted ability. So this check was implemented to fix it

                resetClientCastVars();
            }
            if(HBCTools.areHostle(this, _target)){
                GetComponent<Controller>().autoAttacking = true;
            }
            
                
        } 
        
        }
        
    }
    
    public void startChannel(Ability_V2 _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        // EI_Clones will be passed into an event that will allow them to be modified as need by other effects, stats, Buffs, etc.
        
        queueAbility(_ability, _target, _targetWP);
        isChanneling = true;
        lastChannelTick = 0.0f;
        ReadyToFire = false;
        castTime = _ability.channelDuration;
        IsCasting = true;
        fireChannel(queuedAbility, queuedTarget, queuedTargetWP);
        
    }
    //2nd part
    public void checkChannel(){
        if(!IsCasting){
            isChanneling = false;
            resetClientCastVars();
        }
        else{
            //check for first hit



            //check for middle hits
            if(queuedAbility.channelDuration / (queuedAbility.numberOfTicks - 1) <= lastChannelTick){
                fireChannel(queuedAbility, queuedTarget, queuedTargetWP);
                lastChannelTick = 0.0f;
            }
            //check for final hit
            else if(castTime <= 0.0f){
                fireChannel(queuedAbility, queuedTarget, queuedTargetWP);
                lastChannelTick = 0.0f;
                resetClientCastVars();
            }
        }
        
    }
    [Server]
    public void fireChannel(Ability_V2 _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        // EI_Clones will be passed into an event that will allow them to be modified as need by other effects, stats, Buffs, etc.
        
        
        List<EffectInstruction> EI_clones = _ability.getEffectInstructions().cloneInstructs();
        if(buffs != null){
            foreach(EffectInstruction eI in EI_clones){
                int i = 0;
                int lastBuffCount = buffs.Count;
                while(i < buffs.Count)
                {
                    var buffCastHooks = buffs[i].onCastHooks;
                    if(buffCastHooks != null){
                        if(buffCastHooks.Count > 0){
                            foreach (var hook in buffCastHooks){
                                hook.Invoke(buffs[i], eI);
                            }
                        }
                    }

                    if(lastBuffCount == buffs.Count)
                        i++;

                }
            
                
            }
        }
        
        if(isServer){
            //Debug.Log("firecast -> isServer");
            if(hasTheResources(_ability)){
                foreach(AbilityResource ar in _ability.resourceCosts){
                    damageResource(ar.crType, ar.amount);
                }
                foreach (EffectInstruction eI in EI_clones){
                    eI.sendToActor(_target, _targetWP, this);
                }
                //addToCooldowns(_ability);
                // if(onAbilityCastHooks != null){
                //     onAbilityCastHooks.Invoke(_ability.id);
                // }
                if(gameObject.tag == "Player"){
                    animateAbility(_ability);
                }
            }
            
            else{
                Debug.Log(actorName + " doesn't have the resources: fireChannel");
            }


            
            
        } 
        
        
    }
    
    // Casting: Target finders---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    Actor tryFindTarget(Ability_V2 _ability){
        /*
            run function from Ability that returns a code for how to find a target
            ex.  1 == this actor's target so..
                    _target = target
        */
        if(target != null){ //This actor's target
            //Debug.Log(_ability.getName() + " using current target as target");
            
            return target;
        }
        else{
            Debug.Log("No target found");
            return null;
        }
    }
    Actor tryFindTarget(EffectInstruction _eInstruct){
        if(_eInstruct.targetArg == 0){
            return target;
        }
        else if(_eInstruct.targetArg == 1){
            return this;
        }
        else{
            return null;
        }
    }
    NullibleVector3 tryFindTargetWP(Ability_V2 _ability, Actor passedTarget = null){
        /* In the future I might make a method in the player controller
            to display a graphic and wait for a mouse click to get the 
            world point target but for now I'll just do it immediately */
        NullibleVector3 toReturn = new NullibleVector3();
        if(passedTarget != null){
            toReturn.Value = passedTarget.transform.position;
        }else{
            if(tag == "Player"){
                toReturn.Value = gameObject.GetComponent<PlayerController>().getWorldPointTarget();
            }
            else{
                Debug.LogError("NPC: " + actorName + " is trying to cast a WP ability with no WP");
            }
        }
        
        
        return toReturn;
         
    }
    
    // Casting: Reseting---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    [ClientRpc]
    void resetClientCastVars(){
        resetQueue();
        ReadyToFire = false;
        IsCasting = false;
        resetCastTime();
    }

    void resetQueue(){
        queuedTarget = null;
        queuedTargetWP = null;
    }
    void resetCastTime(){
        IsCasting = false;
        castTime = 0.0f;
    }


    // Casting: AbilityEff handling---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void recieveEffect(EffectInstruction _eInstruct, NullibleVector3 _targetWP, Actor _caster, Actor _secondaryTarget = null)
    {
        // foreach (var eI in _eInstructs){
        //     eI.startEffect(this, _targetWP, _caster);
        // }
        int i = 0;
        int lastBuffCount = buffs.Count;
        while (i < buffs.Count)
        {
            var buffhitHooks = buffs[i].onHitHooks;
            if (buffhitHooks != null)
            {
                if (buffhitHooks.GetPersistentEventCount() > 0)
                {
                    Debug.Log("Invokeing onHitHooks: " + buffs[i].getEffectName());
                    buffhitHooks.Invoke(buffs[i], _eInstruct);
                }
                else
                {
                    //Debug.Log("Buff has no hooks");
                }
            }

            if (lastBuffCount == buffs.Count)
            {
                i++;
            }
        }
        //Debug.Log(actorName + " is starting eI for effect (" + _eInstruct.effect.effectName + ") From: " + (_caster != null ? _caster.actorName : "none"));
        //Debug.Log("recieveEffect " + _eInstruct.effect.effectName +"| caster:" + (_caster != null ? _caster.getActorName() : "_caster is null"));
        _eInstruct.startEffect(this, _targetWP, _caster, _secondaryTarget);
    }
    
    // Buffs---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

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
    public void removeBuff(Buff _callingBuff)
    {
        int buffIndex = buffs.FindIndex(x => x == _callingBuff);

        buffs.RemoveAt(buffIndex);
        Debug.Log("Removed index: " + buffIndex);

        RpcRemoveBuffIndex(buffIndex);
    }

    public void ClientRemoveBuff(Buff _callingBuff)
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

    void AddNewBuff(Buff _buff)
    {
        _buff.setActor(this);
        _buff.setRemainingTime(_buff.getDuration());
        buffs.Add(_buff);
    }

    [ClientRpc]
    void RpcAddNewBuff(Buff _buffFromSever)
    {
        if (isServer)
        {
            return;
        }
        AddNewBuff(_buffFromSever);
    }
    public void applyBuff(Buff _buff)
    {
        //Adding Buff it to this actor's list<Buff>
        if (_buff.getID() >= 0)
        {
            //Check if the buff is already here
            Buff tempBuff_Ref = buffs.Find(b => b.getID() == _buff.getID());

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

    void CheckBuffToRemoveAtPos(Buff _buff, int listPos)
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
    
    // Cooldowns---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    [TargetRpc]
    void TRpcUpdateCooldowns(List<AbilityCooldown> hostListACs)
    {
        // Debug.Log("Updating cooldows hostListACs count: " +  hostListACs.Count.ToString());
        abilityCooldowns = hostListACs;
    }
    void updateCooldowns(){
        if(abilityCooldowns.Count > 0){
            for(int i = 0; i < abilityCooldowns.Count; i++){
                if(abilityCooldowns[i].remainingTime > 0)
                    abilityCooldowns[i].remainingTime -= Time.deltaTime;
                else
                    abilityCooldowns.RemoveAt(i);
            }
        }
    }
    void addToCooldowns(Ability_V2 _ability){
        abilityCooldowns.Add(new AbilityCooldown(_ability));
        //Debug.Log("Host: Updating client cooldowns. Length of list: " + abilityCooldowns.Count);
        if(tag == "Player"){
            TRpcUpdateCooldowns(abilityCooldowns);
        }
        
    }
    public bool checkOnCooldown(Ability_V2 _ability){
        
        if(abilityCooldowns.Count > 0){
            for(int i = 0; i < abilityCooldowns.Count; i++){
                if(abilityCooldowns[i].getName() == _ability.getName()){
                    if(showDebug){
                        //Debug.Log(_ability.getName() + " is on cooldown!");
                    }
                        
                    return true;
                }
            }
            return false;
        }
        else{
            return false;
        }
    }
    public bool CheckOnGCD(){
        if(GetComponent<Controller>().globalCooldown > 0.0f){
            //Debug.Log(actorName + " is on gcd");
            return true;
        }
        else{
            //Debug.Log(actorName + " Not on gcd");
            return false;
        }
    }

    /// <summary>
    /// Return true if the ability is on cooldown. False if off cooldown.
    /// </summary>
    public bool CheckCooldownAndGCD(Ability_V2 _ability)
    {
        if (_ability.offGDC == false)
        { //if its a oGCD chekc if on GCD
            if (CheckOnGCD())
            {
                return true;
            }
        }
        if (checkOnCooldown(_ability))
        { // If we make it through check the ability cd
            return true;
        }

        return false; //if we make it here we are good the GCD and ability not on CD
    }
    
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
    public void ClassResourceCheckRegen(){
        
        resourceTickTime += Time.deltaTime;
        if(resourceTickTime < resourceTickMax){
            return;
        }
        resourceTickTime -= resourceTickMax;
        int count = 0;
        foreach(ClassResource _cr in classResources){
            if(_cr.combatRegen != 0){
                restoreResource(_cr.crType, _cr.combatRegen);
            }
            count += 1;
        }
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
    
    public bool hasResource(ClassResourceType _crType, int _amount){
        if(classResources != null){
            foreach(ClassResource cr in classResources){
                if(cr.crType == _crType){
                    if(_amount <= cr.amount ){
                        //Debug.Log("Has resource AND amount");
                        return true;
                    }
                    else{
                        Debug.LogWarning("Has resource but not amount" + cr.amount + " < " + _amount);
                        return false;
                    }
                    
                }
            }
        }
        Debug.LogWarning("Class Resources are null");
        return true;
    }

    /// <summary>
    /// Return true if the actor has resource to cast ability. False if not enough resource.
    /// </summary>
    public bool hasTheResources(Ability_V2 _ability)
    {
        if (_ability != null)
        {
            if (_ability.resourceCosts == null)
            {
                return true;
            }
            else
            {
                if (_ability.resourceCosts.Count == 0)
                {
                    return true;
                }
                foreach (AbilityResource ar in _ability.resourceCosts)
                {
                    if (hasResource(ar.crType, ar.amount) == false)
                    {
                        return false;
                    }
                }
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
        if (amount <= 0)
        {
            Debug.Log("Amount was Neg calling to restoreValue instead");
            restoreValue(-1 * amount, valueType); //if negative call restore instead with amount's sign flipped
            return;
        }
        switch (valueType)
        {
            case 0:
                health -= amount;
                if (fromActor != null)
                {
                    if (fromActor.tag == "Player")
                    {
                        TRpcCreateDamageTextOffensive(fromActor.GetNetworkConnection(), amount);
                    }
                }
                if (tag == "Player")
                {
                    TRpcCreateDamageTextSelf(amount);
                }
                if (fromActor != null)
                {
                    addDamamgeToMeter(fromActor, amount);
                }
                if (health < 0)
                {
                    health = 0;
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
        if (amount <= 0)
        {
            Debug.Log("Amount was Neg calling to damageValue instead");
            damageValue(-1 * amount, valueType); // if negative call damage instead with amount's sign flipped
        }
        switch (valueType)
        {
            case 0:
                health += amount;
                if (fromActor != null)
                {
                    if (fromActor.tag == "Player")
                    {
                        TRpcCreateDamageTextOffensive(fromActor.GetNetworkConnection(), amount);
                    }
                }
                if (tag == "Player")
                {
                    TRpcCreateDamageTextSelf(amount);
                }
                if (fromActor != null)
                {
                    addDamamgeToMeter(fromActor, amount);
                }
                if (health > maxHealth)
                {
                    health = maxHealth;
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
    public Ability_V2 getQueuedAbility(){
        return queuedAbility;
    }
    public int getHealth(){
        return health;
    }
    public void setHealth(int _health){
        health = _health;
    }
    public int getMaxHealth(){
        return maxHealth;
    }
    public void setMaxHealth(int _maxHealth){
        maxHealth = _maxHealth;
    }

    public int getResourceAmount(int index){
        return classResources[index].amount;
    }
    public int getResourceMax(int index){
        return classResources[index].max;
    }
    public ClassResourceType getResourceType(int index){
        return classResources[index].crType;
    }
    public int ResourceTypeCount(){
        return classResources.Count;
    }
    public string getActorName(){
        return actorName;
    }
    public void setActorName(string _actorName){
        actorName = _actorName;
    }
    public List<Buff> getBuffs(){
        return buffs;
    }
    public void setBuffs(List<Buff> _buffs){
        buffs = _buffs;
    }
    public Role getRole()
    {
        return role;
    }
    public void setRole(Role _role)
    {
        role = _role;
    }
    [ClientRpc]
    public void rpcSetTarget(Actor _target){
        target = _target;
    }
    [Command]
    public void cmdReqSetTarget(Actor _target){ //in future this should be some sort of actor id or something
        rpcSetTarget(_target);
    }
    [ClientRpc]
    public void rpcSetQueuedTarget(Actor _queuedTarget){
        queuedTarget = _queuedTarget;
    }
    [Command]
    public void cmdReqSetQueuedTarget(Actor _queuedTarget){ 
        rpcSetQueuedTarget(_queuedTarget);
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
   
    // Misc---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
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
        TempDamageMeter.addToEntry(fromActor, amount);
    }
    public bool checkRange(Ability_V2 _ability, Vector2 _target){
        if(Vector2.Distance(transform.position, _target) > _ability.range){
            return false;
        }
        else{
            return true;
        }
    }
    public void interruptCast(){

        Debug.Log(queuedAbility.getName() + " was interrupted!");
        resetClientCastVars();
        //Make cast bar red for a sec or two
    }
    public void LocalPlayerBroadcastTarget(){
        if(!isLocalPlayer){
            Debug.LogError("Target not set. not local player");
            return;
        }
        if(isServer){
            RpcSetTarget(target);
        }
        else{
            CmdSetTarget(target);    
        }
        
    }
    [ClientRpc]
    void animateAbility(Ability_V2 _ability){
        //animator.SetInteger("abilityType", ((int)_ability.abilityTag));
        if(_ability.abilityTag == AbilityTags.Weapon){
            animator.SetTrigger("abilityCast");
        }
        if(_ability.abilityTag == AbilityTags.SpecialWeapon){
            animator.SetTrigger("SpecialWeapon");
        }
        
    }
    [ClientRpc]
    public void Knockback(Vector2 _hostVect){
        GetComponent<Rigidbody2D>().AddForce(_hostVect);
    }

    void CalculateState()
    {
        if (Feared > 0)
        {
            actorState = ActorState.Stunned;
            return;
        }
        if (Silenced > 0)
        {
            actorState = ActorState.Silenced;
            return;
        }
        if (ReadyToFire || IsCasting)
        {
            actorState = ActorState.Casting;
            return;
        }
        actorState = ActorState.Free;
        checkState = false;
    }

    #region EventHandlers

    protected virtual void OnPlayerIsDead()
    {
        EventHandler raiseEvent = PlayerIsDead;
        if (raiseEvent != null)
        {
            raiseEvent(this, EventArgs.Empty);
        }
    }

    protected virtual void OnPlayerIsAlive()
    {
        EventHandler raiseEvent = PlayerIsAlive;
        if (raiseEvent != null)
        {
            raiseEvent(this, EventArgs.Empty);
        }
    }
    
    #endregion
    
}

