using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class AbilityHandler : NetworkBehaviour
{
    [Header("Debug Values. Do NOT change in editor.")]
    private Animator animator;
    private Actor actor;
    [SerializeField] private bool showDebug = false;
    
    [field: SerializeField] public Ability_V2 QueuedAbility { get; private set; } // Used when Ability has a cast time
    [field: SerializeField] public Actor QueuedTarget { get; private set; } // Used when Ability has a cast time

    /* WARNING!!!
            DO NOT SERIALIZE BELOW. Doing so will cause an object to be created if
            values are null when selected in the editor!

            This will break some abilities that differentiate between null and a
            value of (0, 0, 0)
    */
    public NullibleVector3 QueuedRelWP { get; private set; }
    public NullibleVector3 QueuedRelWP2 { get; private set; }
    //~~~~~~~~~~~~~~~~~~~~>> WARNING DO NOT SERIALIZE, LOOK ABOVE <<~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    [SerializeField] public List<AbilityCooldown> abilityCooldowns = new List<AbilityCooldown>();
    [SerializeField] private List<OldBuff.Buff> buffs;
    
    // Intentionally made this only pass in the id of the ability bc it shouldn't be
    // used for buffing any effects at the moment. Only, "Did this actor cast the desired ability?"
    // then, do something
    public UnityEvent<int> onAbilityCastHooks = new UnityEvent<int>();
    
    // Cast state values
    // When readyToFire is true queuedAbility will fire
    [SerializeField] public bool ReadyToFire = false; // Will True by CastBar for abilities w/ casts. Will only be true for a freme
    
    [SerializeField] public bool IsCasting = false; // Will be set False by CastBar

    [field: SerializeField] public bool IsChanneling { get; private set; } = false;
    // public UnityEvent CastStarted = new UnityEvent();
    // public UnityEvent OnRequestingCast = new UnityEvent();

    // Cast timers
    public float castTime;
    public float lastChannelTick = 0.0f;
    private bool requestingCast = false;
    [field: SerializeField] public bool RequestingCast
    {
        get
        {
            return requestingCast;
        }
        private set
        {
            requestingCast = value;
            // if(requestingCast)
            // {
            //     OnRequestingCast.Invoke();
            // }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        actor = GetComponent<Actor>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCast();
        UpdateCooldowns();

        if (!isServer) { return; }
        // Server only logic below

        HandleCastQueue();
        if (IsChanneling)
        {
            if (QueuedAbility.isChannel == false)
            {
                //wtf how?
                Debug.Log(QueuedAbility.getName() + "is queued and anout to be channeled BUT isn't a channel| _ability ");
            }
            CheckChannel();
        }

    }

    // Casting: Starting a Cast---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public bool CastAbility3(Ability_V2 _ability, Actor _target = null, NullibleVector3 _relWP = null, NullibleVector3 _relWP2 = null)
    {
        if (RequestingCast || IsCasting)
        {
            return false;
        }

        RequestingCast = true;

        bool result = PrivateCastAbility(_ability, _target, _relWP, _relWP2);

        RequestingCast = result;

        if (result && (TryGetComponent(out EnemyController ec)))
        {
            ec.CheckStopToCast(_ability);
            /*
                If I queued abilities here immediately instead of down in RPCStartCast
                I could just use an event here that controller could listen to, know that
                a cast could be starting, check the queued ability, then stop the nav 
                agent if it casted or a channel

                If I did that rn that would change alot of things, and potentally create
                some new bugs, bc Mirror's weirdness. RPC functions don't execute as soon
                as they are called even on the server

                This could lead to issues like casting two abilities quickly together could
                lead the later one overwritting the 1st in the queue causeing only the 2nd 
                to fire once RPCStartCast gets called

                So this is my band-aid to get the stopping to cast behavior I want from NPCs
            */
        }

        return result;
        
    }
    private bool PrivateCastAbility(Ability_V2 _ability, Actor _target = null, NullibleVector3 _relWP = null, NullibleVector3 _relWP2 = null)
    {
        if (!actor.CanCast)
        {
            //Debug.LogFormat("Actor.castAbility3(): {0} try to cast {1}, but is {2}!", actorName, _ability, EffectState);
            return false;
        }
        // if(_ability.getCastTime() > 0.0f && _ability.castWhileMoving == false && actor.controller.TryingToMove)
        // {
        //     Debug.Log(actor.name + "Cannot cast, trying to move");
        //     return false;
        // }
        if (IsChanneling)
        {
            Debug.LogFormat("Actor.castAbility3(): {0} try to cast {1}, but is CHANNELING and also somehow free to act!", actor.ActorName, _ability);
            return false;
        }
        if (CheckCooldownAndGCD(_ability))
        {
            Debug.LogFormat("Actor.castAbility3(): {0}'s {1} ability on cooldown", actor.ActorName, _ability);
            return false;
        }
        // MirrorTestTools._inst.ClientDebugLog(_ability.getName() + "| Not on cool down or GCD");
        if (!actor.HasTheResources(_ability))
        {
            Debug.LogFormat("Actor.castAbility3(): {0} does not have the resources", actor.ActorName);
            return false;
        }

        if (_ability.NeedsTargetActor())
        {
            if (_target == null)
            {
                //Debug.Log("Try find target..");
                _target = actor.tryFindTarget(_ability);
            }
            if (_target == null)
            {
                Debug.Log("No suitable target found");
                return false;
            }
            else
            {
                if (!CheckRange(_ability, _target.transform.position))
                {
                    if (showDebug)
                    {
                        //Debug.Log("You are out of range");
                    }
                    return false;
                }
                if (_ability.mustBeFacing)
                {
                    if (!HBCTools.checkFacing(actor, _target.gameObject))
                    {
                        Debug.Log("You are not facing target: " + _target.ActorName);
                        return false;
                    }
                }
            }
        }
        if (_ability.NeedsTargetWP())
        {
            if (_relWP == null)
            {
                //Debug.Log("Try find target..");
                _relWP = actor.tryFindTargetWP(_ability, _target);
            }
            if (_relWP == null)
            {
                Debug.Log("No suitable WP found");
                return false;
            }
            else
            {
                if (!CheckRange(_ability, _relWP.Value))
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
            RpcStartCast(_ability, _target, _relWP, _relWP2);
        }
        else if (isLocalPlayer)
        { //isLocalPlayer check may not be necessary
            CmdStartCast(_ability, _target, _relWP, _relWP2);
        }
        //Debug.Log("after Ability_V2 command reached");  
        return true;
    }
    [ClientRpc]
    public void RpcStartCast(Ability_V2 _ability, Actor _target, NullibleVector3 _relWP, NullibleVector3 _relWP2)
    {
        //Debug.Log(actorName + " casted " + _ability.getName());
        // if(MirrorTestTools._inst != null)
        //     MirrorTestTools._inst.ClientDebugLog(_ability.getName()+ "| Host Starting RPCStartCast");


        /* This needs to be revised.
            But for right now this need to be a Rpc so that clients start a cast bar 
            for casted abilities.

            But if the ability has no cast time this function is litterally pointless.
            the firecast doesn't and SHOULDN'T be called on a client
        */

        if (!_ability.offGDC)
        {
            GetComponent<Controller>().globalCooldown = Controller.gcdBase;
        }
        // Debug.Log("rpcStartCast");
        if (_ability.getCastTime() > 0.0f)
        {
            QueueAbility(_ability, _target, _relWP, _relWP2);
            PrepCast();
        }
        else
        {
            if (isServer)
            {
                FireCast(_ability, _target, _relWP, _relWP2);
            }
            else
            {
                //Debug.Log("Client ignoring fireCast");
            }
        }
        RequestingCast = false;
    }

    [Command]
    public void CmdStartCast(Ability_V2 _ability, Actor _target, NullibleVector3 _relWP, NullibleVector3 _relWP2)
    {
        CastAbility3(_ability, _target, _relWP, _relWP2);
    }

    void QueueAbility(Ability_V2 _ability, Actor _queuedTarget = null, NullibleVector3 _queuedRelWP = null, NullibleVector3 _queuedRelWP2 = null)
    {
        //Preparing variables for a cast
        QueuedAbility = _ability;
        QueuedTarget = _queuedTarget;
        QueuedRelWP = _queuedRelWP;
        QueuedRelWP2 = _queuedRelWP2;
    }

    void PrepCast()
    {
        //Creates castbar for abilities with cast times

        if (QueuedAbility.NeedsTargetActor() && QueuedAbility.NeedsTargetWP())
        {
            Debug.Log("Spell that needs an Actor and WP are not yet suported");
            IsCasting = false;
            return;
        }

        IsCasting = true;
        if (QueuedAbility.NeedsTargetActor())
        {
            InitCastBarWithActor();
        }
        else if (QueuedAbility.NeedsTargetWP())
        {
            initCastBarWithWP();
        }
        else
        {
            InitCastBarWithActor();
        }
    }

    void InitCastBarWithActor()
    {
        if (UIManager.Instance == null)
        {
            return;
        }
        // Creates a CastBar with target being an Actor
        if (gameObject.tag == "Player")
        { // For player
          //Creating cast bar and setting it's parent to canvas to display it properly

            GameObject newAbilityCast = Instantiate(UIManager.Instance.castBarPrefab, UIManager.Instance.canvas.transform);
            //                                   v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
            newAbilityCast.GetComponent<CastBar>().Init(QueuedAbility.getName(), this, QueuedTarget, QueuedAbility.getCastTime());
        }
        else
        {// For NPCs
            if (showDebug)
                Debug.Log(actor.ActorName + " starting cast: " + QueuedAbility.getName());
            //gameObject.AddComponent<CastBarNPC>().Init(queuedAbility.getName(), this, queuedTarget, queuedAbility.getCastTime());
        }
    }

    void initCastBarWithWP()
    {
        //   Creates Castbar with target being a world point Vector3
        if (UIManager.Instance == null)
        {
            return;
        }
        if (gameObject.tag == "Player")
        { // For player
          //Creating cast bar and setting it's parent to canvas to display it properly

            GameObject newAbilityCast = Instantiate(UIManager.Instance.castBarPrefab, UIManager.Instance.canvas.transform);

            //                                   v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
            newAbilityCast.GetComponent<CastBar>().Init(QueuedAbility.getName(), this, QueuedRelWP.Value, QueuedAbility.getCastTime());
        }
        else
        {// For NPCs
            if (showDebug)
                Debug.Log(actor.ActorName + " starting cast: " + QueuedAbility.getName());

            //gameObject.AddComponent<CastBarNPC>().Init(queuedAbility.getName(), this, queuedRelWP.Value, queuedAbility.getCastTime());
        }
    }

    public bool CastAbilityRealWPs(Ability_V2 _ability, Actor _target = null, NullibleVector3 _WP = null, NullibleVector3 _WP2 = null)
    {
        if (_WP != null)
        {
            _WP = new NullibleVector3(_WP.Value - transform.position);
        }
        if (_WP2 != null)
        {
            _WP2 = new NullibleVector3(_WP2.Value - transform.position);
        }
        return CastAbility3(_ability, _target, _WP, _WP2);
    }

    public void castRelativeToGmObj(Ability_V2 _ability, GameObject _obj, Vector2 _point)
    {
        NullibleVector3 nVectRelToActor = new NullibleVector3();
        nVectRelToActor.Value = _obj.transform.position + (Vector3)_point;
        nVectRelToActor.Value = nVectRelToActor.Value - transform.position;
        CastAbility3(_ability, _relWP: nVectRelToActor);
    }
    // Casting: Castbar handling + Firing a cast---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void UpdateCast()
    {
        if (!IsCasting)
        {
            return;
        }
        if (isServer)
        {
            if ((GetComponent<Controller>().TryingToMove) && (!QueuedAbility.castWhileMoving))
            {
                Debug.Log("cast movement cancel");
                RpcResetCastVarsGCDReset();
            }
        }

        if (IsChanneling)
        {
            castTime -= Time.deltaTime;
            lastChannelTick += Time.deltaTime;
        }
        else
        {
            castTime += Time.deltaTime;
            if (isServer && castTime >= QueuedAbility.getCastTime())
            {
                //Debug.Log("updateCast: readyToFire = true");
                ReadyToFire = true;
            }
        }
    }

    [Server]
    void HandleCastQueue()
    {
        // Called every Update() to see if queued spell is ready to fire

        if (ReadyToFire)
        {
            //Debug.Log("castCompleted: " + queuedAbility.getName());
            if ((QueuedAbility.NeedsTargetActor()) && (QueuedAbility.NeedsTargetWP()))
            {
                Debug.Log("Cast that requires Actor and WP not yet supported. clearing queue.");
                RpcResetCastVarsGCDReset();
            }
            else if (QueuedAbility.NeedsTargetWP())
            {
                FireCast(QueuedAbility, null, QueuedRelWP, QueuedRelWP2);
            }
            else
            {
                FireCast(QueuedAbility, QueuedTarget);
            }
        }
    }

    [Server]
    public void FireCast(Ability_V2 _ability, Actor _target = null, NullibleVector3 _relWP = null, NullibleVector3 _relWP2 = null)
    {
        // EI_Clones will be passed into an event that will allow them to be modified as need by other effects, stats, Buffs, etc.
        // Debug.Log("FireCast()");
        if (_ability.isChannel)
        {
            StartChannel(_ability, _target, _relWP, _relWP2);
            return;
        }

        // if(MirrorTestTools._inst != null)
        //     MirrorTestTools._inst.ClientDebugLog(_ability.getName()+ "| Host Starting fireCast");
        List<EffectInstruction> EI_clones = _ability.getEffectInstructions().cloneInstructs();
        if (buffs != null)
        {
            foreach (EffectInstruction eI in EI_clones)
            {
                int i = 0;
                int lastBuffCount = buffs.Count;
                while (i < buffs.Count)
                {
                    var buffCastHooks = buffs[i].onCastHooks;
                    if (buffCastHooks != null)
                    {
                        if (buffCastHooks.Count > 0)
                        {
                            foreach (var hook in buffCastHooks)
                            {
                                hook.Invoke(buffs[i], eI);
                            }
                        }
                    }

                    if (lastBuffCount == buffs.Count)
                        i++;
                }
            }
        }

        if (!isServer) { return; }

        //Debug.Log("firecast -> isServer");
        if (actor.HasTheResources(_ability))
        {
            foreach (AbilityResource ar in _ability.resourceCosts)
            {
                actor.damageResource(ar.crType, ar.amount);
            }
            // MirrorTestTools._inst.ClientDebugLog(_ability.getName() + " sending effects in fireCast");

            foreach (EffectInstruction eI in EI_clones)
            {
                eI.sendToActor(_target, actor.GetRealWPOrNull(_relWP), actor, inTargetWP2: actor.GetRealWPOrNull(_relWP2));
            }
            foreach (BuffScriptableObject buff in _ability.buffs)
            {
                _target.buffHandler.AddBuff(buff);
            }
            AddToCooldowns(_ability);
            if (onAbilityCastHooks != null)
            {
                onAbilityCastHooks.Invoke(_ability.id);
            }
            if (gameObject.tag == "Player")
            {
                AnimateAbility(_ability);
            }
        }

        else
        {
            Debug.Log(actor.ActorName + " doesn't have the resources: fireCast");
        }

        if (_ability.getCastTime() > 0.0f)
        {
            //When the game is running a window seems to break if an instant ability (Like autoattack)
            //goes off closely before a casted ability. So this check was implemented to fix it

            RpcResetCastVarsGCDReset();
        }
        if (HBCTools.areHostle(actor, _target))
        {
            GetComponent<Controller>().autoAttacking = true;
        }
    }

    public void StartChannel(Ability_V2 _ability, Actor _target = null, NullibleVector3 _relWP = null, NullibleVector3 _relWP2 = null)
    {
        // EI_Clones will be passed into an event that will allow them to be modified as need by other effects, stats, Buffs, etc.
        if (_ability.isChannel == false)
        {
            //wtf how?
            Debug.Log(_ability.getName() + "is not a channel BUT is being channeled");
        }

        QueueAbility(_ability, _target, _relWP, _relWP2);
        if(QueuedAbility.getCastTime() <= 0.0f)
        {
            PrepCast();
        }

        if (QueuedAbility.isChannel == false)
        {
            //wtf how?
            IsChanneling = true;
        }
        else
        {
            IsChanneling = true;
        }

        lastChannelTick = 0.0f;
        ReadyToFire = false;
        castTime = _ability.channelDuration;
        IsCasting = true;
        if (onAbilityCastHooks != null)
        {
            onAbilityCastHooks.Invoke(_ability.id);
        }
        if (QueuedAbility.isChannel == false)
        {
            //wtf how?
            Debug.Log(QueuedAbility.getName() + "is queued and about to be channeled BUT isn't a channel| _ability " + _ability.getName());
        }
        FireChannel(QueuedAbility, QueuedTarget, QueuedRelWP, QueuedRelWP2);

    }
    //2nd part
    public void CheckChannel()
    {
        if (!IsCasting)
        {
            RpcResetCastVars();
            return;
        }

        //check for final hit
        if (castTime <= 0.0f)
        {
            FireChannel(QueuedAbility, QueuedTarget, QueuedRelWP, QueuedRelWP2);
            lastChannelTick = 0.0f;
            RpcResetCastVars();
        }
        //check for middle hits
        else if (QueuedAbility.channelDuration / (QueuedAbility.numberOfTicks - 1) <= lastChannelTick)
        {
            FireChannel(QueuedAbility, QueuedTarget, QueuedRelWP, QueuedRelWP2);
            lastChannelTick = 0.0f;
        }
    }

    [Server]
    public void FireChannel(Ability_V2 _ability, Actor _target = null, NullibleVector3 _relWP = null, NullibleVector3 _relWP2 = null)
    {
        // EI_Clones will be passed into an event that will allow them to be modified as need by other effects, stats, Buffs, etc.
        List<EffectInstruction> EI_clones = _ability.getEffectInstructions().cloneInstructs();
        if (buffs != null)
        {
            foreach (EffectInstruction eI in EI_clones)
            {
                int i = 0;
                int lastBuffCount = buffs.Count;
                while (i < buffs.Count)
                {
                    var buffCastHooks = buffs[i].onCastHooks;
                    if (buffCastHooks != null)
                    {
                        if (buffCastHooks.Count > 0)
                        {
                            foreach (var hook in buffCastHooks)
                            {
                                hook.Invoke(buffs[i], eI);
                            }
                        }
                    }

                    if (lastBuffCount == buffs.Count)
                        i++;
                }
            }
        }

        if (!isServer) { return; }

        //Debug.Log("firecast -> isServer");
        if (actor.HasTheResources(_ability))
        {
            foreach (AbilityResource ar in _ability.resourceCosts)
            {
                actor.damageResource(ar.crType, ar.amount);
            }

            foreach (EffectInstruction eI in EI_clones)
            {
                eI.sendToActor(_target, actor.GetRealWPOrNull(_relWP), actor, inTargetWP2: actor.GetRealWPOrNull(_relWP2));
            }
            //addToCooldowns(_ability);
            // if(onAbilityCastHooks != null){
            //     onAbilityCastHooks.Invoke(_ability.id);
            // }
            if (gameObject.tag == "Player")
            {
                AnimateAbility(_ability);
            }
        }

        else
        {
            Debug.Log(actor.ActorName + " doesn't have the resources: fireChannel");
        }
    }

    // Cooldowns---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    [TargetRpc]
    void TRpcUpdateCooldowns(List<AbilityCooldown> hostListACs)
    {
        // Debug.Log("Updating cooldows hostListACs count: " +  hostListACs.Count.ToString());
        // hostListACs.Capacity = 100;
        abilityCooldowns = hostListACs;
    }
    [Client]
    [TargetRpc]
    void TRpcUpdateCooldown(AbilityCooldown hostAC)
    {
        abilityCooldowns.Add(hostAC);
    }

    void UpdateCooldowns()
    {
        if (abilityCooldowns.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < abilityCooldowns.Count; i++)
        {
            if (abilityCooldowns[i].remainingTime > 0)
                abilityCooldowns[i].remainingTime -= Time.deltaTime;
            else
            {
                abilityCooldowns.RemoveAt(i);
                UIManager.removeCooldownEvent.Invoke(i);
                i--;
            }
        }
    }

    void AddToCooldowns(Ability_V2 _ability)
    {
        // int capacityBefore = abilityCooldowns.Capacity;
        // AbilityCooldown refBefore = null;
        float cooldown = _ability.getCooldown();
        if (Mathf.Approximately(cooldown, 0) || cooldown < 0)
        {
            return;
        }

        AbilityCooldown acRef = new AbilityCooldown(_ability);
        abilityCooldowns.Add(acRef);
        // Debug.Log("before: " + capacityBefore + "after: " + abilityCooldowns.Capacity);
        //Debug.Log("Host: Updating client cooldowns. Length of list: " + abilityCooldowns.Count);
        if (tag == "Player" && !isLocalPlayer)
        {
            TRpcUpdateCooldown(acRef);
        }

    }

    [TargetRpc]
    public void TRpcRefundCooldown(Ability_V2 _hostAbility, float _hostflatAmt)
    {
        RefundCooldown(_hostAbility, _hostflatAmt);
    }

    public void RefundCooldown(Ability_V2 _toRefund, float flatAmt)
    {
        for (int i = 0; i < abilityCooldowns.Count; i++)
        {
            if (abilityCooldowns[i].abilityName == _toRefund.getName())
            {
                abilityCooldowns[i].remainingTime -= flatAmt;
                if (abilityCooldowns[i].remainingTime <= 0)
                {
                    abilityCooldowns.RemoveAt(i);
                    UIManager.removeCooldownEvent.Invoke(i);
                }
                if (isServer && !isLocalPlayer && tag == "Player")
                {
                    //Debug.Log("Server: Telling client to refund cooldown");
                    TRpcRefundCooldown(_toRefund, flatAmt);
                }

                return;
            }
        }
    }

    public bool CheckOnCooldown(Ability_V2 _ability)
    {
        if (_ability == null)
        {
            Debug.Log("The ability you are checking on cd is null");
            return true;
        }
        if (abilityCooldowns.Count <= 0)
        {
            return false;
        }
        for (int i = 0; i < abilityCooldowns.Count; i++)
        {
            if (abilityCooldowns[i].getName() == _ability.getName())
            {
                if (showDebug)
                {
                    //Debug.Log(_ability.getName() + " is on cooldown!");
                }

                return true;
            }
        }
        return false;
    }

    public bool CheckOnGCD()
    {
        if (GetComponent<Controller>().globalCooldown > 0.0f)
        {
            //Debug.Log(actorName + " is on gcd");
            return true;
        }
        else
        {
            //Debug.Log(actorName + " Not on gcd");
            return false;
        }
    }

    /// <summary>
    /// Return true if the ability is on cooldown. False if off cooldown.
    /// </summary>
    public bool CheckCooldownAndGCD(Ability_V2 _ability)
    {
        if (_ability.offGDC == false && CheckOnGCD()) //if its a oGCD chekc if on GCD
        {
            return true;
        }
        if (CheckOnCooldown(_ability))
        { // If we make it through check the ability cd
            return true;
        }

        return false; //if we make it here we are good the GCD and ability not on CD
    }

    public bool CheckRange(Ability_V2 _ability, Vector2 _target)
    {
        if (Vector2.Distance(transform.position, _target) > _ability.range)
        {
            return false;
        }
        return true;
    }

    public void InterruptCast(object sender = null, EventArgs e = null)
    {
        if (QueuedAbility == null)
        {
            return;
        }
        Debug.Log(QueuedAbility.getName() + " was interrupted!");
        RpcResetCastVars();
        //Make cast bar red for a sec or two
    }

    // Casting: Reseting---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void ResetCastVars()
    {
        ResetQueue();
        ReadyToFire = false;
        IsCasting = false;
        IsChanneling = false;
        ResetCastTime();
    }
    [ClientRpc]
    public void RpcResetCastVars()
    {
        ResetCastVars();
    }
    [ClientRpc]
    public void RpcResetCastVarsGCDReset()
    {
        ResetCastVars();
        GetComponent<Controller>().globalCooldown = 0.0f;
    }

    void ResetQueue()
    {
        QueuedTarget = null;
        QueuedRelWP = null;
        QueuedRelWP2 = null;
        QueuedAbility = null;
    }

    void ResetCastTime()
    {
        IsCasting = false;
        castTime = 0.0f;
    }

    [ClientRpc]
    void AnimateAbility(Ability_V2 _ability)
    {
        //animator.SetInteger("abilityType", ((int)_ability.abilityTag));
        if (_ability.abilityTag == AbilityTags.Weapon)
        {
            animator.SetTrigger("abilityCast");
        }
        if (_ability.abilityTag == AbilityTags.SpecialWeapon)
        {
            animator.SetTrigger("SpecialWeapon");
        }
    }
}
