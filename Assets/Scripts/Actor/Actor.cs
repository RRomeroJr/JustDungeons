using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
     Container many for any RPG related elements
*/

public class Actor : MonoBehaviour
{
    public bool showDebug = false;
    public string actorName;
    public int health;
    public int maxHealth;
    public float mana;
    public float maxMana;
    public Actor target;
    public Color unitColor;
    public List<AbilityEffect> abilityEffects;
    
    // When castReady is true queueAbility will fire
    public bool castReady = false; // Will True by CastBar for abilities w/ casts. Will only be true for a freme
    public bool isCasting = false; // Will only be set False by CastBar 
    public Ability queuedAbility;
    public Actor queuedTarget;
    public List<AbilityCooldown> abilityCooldowns = new List<AbilityCooldown>();
    public UIManager uiManager;
    public GameObject abilityDeliveryPrefab;
    //public GameObject testParticlesPrefab;



    void Start(){
        abilityEffects = new List<AbilityEffect>();
    }
    void Update(){
        updateCooldowns();
        handleAbilityEffects();
        handleCastQueue();
    }
    //------------------------------------------------------------handling Active Ability Effects-------------------------------------------------------------------------
    
    void handleAbilityEffects(){

        if(abilityEffects.Count > 0){

            for(int i = 0; i < abilityEffects.Count; i++){

                switch(abilityEffects[i].getEffectType()){
                    case 0: // damage
                        handleDamage(abilityEffects[i]);

                        break;

                    case 1: // heal
                        handleHeal(abilityEffects[i]);
                        break;
                    case 2: // DoT
                        if(abilityEffects[i].start){ 
                        // if this isn't the first frame

                            //Iterate duration
                            abilityEffects[i].remainingTime -= Time.deltaTime;
                            //Iterate lastTick
                            abilityEffects[i].lastTick += Time.deltaTime;

                            if(abilityEffects[i].lastTick >= abilityEffects[i].getTickRate()){
                                // if rdy to tick
                                //Spawn particles
                                if(abilityEffects[i].particles != null)
                                    Instantiate(abilityEffects[i].particles, gameObject.transform);
                                handleDoT(abilityEffects[i]);
                                //Debug.Log("Actor: ticking" + abilityEffects[i].getEffectName() + " on " + actorName);
                            }
                        }
                        else{
                            handleDoT(abilityEffects[i]);
                            abilityEffects[i].start = true;
                        }
                        break;
                    case 3: // HoT
                        if(abilityEffects[i].start){
                        // if this isn't the first frame
                            //Iterate duration
                            abilityEffects[i].remainingTime -= Time.deltaTime;
                            //Iterate lastTick
                            abilityEffects[i].lastTick += Time.deltaTime;

                            if(abilityEffects[i].lastTick >= abilityEffects[i].getTickRate()){
                                // if rdy to tick

                                handleHoT(abilityEffects[i]);
                               // Debug.Log("Actor: ticking" + abilityEffects[i].getEffectName() + " on " + actorName);
                            }
                            
                        }
                        else{
                            handleHoT(abilityEffects[i]);
                            abilityEffects[i].start = true;
                        }
                        break;
                    default:
                        if(showDebug)
                        Debug.Log("Unknown Ability type on " + actorName + "! Don't know what to do! Trying to remove..");
                        abilityEffects[i].duration = 0.0f;
                        break;
                
                }
            checkabilityEffectToRemoveAtPos(abilityEffects[i], i);
            }
        //Debug.Log(actorName + " cleared all Ability effects!");
        }
    }
    void damageValue(int amount){
        // Right now this only damages health, but, maybe in the future,
        // This could take an extra param to indicate a different value to "damage"
        // For ex. a Ability that reduces maxHealth or destroys mana

        //Debug.Log("damageValue: " + amount.ToString()+ " on " + actorName);
        health -= amount;

    }

    void restoreValue(int amount){
        // This would be the opposite of damageValue(). Look at that for more details
        
        //Debug.Log("restoreValue: " + amount.ToString()+ " on " + actorName);
        health += amount;

    }

