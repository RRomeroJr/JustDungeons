using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
     Container many for any RPG related elements
*/

public class Actor : MonoBehaviour
{
    public string actorName;
    public float health; // RR: This was changed to FloatReference had to change it back bc it caused a bunch of errors
    public float maxHealth;
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
                        if(activeSpellEffects[i].getDuration() <= 0.0f){
                            Debug.Log("Damaging " + actorName + ": " + activeSpellEffects[i].getPower().ToString());
                            health -= activeSpellEffects[i].getPower();
                            activeSpellEffects[i].elaspedTime = activeSpellEffects[i].getDuration();// kinda unnecesary but for saftey to make sure it gets deleted
                        }else{
                            //Debug.Log("Applying DoT to " + actorName + ": " + activeSpellEffects[i].getPower().ToString());
                            health -= (Time.deltaTime/ activeSpellEffects[i].getDuration()) * activeSpellEffects[i].getPower();
                            activeSpellEffects[i].elaspedTime += Time.deltaTime;
                            /*if(activeSpellEffects[i].elaspedTime % activeSpellEffects[i].getTickRate() == 0.0f){
                                //Debug.Log("Ticking Dmg " + actorName + ": " + activeSpellEffects[i].getPower().ToString());
                                health -= (activeSpellEffects[i].getTickRate()/ activeSpellEffects[i].getDuration()) * activeSpellEffects[i].getPower();
                                activeSpellEffects[i].elaspedTime += Time.fixedDeltaTime;
                            }*/
                        }
                        break;

                    case 1: // heal
                        if(activeSpellEffects[i].getDuration() <= 0.0f){
                            Debug.Log("Damaging " + actorName + ": " + activeSpellEffects[i].getPower().ToString());
                            health += activeSpellEffects[i].getPower();
                            activeSpellEffects[i].elaspedTime = activeSpellEffects[i].getDuration(); // kinda unnecesary but for saftey to make sure it gets deleted
                        }else{
                            health += (Time.deltaTime/ activeSpellEffects[i].getDuration()) * activeSpellEffects[i].getPower();
                            activeSpellEffects[i].elaspedTime += Time.deltaTime;
                        }
                        break;
                    case 2: // DoT
                        /*
                            For now will do damage on every frame. But in 
                            future damage will come out every tickRate secs
                        */
                        /*
                        health -= ((Time.deltaTime / spellEffects[i].duration) * spellEffects[i].power);

                        if(spellEffects[i].isActive == false){
                            Debug.Log(actorName + " Applying:" + spellEffects[i].effectName);
                            spellEffects[i].isActive = true; 
                        }
                        else{

                            spellEffects[i].duration -= Time.deltaTime; 
                        }
                        break;
                        */
                    default:
                        Debug.Log("Unknown spell type on " + actorName + "! Don't know what to do! Trying to remove..");
                        activeSpellEffects[i].elaspedTime = activeSpellEffects[i].getDuration();
                        break;
                
                }
            checkSEToRemoveAtPos(activeSpellEffects[i], i);
            }
        //Debug.Log(actorName + " cleared all spell effects!");
        }
    }
    void takeDamage(float amount){
        health -= amount;
    }

    void checkSEToRemoveAtPos(ActiveSpellEffect inASE, int listPos){
        if(inASE.elaspedTime >= inASE.getDuration()){
            Debug.Log(actorName + ": Removing.. "+ inASE.getEffectName());
            activeSpellEffects.RemoveAt(listPos);
        }
    }

    public void applySpellEffect(SpellEffect inSpellEffect, Actor inCaster){
        activeSpellEffects.Add(new ActiveSpellEffect(inSpellEffect, inCaster));
    }
}
