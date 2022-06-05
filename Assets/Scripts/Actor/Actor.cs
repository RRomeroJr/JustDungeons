using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
     Container many for any RPG related elements
*/

public class Actor : MonoBehaviour
{
    public string actorName;
    public int health; // RR: This was changed to FloatReference had to change it back bc it caused a bunch of errors
    public int maxHealth;
    public float mana;
    public float maxMana;

    public Color unitColor;
    public List<AbilityEffect> abilityEffects;
    public List<ActiveAbilityEffect> activeAbilityEffects; 

    void Start(){
        abilityEffects = new List<AbilityEffect>();
        activeAbilityEffects = new List<ActiveAbilityEffect>(); 
    }
    void Update(){
        handleAbilityEffects();
    }

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
                        Debug.Log("Unknown Ability type on " + actorName + "! Don't know what to do! Trying to remove..");
                        activeAbilityEffects[i].duration = 0.0f;
                        break;
                
                }
            checkASEToRemoveAtPos(activeAbilityEffects[i], i);
            }
        //Debug.Log(actorName + " cleared all Ability effects!");
        }
    }
    void damageValue(int amount){
        // Right now this only damages health, but, maybe in the future,
        // This could take an extra param to indicate a different value to "damage"
        // For ex. a Ability that reduces maxHealth or destroys mana

        Debug.Log("damageValue: " + amount.ToString()+ " on " + actorName);
        health -= amount;

    }

    void restoreValue(int amount){
        // This would be the opposite of damageValue(). Look at that for more details
        
        Debug.Log("restoreValue: " + amount.ToString()+ " on " + actorName);
        health += amount;

    }

    void checkASEToRemoveAtPos(ActiveAbilityEffect inASE, int listPos){
        // Remove ActiveAbilityEffect is it's duration is <= 0.0f

        if(inASE.remainingTime <= 0.0f){
            Debug.Log(actorName + ": Removing.. "+ inASE.getEffectName());
            activeAbilityEffects.RemoveAt(listPos);
        }
    }

    public void applyAbilityEffect(AbilityEffect inAbilityEffect, Actor inCaster){

        activeAbilityEffects.Add(new ActiveAbilityEffect(inAbilityEffect, inCaster));
        //Debug.Log("Actor: Applying.." + inAbilityEffect.getEffectName() + " to " + actorName);  

    }
    void handleDamage(ActiveAbilityEffect inASE){// Type 0

        /* 
            In here you could add interesting interactions
            Maybe something like 
            if(ReverseDamageAndHealing)
                then call restoreValue() instead
        */
        
        damageValue( (int) inASE.getPower() );// likly will change once we have stats

        // For saftey to make sure that the effect is removed from list
        // right aft the effect finishes
        inASE.remainingTime = 0.0f; 
            
    }
    void handleDoT(ActiveAbilityEffect inASE){// Type 2

        // Do any extra stuff

        damageValue( (int) ( ( inASE.getTickRate() / (inASE.getDuration() + inASE.getTickRate()) ) * inASE.getPower() ) );// likly will change once we have stats
        //damageValue( (int) inASE.getPower()  );
        if(inASE.lastTick >= inASE.tickRate)
            inASE.lastTick -= inASE.tickRate;
            
    }

    void handleHeal(ActiveAbilityEffect inASE){// Type 0

        /* 
            In here you could add interesting interactions
            Maybe something like 
            if(ReverseDamageAndHealing)
                then call damageValue() instead
        */
        
        restoreValue( (int) inASE.getPower() ); // likly will change once we have stats

        // For saftey to make sure that the effect is removed from list
        // right aft the effect finishes
        inASE.remainingTime = 0.0f; 
            
    }

    void handleHoT(ActiveAbilityEffect inASE){// Type 2

        // Do any extra stuff

        restoreValue( (int) ( ( inASE.getTickRate() / inASE.getDuration() ) * inASE.getPower() ) );// likly will change once we have stats
        if(inASE.lastTick >= inASE.tickRate)
            inASE.lastTick -= inASE.tickRate;
            
    }
    

    float RoundToNearestHalf(float value)
    {
        return MathF.Round(value * 2) / 2;
    }
}