    void checkabilityEffectToRemoveAtPos(AbilityEffect inabilityEffect, int listPos){
        // Remove AbilityEffect is it's duration is <= 0.0f

        if(inabilityEffect.remainingTime <= 0.0f){
            Debug.Log(actorName + ": Removing.. "+ inabilityEffect.getEffectName());
            abilityEffects[listPos].OnEffectFinish(abilityEffects[listPos].caster, this);
            abilityEffects.RemoveAt(listPos);
        }
    }
    public void applyAbilityEffect(AbilityEffect inAbilityEffect, Actor inCaster){

        //Adding AbilityEffect it to this actor's list<AbilityEffect>
        inAbilityEffect.caster = inCaster;
        inAbilityEffect.remainingTime = inAbilityEffect.getDuration();
        abilityEffects.Add(inAbilityEffect);

        inAbilityEffect.OnEffectStart(inCaster, this);
        //Debug.Log("Actor: Applying.." + inAbilityEffect.getEffectName() + " to " + actorName);  

    }
    void handleDamage(AbilityEffect inabilityEffect){// Type 0

        /* 
            In here you could add interesting interactions
            Maybe something like 
            if(ReverseDamageAndHealing)
                then call restoreValue() instead
        */
        
        damageValue( (int) inabilityEffect.getPower() );// likly will change once we have stats

        // For saftey to make sure that the effect is removed from list
        // right aft the effect finishes
        inabilityEffect.remainingTime = 0.0f; 
            
    }
    void handleDoT(AbilityEffect inabilityEffect){// Type 2

        // Do any extra stuff

        damageValue( (int) ( ( inabilityEffect.getTickRate() / (inabilityEffect.getDuration() + inabilityEffect.getTickRate()) ) * inabilityEffect.getPower() ) );// likly will change once we have stats
        //damageValue( (int) inabilityEffect.getPower()  );
        if(inabilityEffect.lastTick >= inabilityEffect.tickRate)
            inabilityEffect.lastTick -= inabilityEffect.tickRate;
            
    }

    void handleHeal(AbilityEffect inabilityEffect){// Type 1

        /* 
            In here you could add interesting interactions
            Maybe something like 
            if(ReverseDamageAndHealing)
                then call damageValue() instead
        */
        
        restoreValue( (int) inabilityEffect.getPower() ); // likly will change once we have stats

        // For saftey to make sure that the effect is removed from list
        // right aft the effect finishes
        inabilityEffect.remainingTime = 0.0f; 
            
    }

    void handleHoT(AbilityEffect inabilityEffect){// Type 3

        // Do any extra stuff

        restoreValue( (int) ( ( inabilityEffect.getTickRate() / inabilityEffect.getDuration() ) * inabilityEffect.getPower() ) );// likly will change once we have stats
        if(inabilityEffect.lastTick >= inabilityEffect.tickRate)
            inabilityEffect.lastTick -= inabilityEffect.tickRate;
            
    }

    //-------------------------------------------------------------------handling casts--------------------------------------------------------------------------

