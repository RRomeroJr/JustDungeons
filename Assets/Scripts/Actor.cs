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
    public List<SpellEffect> spellEffects;
    public List<ActiveSpellEffect> activeSpellEffects; 

    void Start(){
        spellEffects = new List<SpellEffect>();
        activeSpellEffects = new List<ActiveSpellEffect>(); 
    }
    void Update(){
        handleSpellEffects();
    }

    void handleSpellEffects(){

        if(activeSpellEffects.Count > 0){

            for(int i = 0; i < activeSpellEffects.Count; i++){

                switch(activeSpellEffects[i].getEffectType()){
                    case 0: // damage
                        handleDamage(activeSpellEffects[i]);

                        break;

                    case 1: // heal
                        handleHeal(activeSpellEffects[i]);
                        break;
                    case 2: // DoT
                        if(activeSpellEffects[i].lastTick >= activeSpellEffects[i].getTickRate()){
                            // if rdy to tick

                            handleDoT(activeSpellEffects[i]);
                            Debug.Log("Actor: ticking" + activeSpellEffects[i].getEffectName() + " on " + actorName);
                        }
                        //Iterate duration
                        activeSpellEffects[i].duration -= Time.deltaTime;
                        //Iterate lastTick
                        activeSpellEffects[i].lastTick += Time.deltaTime;
                        break;
                    case 3: // HoT
                        if(activeSpellEffects[i].lastTick >= activeSpellEffects[i].getTickRate()){
                            // if rdy to tick

                            handleHoT(activeSpellEffects[i]);
                            Debug.Log("Actor: ticking" + activeSpellEffects[i].getEffectName() + " on " + actorName);
                        }
                        //Iterate duration
                        activeSpellEffects[i].duration -= Time.deltaTime;
                        //Iterate lastTick
                        activeSpellEffects[i].lastTick += Time.deltaTime;
                        break;
                    default:
                        Debug.Log("Unknown spell type on " + actorName + "! Don't know what to do! Trying to remove..");
                        activeSpellEffects[i].duration = 0.0f;
                        break;
                
                }
            checkASEToRemoveAtPos(activeSpellEffects[i], i);
            }
        //Debug.Log(actorName + " cleared all spell effects!");
        }
    }
    void damageValue(int amount){
        // Right now this only damages health, but, maybe in the future,
        // This could take an extra param to indicate a different value to "damage"
        // For ex. a spell that reduces maxHealth or destroys mana

        health -= amount;

    }

    void restoreValue(int amount){
        // This would be the opposite of damageValue(). Look at that for more details
        
        health += amount;

    }

    void checkASEToRemoveAtPos(ActiveSpellEffect inASE, int listPos){
        // Remove ActiveSpellEffect is it's duration is <= 0.0f

        if(inASE.getDuration() <= 0.0f){
            Debug.Log(actorName + ": Removing.. "+ inASE.getEffectName());
            activeSpellEffects.RemoveAt(listPos);
        }
    }

    public void applySpellEffect(SpellEffect inSpellEffect, Actor inCaster){

        activeSpellEffects.Add(new ActiveSpellEffect(inSpellEffect, inCaster));
        //Debug.Log("Actor: Applying.." + inSpellEffect.getEffectName() + " to " + actorName);  

    }
    void handleDamage(ActiveSpellEffect inASE){// Type 0

        /* 
            In here you could add interesting interactions
            Maybe something like 
            if(ReverseDamageAndHealing)
                then call restoreValue() instead
        */
        
        damageValue( (int) inASE.getPower() );// likly will change once we have stats

        // For saftey to make sure that the effect is removed from list
        // right aft the effect finishes
        inASE.duration = 0.0f; 
            
    }
    void handleDoT(ActiveSpellEffect inASE){// Type 2

        // Do any extra stuff

        damageValue( (int) ( ( inASE.getTickRate() / inASE.getDuration() ) * inASE.getPower() ) );// likly will change once we have stats
        inASE.lastTick = 0.0f;
            
    }

    void handleHeal(ActiveSpellEffect inASE){// Type 0

        /* 
            In here you could add interesting interactions
            Maybe something like 
            if(ReverseDamageAndHealing)
                then call damageValue() instead
        */
        
        restoreValue( (int) inASE.getPower() ); // likly will change once we have stats

        // For saftey to make sure that the effect is removed from list
        // right aft the effect finishes
        inASE.duration = 0.0f; 
            
    }

    void handleHoT(ActiveSpellEffect inASE){// Type 2

        // Do any extra stuff

        restoreValue( (int) ( ( inASE.getTickRate() / inASE.getDuration() ) * inASE.getPower() ) );// likly will change once we have stats
        inASE.lastTick = 0.0f;
            
    }
    

    float RoundToNearestHalf(float value)
    {
        return MathF.Round(value * 2) / 2;
    }
}
