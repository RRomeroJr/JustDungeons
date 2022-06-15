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
    public int health; // RR: This was changed to FloatReference had to change it back bc it caused a bunch of errors
    public int maxHealth;
    public float mana;
    public float maxMana;
    public Actor target;
    public Color unitColor;
    public List<ActiveAbilityEffect> activeAbilityEffects;
    
    // When castReady is true queueAbility will fire
    public bool castReady = false; // Will True by CastBar for abilities w/ casts. Will only be true for a freme
    public bool isCasting = false; // Will only be set False by CastBar 
    public Ability queuedAbility;
    public List<AbilityCooldown> abilityCooldowns = new List<AbilityCooldown>();
    public UIManager uiManager;

    //public GameObject testParticlesPrefab;


    void Start(){
        activeAbilityEffects = new List<ActiveAbilityEffect>(); 
    }
    void Update(){
        updateCooldowns();
        handleAbilityEffects();
        handleCast();
    }
    //------------------------------------------------------------handling Active Ability Effects-------------------------------------------------------------------------
    
    void handleAbilityEffects(){

        if(activeAbilityEffects.Count > 0){

            for(int i = 0; i < activeAbilityEffects.Count; i++){

                switch(activeAbilityEffects[i].getEffectType()){
                    case 0: // damage
                        handleDamage(activeAbilityEffects[i]);

                        break;

                    case 1: // heal
                        handleHeal(activeAbilityEffects[i]);
                        break;
                    case 2: // DoT
                        if(activeAbilityEffects[i].start){ 
                        // if this isn't the first frame

                            //Iterate duration
                            activeAbilityEffects[i].remainingTime -= Time.deltaTime;
                            //Iterate lastTick
                            activeAbilityEffects[i].lastTick += Time.deltaTime;

                            if(activeAbilityEffects[i].lastTick >= activeAbilityEffects[i].getTickRate()){
                                // if rdy to tick
                                //Spawn particles
                                if(activeAbilityEffects[i].particles != null)
                                    Instantiate(activeAbilityEffects[i].particles, gameObject.transform);
                                handleDoT(activeAbilityEffects[i]);
                                //Debug.Log("Actor: ticking" + activeAbilityEffects[i].getEffectName() + " on " + actorName);
                            }
                        }
                        else{
                            handleDoT(activeAbilityEffects[i]);
                            activeAbilityEffects[i].start = true;
                        }
                        break;
                    case 3: // HoT
                        if(activeAbilityEffects[i].start){
                        // if this isn't the first frame
                            //Iterate duration
                            activeAbilityEffects[i].remainingTime -= Time.deltaTime;
                            //Iterate lastTick
                            activeAbilityEffects[i].lastTick += Time.deltaTime;

                            if(activeAbilityEffects[i].lastTick >= activeAbilityEffects[i].getTickRate()){
                                // if rdy to tick

                                handleHoT(activeAbilityEffects[i]);
                               // Debug.Log("Actor: ticking" + activeAbilityEffects[i].getEffectName() + " on " + actorName);
                            }
                            
                        }
                        else{
                            handleHoT(activeAbilityEffects[i]);
                            activeAbilityEffects[i].start = true;
                        }
                        break;
                    default:
                        if(showDebug)
                        Debug.Log("Unknown Ability type on " + actorName + "! Don't know what to do! Trying to remove..");
                        activeAbilityEffects[i].duration = 0.0f;
                        break;
                
                }
            checkAAEToRemoveAtPos(activeAbilityEffects[i], i);
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

    void checkAAEToRemoveAtPos(ActiveAbilityEffect inAAE, int listPos){
        // Remove ActiveAbilityEffect is it's duration is <= 0.0f

        if(inAAE.remainingTime <= 0.0f){
            //Debug.Log(actorName + ": Removing.. "+ inAAE.getEffectName());
            activeAbilityEffects.RemoveAt(listPos);
        }
    }
    public void applyAbilityEffect(AbilityEffect inAbilityEffect, Actor inCaster){
        //Creates an ActiveAbillityEffect and adds it to this actor's list<ActiveAbilityEffect>

        activeAbilityEffects.Add(new ActiveAbilityEffect(inAbilityEffect, inCaster));
        //Debug.Log("Actor: Applying.." + inAbilityEffect.getEffectName() + " to " + actorName);  

    }
    void handleDamage(ActiveAbilityEffect inAAE){// Type 0

        /* 
            In here you could add interesting interactions
            Maybe something like 
            if(ReverseDamageAndHealing)
                then call restoreValue() instead
        */
        
        damageValue( (int) inAAE.getPower() );// likly will change once we have stats

        // For saftey to make sure that the effect is removed from list
        // right aft the effect finishes
        inAAE.remainingTime = 0.0f; 
            
    }
    void handleDoT(ActiveAbilityEffect inAAE){// Type 2

        // Do any extra stuff

        damageValue( (int) ( ( inAAE.getTickRate() / (inAAE.getDuration() + inAAE.getTickRate()) ) * inAAE.getPower() ) );// likly will change once we have stats
        //damageValue( (int) inAAE.getPower()  );
        if(inAAE.lastTick >= inAAE.tickRate)
            inAAE.lastTick -= inAAE.tickRate;
            
    }

    void handleHeal(ActiveAbilityEffect inAAE){// Type 1

        /* 
            In here you could add interesting interactions
            Maybe something like 
            if(ReverseDamageAndHealing)
                then call damageValue() instead
        */
        
        restoreValue( (int) inAAE.getPower() ); // likly will change once we have stats

        // For saftey to make sure that the effect is removed from list
        // right aft the effect finishes
        inAAE.remainingTime = 0.0f; 
            
    }

    void handleHoT(ActiveAbilityEffect inAAE){// Type 3

        // Do any extra stuff

        restoreValue( (int) ( ( inAAE.getTickRate() / inAAE.getDuration() ) * inAAE.getPower() ) );// likly will change once we have stats
        if(inAAE.lastTick >= inAAE.tickRate)
            inAAE.lastTick -= inAAE.tickRate;
            
    }

    //-------------------------------------------------------------------handling casts--------------------------------------------------------------------------

    public void castAbility(Ability inAbility, Actor inTarget){
            checkAndQueue(inAbility);
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
    private void cast(Ability inAbility, Actor inTarget){
        if(inTarget != null){
            //Debug.Log("A: " + actorName + " casting " + inAbility.getName() + " on " + target.actorName);
            inTarget.applyAbilityEffect(inAbility.getEffect(), this);
            addToCooldowns(queuedAbility);
            castReady = false;
        }
        else{
            if(showDebug)
            Debug.Log("Actor: " + actorName + " has no target!");
        }

    }
    private void handleCast(){
        if(castReady){
            //Debug.Log("castCompleted: " + queuedAbility.getName()); 
            cast(queuedAbility, target);
        }
    }
    public void queueAbility(Ability inAbility){
        if(isCasting){
            if(showDebug)
            Debug.Log(actorName + " is casting!");
        }
        else{
            if(!castReady){
                if(target != null){ 

                    if(inAbility.getCastTime() > 0.0f){ // Casted Ability

                        //Debug.Log("Trying to create a castBar for " + inAbility.getName());

                        //Preparing variables for cast
                        queuedAbility = inAbility;
                        castReady = false; // for saftey. Should've been set by castBar or initialized that way already
                        isCasting = true;

                        
                        if(gameObject.tag == "Player"){ // For player
                            //Creating cast bar and setting it's parent to canvas to display it properly

                            GameObject newAbilityCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);
                            // v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
                            newAbilityCast.GetComponent<CastBar>().Init(inAbility.getName(), this,
                                                                            target, inAbility.getCastTime());
                        }
                        else{// For NPCs
                            if(showDebug)
                            Debug.Log(actorName + " starting cast: " + inAbility.getName());
                            gameObject.AddComponent<CastBarNPC>().Init(inAbility.getName(), this,
                                                                            target, inAbility.getCastTime());
                        }

                    }
                    else{
                        if(showDebug)
                        Debug.Log("GM| Instant cast: " + inAbility.getName());
                        queuedAbility = inAbility;
                        castReady = true;
                    }
                }
                else{
                    if(showDebug)
                    Debug.Log(actorName + " doesn't have a target!");
                }
            }
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
    public void checkAndQueue(Ability _ability){
        if(checkOnCooldown(_ability) == false){
            queueAbility(_ability);
        }
    }
    //-------------------------------------------------------------------other---------------------------------------------------------------------------------------------------------
    float RoundToNearestHalf(float value)
    {
        return MathF.Round(value * 2) / 2;
    }
}

