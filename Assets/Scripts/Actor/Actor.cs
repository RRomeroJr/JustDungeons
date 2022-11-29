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

public class Actor : NetworkBehaviour
{
    [Header ("Set Manually in Prefab if Needed") ]
    
    
    public bool showDebug = false;
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
    public bool readyToFire = false; // Will True by CastBar for abilities w/ casts. Will only be true for a freme
    public bool isCasting = false; // Will be set False by CastBar
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
    public uint silienced = 0;
    public uint tauntImmune = 0;
    
    
    

    [ClientRpc]
    public void updateClassResourceAmount(int index, int _amount){
        classResources[index].amount = _amount;
    }
    [ClientRpc]
    public void updateClassResourceMax(int index, int _max){
        classResources[index].max = _max;
    }
    public bool damageResource(ClassResourceType _crt, int _amount){
        if(_amount < 0){
            Debug.Log("Swapping to dmgResource bc amount was < 0");
            return restoreResource(_crt, -_amount);
        }
         if(_crt != null){
            // if(_crt == AbilityResourceTypes.Health){
            //     setHealth(actor.getHealth() - _cost.amount);
            //     return true;
            // }
            // else{
                int index = 0;
                foreach(ClassResource cr in classResources){
                    if(_crt == cr.crType){
                        int diff = cr.amount - _amount;
                        if(diff >= 0){
                            updateClassResourceAmount(index, diff);
                        }
                        else{
                            updateClassResourceAmount(index, 0);
                        }
                        return true;
                    }
                    index++;
                }
            // }
        }
        return false;
    }
    public bool restoreResource(ClassResourceType _crt, int _amount){
        if(_amount < 0){
            Debug.Log("Swapping to dmgResource bc amount was < 0");
            return damageResource(_crt, -_amount);
        }
         if(_crt != null){
            // if(_crt == AbilityResourceTypes.Health){
            //     setHealth(actor.getHealth() - _cost.amount);
            //     return true;
            // }
            // else{
                int index = 0;
                foreach(ClassResource cr in classResources){
                    if(_crt == cr.crType){ //if _crt is a resource that we have
                        int sum = cr.amount + _amount; //Then get the sum of it + what we got
                        
                        if(sum <= cr.max){
                            //then add it to our amount
                            Debug.Log("Adding " + _amount.ToString() + " " + _crt.GetType().ToString());
                            updateClassResourceAmount(index, sum);
                        }
                        else{
                            //set our cr.amount to max
                            updateClassResourceAmount(index, cr.max);
                        }
                        return true;
                    }
                    index++;
                }
            // }
        }
        return false;
    }
    void Start(){
        
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if(isLocalPlayer){
            UIManager.playerActor = this;
            Nameplate.Create(this);
        }
        animator = GetComponent<Animator>();
        //gameObject.GetComponent<Renderer>().Color = unitColor;
        //Nameplate.Create(this);


    }
    void Update(){
        if(health <= 0){
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
        handleAbilityEffects();
        if(isServer){
            handleCastQueue();
            
            if(isChanneling){
                checkChannel();
            }
        }
        
    }
    //------------------------------------------------------------handling Active Ability Effects-------------------------------------------------------------------------
    

    [TargetRpc]
    void TRpcUpdateCooldowns(List<AbilityCooldown> hostListACs){

       // Debug.Log("Updating cooldows hostListACs count: " +  hostListACs.Count.ToString());
        abilityCooldowns = hostListACs;
    }
    [TargetRpc]
    void TRpcCreateDamageTextSelf(int amount){
        DamageText.Create(transform.position, amount);
    }
    [TargetRpc]
    void TRpcCreateDamageTextOffensive(NetworkConnection attackingPlayer, int amount){
        //Debug.Log("disiplaying Damage numbers");
        
        DamageText.Create(transform.position, amount);
    }
    [ClientRpc]
    void addDamamgeToMeter(Actor fromActor, int amount){
        TempDamageMeter.addToEntry(fromActor, amount);
    }
    public void damageValue(int amount, int valueType = 0, Actor fromActor = null){
        // Right now this only damages health, but, maybe in the future,
        // This could take an extra param to indicate a different value to "damage"
        // For ex. a Ability that reduces maxHealth or destroys mana

        //Debug.Log("damageValue: " + amount.ToString()+ " on " + actorName);
        if(amount >= 0){
            switch (valueType){
                case 0:
                    health -= amount;
                    if(fromActor != null){
                        if(fromActor.tag == "Player"){
                            TRpcCreateDamageTextOffensive(fromActor.GetNetworkConnection(), amount);
                        }
                    }
                    if(tag == "Player"){
                        TRpcCreateDamageTextSelf(amount);
                    }
                    
                    
                    if(fromActor != null){
                        addDamamgeToMeter(fromActor, amount);
                    }
                    if(health < 0){
                        health = 0;
                    }
                    break;
                case 1:
                    maxHealth -= amount;
                    if(maxHealth < 1){      // Making this 0 might cause a divide by 0 error. Check later
                        maxHealth = 1;
                    }
                    break;
                default:
                    break;
            }
        }
        else{
            Debug.Log("Amount was Neg calling to restoreValue instead");
            restoreValue(-1 * amount, valueType); //if negative call restore instead with amount's sign flipped
        }
    }
    // public bool damageResource(AbilityResourceTypes _arType, int amount){
    //     foreach(ActorResource ar in resources){
    //         if(ar.arType == _arType){
    //             ar.amount -= amount;
    //             if(ar.amount < 0){
    //                 ar.amount = 0;
    //             }
    //             return true;
    //         }
    //     }
    //     return false;
    // }
    // public bool damageResource(AbilityResource _cost){
    //     if(_cost != null){
    //         if(_cost.arType == AbilityResourceTypes.Health){
    //             health -= _cost.amount;
    //             return true;
    //         }
    //         else{
    //             foreach(ActorResource ar in resources){
    //                 if(ar.arType == _cost.arType){
    //                     ar.amount -= _cost.amount;
    //                     if(ar.amount < 0){
    //                         ar.amount = 0;
    //                     }
    //                     return true;
    //                 }
    //             }
    //         }
    //     }
    //     return false;
    // }


    public void restoreValue(int amount, int valueType = 0, Actor fromActor = null){
        // This would be the opposite of damageValue(). Look at that for more details
        //  Maybe in the future calcing healing may have diff formula to calcing damage taken
        
        //Debug.Log("restoreValue: " + amount.ToString()+ " on " + actorName);
        if(amount >= 0){
            switch (valueType){
                case 0:
                    health += amount;
                    if(fromActor != null){
                        if(fromActor.tag == "Player"){
                            TRpcCreateDamageTextOffensive(fromActor.GetNetworkConnection(), amount);
                        }
                    }
                    if(tag == "Player"){
                        TRpcCreateDamageTextSelf(amount);
                    }
                    if(fromActor != null){
                        addDamamgeToMeter(fromActor, amount);
                    }
                    if(health > maxHealth){
                        health = maxHealth;
                    }
                    break;
                case 1:
                    if(maxHealth + amount > maxHealth){ // if int did not wrap around max int
                        maxHealth += amount;    
                    }
                    break;

                default:
                    break;
            }
        }
        else{
            Debug.Log("Amount was Neg calling to damageValue instead");
            damageValue( -1 * amount, valueType); // if negative call damage instead with amount's sign flipped
        }
    }

    
    void checkAbilityEffectToRemoveAtPos(Buff _buff, int listPos){
        // Remove AbilityEffect is it's duration is <= 0.0f

        if(_buff.getRemainingTime() <= 0.0f){
            if(showDebug)
            Debug.Log(actorName + ": Removing.. "+ _buff.getEffectName());
            //buffs[listPos].OnEffectFinish(); // AE has a caster and target now so the args could be null?
            buffs.RemoveAt(listPos);
        }
    }
    public void recieveEffect(EffectInstruction _eInstruct, NullibleVector3 _targetWP, Actor _caster, Actor _secondaryTarget = null){
        
        // foreach (var eI in _eInstructs){
        //     eI.startEffect(this, _targetWP, _caster);
        // }
        int i = 0;
        int lastBuffCount = buffs.Count;
        while(i < buffs.Count)
        {
            var buffhitHooks = buffs[i].onHitHooks;
            if(buffhitHooks != null){
                if(buffhitHooks.GetPersistentEventCount() > 0){
                    Debug.Log("Invokeing onHitHooks: " + buffs[i].getEffectName());
                    buffhitHooks.Invoke(buffs[i], _eInstruct);
                }else{
                    //Debug.Log("Buff has no hooks");
                }
            }

        if(lastBuffCount == buffs.Count)
            i++;

        }
        //Debug.Log(actorName + " is starting eI for effect (" + _eInstruct.effect.effectName + ") From: " + (_caster != null ? _caster.actorName : "none"));
        //Debug.Log("recieveEffect " + _eInstruct.effect.effectName +"| caster:" + (_caster != null ? _caster.getActorName() : "_caster is null"));
        _eInstruct.startEffect(this, _targetWP, _caster, _secondaryTarget);
    }
    public void applyBuff(Buff _buff){
        
        //Adding Buff it to this actor's list<Buff>
        
        if(_buff.getID() >= 0){
            //Check if the buff is already here
            Buff tempBuff_Ref = buffs.Find(b => b.getID() == _buff.getID());

            //if we found something
            if(tempBuff_Ref  != null){// Based on circumstances, I might need you do something different

                if( (tempBuff_Ref.isStackable())&&(tempBuff_Ref.isRefreshable()) ){ // if stackable and refreshable
                    tempBuff_Ref.addStacks(1);
                    tempBuff_Ref.setRemainingTime(tempBuff_Ref.getDuration());
                    //Debug.Log("stting remaing to "+ tempBuff_Ref.getDuration().ToString());
                    return;
                }
                else if(tempBuff_Ref.isStackable()){
                    tempBuff_Ref.addStacks(1);
                    return;
                }
                else if(tempBuff_Ref.isRefreshable()){
                    //Debug.Log("Refreshable");
                    tempBuff_Ref.setRemainingTime(tempBuff_Ref.getDuration()); // Add pandemic time?
                    return;
                }
            }
        }
        else{

        }
        //---------------------If we get this far we are just adding it like normal-------------------------------------
        
        
        
        if(isServer){
            AddNewBuff(_buff);
            RpcAddNewBuff(_buff);
            
        }
        
    }
    [Server]
    public void removeBuff(Buff _callingBuff){
        
        int buffIndex = buffs.FindIndex(x => x == _callingBuff);

        buffs.RemoveAt(buffIndex);
        Debug.Log("Removed index: " + buffIndex);
        RpcRemoveBuffIndex(buffIndex);
    }
    [ClientRpc]
    void RpcRemoveBuffIndex(int hostIndex){
        if(isServer){
            return;
        }
        Debug.Log("Host saying to remove buff index: " + hostIndex);
        buffs.RemoveAt(hostIndex);
    }
    void AddNewBuff(Buff _buff){
        _buff.setActor(this);
        _buff.setRemainingTime(_buff.getDuration());
        buffs.Add(_buff);
    }
    [ClientRpc]
    void RpcAddNewBuff(Buff _buffFromSever){
        if(isServer)
            return;

        AddNewBuff(_buffFromSever);
    }
    void handleAbilityEffects(){

       
        if(buffs.Count > 0){

            int i = 0;
            int lastBuffCount = buffs.Count;
            while(i < buffs.Count){
                //Debug.Log("Buffs[" + i.ToString() + "] = " + buffs[i].getEffectName());
                buffs[i].update();
                if(lastBuffCount == buffs.Count){
                    
                    i++;
                }
                else{
                    ///Debug.Log("After RM Buffs[" + (i - 1).ToString() + "] = " + buffs[i-1].getEffectName());
                    lastBuffCount = buffs.Count;
                }
            }
        
        }
    }

    //Casting----------------------------------------------------------------------------------------------
    public void castRelativeToGmObj(Ability_V2 _ability, GameObject _obj, Vector2 _point){
        NullibleVector3 nVect = new NullibleVector3();
        nVect.Value = _obj.transform.position + (Vector3)_point;
        castAbility3(_ability, _targetWP: nVect);
    }
    
    
    public bool castAbility3(Ability_V2 _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        /*  Returns true if a REQUEST to fire was made. NOT if the cast was actually fired
        */

        //Debug.Log("castAbility3");
        
        if(checkOnCooldown(_ability) == false){ //Will be check on cd later
            // MirrorTestTools._inst.ClientDebugLog(_ability.getName() + "| Not on cool down or GCD");
            if(hasTheResources(_ability)){
                //if ability is magical check silence
                // For now silence works on everything including auto attack
                if(silienced == 0){ 
                    if(!readyToFire){
                        if(!isCasting){
                            if(_ability.NeedsTargetActor()){
                                if(_target == null){
                                //Debug.Log("Try find target..");
                                    _target = tryFindTarget(_ability);
                                }
                                if(_target == null){
                                    Debug.Log("No suitable target found");
                                    return false;
                                }
                                else{
                                    if(!checkRange(_ability, _target.transform.position)){
                                        if(showDebug)
                                            Debug.Log("You are out of range");
                                        return false;
                                    }
                                }
                            }
                            if(_ability.NeedsTargetWP()){
                                if(_targetWP == null){
                                //Debug.Log("Try find target..");
                                    _targetWP = tryFindTargetWP(_ability, _target);
                                }
                                if(_targetWP == null){
                                    Debug.Log("No suitable WP found");
                                    return false;
                                }
                                else{
                                    if(!checkRange(_ability, _targetWP.Value)){
                                        if(showDebug)
                                            Debug.Log("You are out of range");
                                        return false;
                                    }
                                }
                            }
                            //Debug.Log("castAbility3 inner if");
                            // if(isServer){
                            //     serverSays(_ability);
                            // }
                            
                            
                            if(isServer){
                                        //MirrorTestTools._inst.ClientDebugLog("Starting RPC");
                                    rpcStartCast(_ability, _target, _targetWP);
                            }
                            else if(isLocalPlayer){ //isLocalPlayer check may not be necessary
                                    cmdStartCast(_ability, _target, _targetWP);
                                
                            }
                            
                            //Debug.Log("after Ability_V2 command reached");  
                        }
                        else{
                            //Debug.Log(actorName + " is casting!");
                        }         
                    }
                    else{ //readyToFire == true
                        if(showDebug)
                        Debug.Log("Something else is ready to fire and blocking this cast");
                        return false;
                    }
                } //end if(requestingCast)
                else{
                    //Debug.Log(actorName + " try to cast" + _ability + " but is silenced!");
                    return false;
                }
            }
            else{ //hasTheResources() == false
                Debug.Log(actorName + " does not have the resources");
                return false;
            }
        }
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
    [Server]
    public void fireCast(Ability_V2 _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        // EI_Clones will be passed into an event that will allow them to be modified as need by other effects, stats, Buffs, etc.
        // Debug.Log("FireCast()");
        if(_ability.isChannel){
            startChannel(_ability, _target, _targetWP);
        }
        else{
            if(_ability.id == 0){
                 //Debug.Log("Firing an AA");

            }
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
                // i = 0;
                // int lastOnCastCount = onCastHooks.GetPersistentEventCount();
                // while(i < onCastHooks.GetPersistentEventCount() )
                // {
                    
                //     if(onCastHooks != null){
                        
                //         onCastHooks.Invoke(eI);
                        
                //     }

                // if(lastOnCastCount == onCastHooks.GetPersistentEventCount())
                //     i++;

                // }
                
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
                
            
                
        } 
        
        }
        
        
    }
    public void startChannel(Ability_V2 _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        // EI_Clones will be passed into an event that will allow them to be modified as need by other effects, stats, Buffs, etc.
        
        queueAbility(_ability, _target, _targetWP);
        isChanneling = true;
        lastChannelTick = 0.0f;
        readyToFire = false;
        castTime = _ability.channelDuration;
        isCasting = true;
        fireChannel(queuedAbility, queuedTarget, queuedTargetWP);
        
    }
    //2nd part
    public void checkChannel(){
        if(!isCasting){
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
    void resetClientCastVars(){
        resetQueue();
        readyToFire = false;
        isCasting = false;
        resetCastTime();
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
        isCasting = true;
        // if(MirrorTestTools._inst != null)
        //             MirrorTestTools._inst.ClientDebugLog("prepcast() isCasting = " + isCasting.ToString());
        // Creating CastBar or CastBarNPC with apropriate variables   
        if( queuedAbility.NeedsTargetActor() && queuedAbility.NeedsTargetWP() ){
            Debug.Log("Spell that needs an Actor and WP are not yet suported");
            isCasting = false; 
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
    [Server]
    void handleCastQueue(){
        // Called every Update() to see if queued spell is ready to fire

        if(readyToFire){
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
    
    //-------------------------------------------------------------------handling casts--------------------------------------------------------------------------
    
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
        if(GetComponent<Controller>().globalCooldown > 0.0f){
            //Debug.Log(actorName + " is on gcd");
            return true;
        }
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
    public bool checkRange(Ability_V2 _ability, Vector2 _target){
        if(Vector2.Distance(transform.position, _target) > _ability.range){
            return false;
        }
        else{
            return true;
        }
    }
    
    void resetQueue(){
        queuedTarget = null;
        queuedTargetWP = null;
    }
    //------------------------------------------------------------------Setters/ getters---------------------------------------------------------------------------------
    /*public Ability getQueuedAbility(){
        return queuedAbility;
    }*/
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
    
    void updateCast(){
        
        if(isCasting){
            if(isChanneling){
                castTime -= Time.deltaTime;
                lastChannelTick += Time.deltaTime;
            }
            else{
                castTime += Time.deltaTime;
                if(isServer)
                    if(castTime >= queuedAbility.getCastTime()){
                        //Debug.Log("updateCast: readyToFire = true");
                        readyToFire = true;
                    }
            }
            
            
                
            
        }
    }
    void resetCastTime(){
        isCasting = false;
        castTime = 0.0f;
    }
    public float getHealthPercent(){
        return (float)health / (float)maxHealth;
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
    public bool hasTheResources(Ability_V2 _ability){
        if(_ability != null){
            if(_ability.resourceCosts == null){
                return true;
            }
            else{
                if(_ability.resourceCosts.Count == 0){
                    return true;
                }
                foreach(AbilityResource ar in _ability.resourceCosts){
                    if(hasResource(ar.crType, ar.amount) == false){
                        return false;
                        
                    }
                }
            } 
            
        }
        
        return true;
    }
    [ClientRpc]
    public void Knockback(Vector2 _hostVect){
        GetComponent<Rigidbody2D>().AddForce(_hostVect);
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
    public void interruptCast(){

        Debug.Log(queuedAbility.getName() + " was interrupted!");
        resetClientCastVars();
        //Make cast bar red for a sec or two
    }
    #endregion
    //----------------------------------------------------------------old code no longer used------------------------------------------------------------------------------------

    //Old ability stuff-------------------------------------------------------------------------------------------------------------------------
    /*
    public void castAbility(Ability _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        //Main way to make Acotr cast an ability

        if(checkOnCooldown(_ability) == false){
            if(!readyToFire){
                if(checkAbilityReqs(_ability, _target, _targetWP)){
                    if(_ability.getCastTime() > 0.0f){
                        if(!isCasting){
                            // ActorCstingAbilityEvent()
                            queueAbility(_ability, _target, _targetWP);
                            prepCast();
                        }
                    }
                    else{
                        // ActorCastingAbilityEvent.Invoke(_ability)
                        fireCast(_ability, _target, _targetWP);
                    }
                }
                else{
                    if(showDebug)
                        Debug.Log("Ability doesn't have necessary requirments");
                    resetQueue();
                }                   
            }
            else{
                if(showDebug)
                Debug.Log("Something else is ready to fire and blocking this cast");
            }
        }
    }
    public void castAbility(Ability _ability, Vector3 _queuedTargetWP){
        //Main way to make Acotr cast an ability

        if(checkOnCooldown(_ability) == false){
            if(!readyToFire){
                NullibleVector3 tempNullibleVect = _queuedTargetWP;
                if(checkAbilityReqs(_ability, null, tempNullibleVect)){
                    if(_ability.getCastTime() > 0.0f){
                        if(!isCasting){
                            queueAbility(_ability, null, tempNullibleVect);
                            prepCast();
                        }
                    }
                    else{
                        fireCast(_ability, null, tempNullibleVect);
                    }
                }
                else{
                    if(showDebug)
                        Debug.Log("Ability doesn't have necessary requirments: A, WP");
                    resetQueue();
                }                       
            }
            else{
                if(showDebug)
                Debug.Log("Something else is ready to fire and blocking this cast: A, WP");
            }
        }
    }
    public void freeCast(Ability _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        //  Make Acotor cast an ability without starting a cooldown or (in the future) cost resources
        // Maybe make this into an overload of castAbility later

        if(checkAbilityReqs(_ability, _target, _targetWP)){
            if(handleDelivery(_ability, _target)){
                // No cd gfenerated
                // Make not cost resources
                readyToFire = false;
            }
            
        }
        else{
            if(showDebug)
            Debug.Log(actorName + " Free cast failed reqs");
        }

    }
    public void freeCast(Ability _ability, Vector3 _targetWP){
        //  Make Acotor cast an ability without starting a cooldown or (in the future) cost resources
        // Maybe make this into an overload of castAbility later

        if(checkAbilityReqs(_ability, null, _targetWP)){
            if(handleDelivery(_ability, null, _targetWP)){
                // No cd gfenerated
                // Make not cost resources
                readyToFire = false;
            }
            
        }
        else{
            if(showDebug)
            Debug.Log(actorName + " Free cast failed reqs: A, WP");
        }

    }
    void forceCastAbility(Ability _ability, Actor _target){
        
        
        //                                                          ***IGNORE*** Unused for now
        
        
        if(_target != null){
            //Debug.Log("A: " + actorName + " casting " + _ability.getName() + " on " + target.actorName);
            _target.applyAbilityEffects(_ability.createEffects(), this);
            addToCooldowns(queuedAbility);
        }
        else{
            if(showDebug)
            Debug.Log("Actor: " + actorName + " has no target!");
        }

    }

    public void fireCast(Ability _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        // Main way for "Fireing" a cast by creating a delivery if needed then creating an AbilityCooldown
        if(handleDelivery(_ability, _target, _targetWP)){
            addToCooldowns(queuedAbility);
            readyToFire = false;
        }

    }  
    
    
    void queueAbility(Ability _ability, Actor _queuedTarget = null, NullibleVector3 _queuedTargetWP = null){
        //Preparing variables for a cast
        queuedAbility = _ability;
        queuedTarget = _queuedTarget;
        queuedTargetWP = _queuedTargetWP;
    }
    
    
    bool handleDelivery(Ability _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        // Creates delivery if needed. Applies effects to target if not
        // ***** WILL RETURN FALSE if DeliveryType is -1 (auto apply to target) and there is no target *****
        
            if(_ability.getDeliveryType() == -1){
                if(_target != null){
                    _target.applyAbilityEffects(createEffects(_ability), this);
                    return true;
                }
                else{
                    Debug.Log(actorName + ": Type -1 Actor to Actor Delivery with no target");
                    return false;
                }
            }
            else if(_ability.getDeliveryType() == -2){
                if(_targetWP != null){
                    this.applyAbilityEffects(_ability.createEffects(_targetWP.Value), this);
                    return true;
                }
                else{
                    Debug.Log(actorName + ": Type -2 Actor to WP with no WP");
                    return false;
                }
            }
            else{
                GameObject delivery = CreateAndInitDelivery(_ability, _target, _targetWP);
                return true;
            }        
    }
    GameObject CreateAndInitDelivery(Ability _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        // Creates and returns delivery

        GameObject delivery;

        if( (_ability.NeedsTargetActor()) && (_ability.NeedsTargetWP()) ){
            Debug.Log("Spell With Actor and WP reqs not yet suported");
            delivery = null;
        }
        else if(_ability.NeedsTargetActor()){
            delivery = Instantiate(abilityDeliveryPrefab, gameObject.transform.position, gameObject.transform.rotation);
            delivery.GetComponent<AbilityDelivery>().init( modEffects(createEffects(_ability)), _target, _ability.getDeliveryType(), this, _ability.getSpeed());
            return delivery;
        }
        else if(_ability.NeedsTargetWP()){

            delivery = Instantiate(abilityDeliveryPrefab, gameObject.transform.position, gameObject.transform.rotation);
            delivery.GetComponent<AbilityDelivery>().init( modEffects(createEffects(_ability)), _targetWP.Value, _ability.getDeliveryType(), this, _ability.getSpeed(), _ability.getDuration());
            return delivery;
        }
        delivery = null;
        return delivery;
    }
    
    
    bool checkAbilityReqs(Ability _ability, Actor _target = null, NullibleVector3 _targetWP = null){
        // Checks if the requirments of _ability are satisfied

        //Debug.Log(_ability.NeedsTargetActor().ToString() + " " + _ability.NeedsTargetWP().ToString());
        if( (_ability.NeedsTargetActor()) && (_ability.NeedsTargetWP()) ){
            //Debug.Log(_ability.getName() + " Needs BOTH a target and WP");
            return ( (_target != null) && (_targetWP != null) );
        }
        else if(_ability.NeedsTargetActor()){
            //Debug.Log(_ability.getName() + " Needs only a target");
            return _target != null;
        }
        else if(_ability.NeedsTargetWP()){
            //Debug.Log(_ability.getName() + " Needs only a WP");
            return _targetWP != null;
        }
        else{
            return true;
        }
    }
    void addToCooldowns(Ability _ability){
        abilityCooldowns.Add(new AbilityCooldown(queuedAbility));
    }
    
    public bool checkOnCooldown(Ability _ability){
        if(abilityCooldowns.Count > 0){
            for(int i = 0; i < abilityCooldowns.Count; i++){
                if(abilityCooldowns[i].getName() == _ability.getName()){
                    if(showDebug)
                        Debug.Log(_ability.getName() + " is on cooldown!");
                    return true;
                }
            }
            return false;
        }
        else{
            return false;
        }
    }
    
    List<AbilityEffect> createEffects(Ability _ability){
        List<AbilityEffect> temp;
        temp = _ability.createEffects(this);
        if(aeFireEvent != null){
            aeFireEvent.Invoke(temp);
        }
        return temp;
    }
    AbilityEffect modEffects(AbilityEffect _ae){


        //
        //       *** _ae's effects stats get modified here***
        //             Based on stats and/ or certain buffs
        //                      this actor has

        return _ae;
    }
    List<AbilityEffect> modEffects(List<AbilityEffect> _listAE){


        //
        //       *** _listAE's effects stats get modified here***
        //             Based on stats and/ or certain buffs
        //                      this actor has

        return _listAE;
    }
    //Old AbilityEffect stuff--------------------------------------------------------------------------------------------------------------------
    
    void checkAbilityEffectToRemoveAtPos(AbilityEffect _abilityEffect, int listPos){
        // Remove AbilityEffect is it's duration is <= 0.0f

        if(_abilityEffect.getRemainingTime() <= 0.0f){
            if(showDebug)
            Debug.Log(actorName + ": Removing.. "+ _abilityEffect.getEffectName());
            abilityEffects[listPos].OnEffectFinish(); // AE has a caster and target now so the args could be null?
            abilityEffects.RemoveAt(listPos);
        }
    }
    public void RemoveActiveEffect(Predicate<AbilityEffect> pred){
        // Remove AbilityEffect is it's duration is <= 0.0f
        int temp = abilityEffects.FindIndex(pred);
        
        if(temp >= 0){
            Debug.Log(actorName + ": Manually removing at.. "+ temp.ToString());
            abilityEffects[temp].OnEffectFinish(); // AE has a caster and target now so the args could be null?
            abilityEffects.RemoveAt(temp);
        }
        else{
            Debug.Log("ae was not found:");
        }
    }
    public void applyAbilityEffect(AbilityEffect _abilityEffect, Actor inCaster){

        //Adding AbilityEffect it to this actor's list<AbilityEffect>
        
        if(_abilityEffect.getDuration() > 0.0f){
            //Debug.Log("Effect has dur. Checking to stack/ refresh");
            AbilityEffect tempAE_Ref = abilityEffects.Find(ae => ae.getID() == _abilityEffect.getID());
            if(tempAE_Ref != null){
                if( (tempAE_Ref.isStackable())&&(tempAE_Ref.isRefreshable()) ){ // if stackable and refreshable
                    tempAE_Ref.addStacks(1);
                    tempAE_Ref.setRemainingTime(tempAE_Ref.getDuration());
                    return;
                }
                else if(tempAE_Ref.isStackable()){
                    tempAE_Ref.addStacks(1);
                    return;
                }
                else if(tempAE_Ref.isRefreshable()){
                    //Debug.Log("Refreshable");
                    tempAE_Ref.setRemainingTime(tempAE_Ref.getDuration()); // Add pandemic time?
                    return;
                }
            }
        }
        //_abilityEffect.setCaster(inCaster);
        _abilityEffect.setTarget(this);
        _abilityEffect.setRemainingTime(_abilityEffect.getDuration());
        //Debug.Log(_abilityEffect.getRemainingTime().ToString() + " " + _abilityEffect.getDuration().ToString());
        _abilityEffect.OnEffectStart();
        abilityEffects.Add(_abilityEffect);
        _abilityEffect.setStart(true);

        // AE has a caster and target now so the args could be null?
        //Debug.Log("Actor: Applying.." + _abilityEffect.getEffectName() + " to " + actorName);  

    }
    public void applyAbilityEffects(List<AbilityEffect> _abilityEffects, Actor inCaster){
        if(_abilityEffects.Count > 0){
            for(int i = 0; i < _abilityEffects.Count; i++ ){
                applyAbilityEffect(_abilityEffects[i], inCaster);
            }
        } 

    }*/
}