    public void castAbility(Ability inAbility, Actor inTarget = null){
            checkAndQueue(inAbility, inTarget);
    }
    public void forceCastAbility(Ability inAbility, Actor inTarget){
        
        /*
            cast that should ignore cooldowns, cast time and resources?
        */
        
        if(inTarget != null){
            //Debug.Log("A: " + actorName + " casting " + inAbility.getName() + " on " + target.actorName);
            inTarget.applyAbilityEffect(inAbility.getEffect(), this);
            addToCooldowns(queuedAbility);
        }
        else{
            if(showDebug)
            Debug.Log("Actor: " + actorName + " has no target!");
        }

    }
    public void cast(Ability inAbility, Actor inTarget){
        if(inTarget != null){
            //Debug.Log("A: " + actorName + " casting " + inAbility.getName() + " on " + target.actorName);
            //inTarget.applyAbilityEffect(inAbility.getEffect(), this);

            AbilityEffect tempAE_Ref = inAbility.getEffect().clone();
            /*          
                 vV__Pretend below power is being modified by Actor's stats__Vv
            */
            tempAE_Ref.setEffectName(tempAE_Ref.getEffectName() + " (clone)");
            // ============================================================================]

            GameObject delivery = Instantiate(abilityDeliveryPrefab);
            delivery.GetComponent<AbilityDelivery>().init( tempAE_Ref, 0, this, inTarget, 0.01f);
            addToCooldowns(queuedAbility);
            castReady = false;
        }
        else{
            if(showDebug)
            Debug.Log("Actor: " + actorName + " has no target!");
        }

    }
    public void freeCast(Ability inAbility, Actor inTarget){
        if(inTarget != null){
            //Debug.Log("A: " + actorName + " casting " + inAbility.getName() + " on " + target.actorName);
            //inTarget.applyAbilityEffect(inAbility.getEffect(), this);

            AbilityEffect tempAE_Ref = inAbility.getEffect().clone();
            /*          
                 vV__Pretend below power is being modified by Actor's stats__Vv
            */
            tempAE_Ref.setEffectName(tempAE_Ref.getEffectName() + " (clone)");
            // ============================================================================]

            GameObject delivery = Instantiate(abilityDeliveryPrefab);
            delivery.GetComponent<AbilityDelivery>().init( tempAE_Ref, 0, this, inTarget, 0.01f);
            // No cd gfenerated
            // Make not cost resources
            castReady = false;
        }
        else{
            if(showDebug)
            Debug.Log("Actor: " + actorName + " has no target!");
        }

    }
    private void handleCastQueue(){
        if(castReady){
            //Debug.Log("castCompleted: " + queuedAbility.getName()); 
            cast(queuedAbility, queuedTarget);
        }
    }
    public void queueAbility(Ability inAbility, Actor _queuedTarget = null){
        if(isCasting){
            if(showDebug)
            Debug.Log(actorName + " is casting!");
        }
        else{
            if(!castReady){
                
                if(inAbility.needsTarget()){
                    if(_queuedTarget != null){ 
                        
                        startCast(inAbility, _queuedTarget);
                    }
                    else{
                        Debug.Log("Where are we? 222222");
                        if(showDebug == true)
                            Debug.Log(actorName + " doesn't have a target!");
                    }
                }
                else{
                    startCast(inAbility, _queuedTarget);
                }
            }
        }
    }
    void startCast(Ability inAbility, Actor _queuedTarget = null){
        if(inAbility.getCastTime() > 0.0f){ // Casted Ability

            //Debug.Log("Trying to create a castBar for " + inAbility.getName());

            //Preparing variables for cast
            queuedAbility = inAbility;
            queuedTarget = _queuedTarget;
            castReady = false; // for saftey. Should've been set by castBar or initialized that way already
            isCasting = true;

            
            if(gameObject.tag == "Player"){ // For player
                //Creating cast bar and setting it's parent to canvas to display it properly

                GameObject newAbilityCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);
                //                                   v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
                newAbilityCast.GetComponent<CastBar>().Init(inAbility.getName(), this, target, inAbility.getCastTime());
            }
            else{// For NPCs
                if(showDebug)
                Debug.Log(actorName + " starting cast: " + inAbility.getName());
                gameObject.AddComponent<CastBarNPC>().Init(inAbility.getName(), this, target, inAbility.getCastTime());
            }

        }
        else{
            if(showDebug)
                Debug.Log("GM| Instant cast: " + inAbility.getName());
            queuedAbility = inAbility;
            queuedTarget = _queuedTarget;
            castReady = true;
        }
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
    void addToCooldowns(Ability _ability){
        abilityCooldowns.Add(new AbilityCooldown(queuedAbility));
    }
    public bool checkOnCooldown(Ability _ability){
        if(abilityCooldowns.Count > 0){
            for(int i = 0; i < abilityCooldowns.Count; i++){
                if(abilityCooldowns[i].getName() == _ability.getName()){
                    if(showDebug)
                        Debug.Log(queuedAbility.getName() + " is on cooldown!");
                    return true;
                }
            }
            return false;
        }
        else{
            return false;
        }
    }
    public void checkAndQueue(Ability _ability, Actor _queuedTarget = null){
        if(checkOnCooldown(_ability) == false){
            queueAbility(_ability, _queuedTarget);
        }
    }
    //-------------------------------------------------------------------other---------------------------------------------------------------------------------------------------------
    float RoundToNearestHalf(float value)
    {
        return MathF.Round(value * 2) / 2;
    }
}

