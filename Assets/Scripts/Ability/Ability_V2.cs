using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum AbilityTags{
        Weapon,
        SpecialWeapon,
        Spell,
        SpecialSpell

    }


[System.Serializable]
[CreateAssetMenu(fileName="Ability_V2", menuName = "HBCsystem/Ability_V2")]
public class Ability_V2 : ScriptableObject{
    
    [SerializeField] protected string abilityName;
    
    [SerializeField] public List<EffectInstruction> eInstructs;
    //[SerializeField] protected List<AbilityEff> effects;
    //[SerializeField] protected List<int> targetArgs; //if theres an arg here I will use it if not assumed 0


    //[SerializeField] public bool needsTarget = true;
    
    [SerializeField] protected float castTime;
    [SerializeField] protected float cooldown;
    public bool needsActor;
    public bool needsWP;
    protected bool canEdit = true;
    public int id;
    public AbilityTags abilityTag = AbilityTags.Weapon;
    //public List<DeliveryInstruction> deliveryInstructs = new List<DeliveryInstruction>();
    

    public string getName(){
        return abilityName;
    }/*
    public AbilityEff getEffect(){
        return effects[0];
    }
    public List<AbilityEff> getEffects(){
        return effects;
    }*/
    public float getCastTime(){
        return castTime;
    }

    public float getCooldown(){
        //Debug.Log(abilityName + " cd: " + cooldown.ToString());
        return cooldown;
    }

    public void setName(string _abilityName){
       
        abilityName = _abilityName;        
        
    }/*
    public void setEffect(AbilityEff _abilityEffectPreset){
        if(canEdit){
            List<AbilityEff> temp = new List<AbilityEff>();
            temp.Add(_abilityEffectPreset);
            effects = temp;       
        }else{
            Debug.Log("Can't edit Ability" + abilityName);
        }
    }
    public void setEffects(List<AbilityEff> _effects){
        if(canEdit){
            effects = _effects;        
        }else{
            Debug.Log("Can't edit Ability" + abilityName);
        }
    }
    public void addEffect(AbilityEff _abilityEffectPreset){
        if(canEdit){
            effects.Add(_abilityEffectPreset);      
        }else{
            Debug.Log("Can't edit Ability" + abilityName);
        }
    }*/
    /*public List<AbilityEffect> createEffects(){
        List<AbilityEffect> tempList_ref = effects.createEffects();
        return tempList_ref;
    }
    public List<AbilityEffect> createEffects(Actor _caster){
        List<AbilityEffect> tempList_ref = effects.createEffects(_caster);
        return tempList_ref;
    }
    public List<AbilityEffect> createEffects(Vector3 _targetWP){
        List<AbilityEffect> tempList_ref = effects.createEffects();
        for(int i = 0; i < tempList_ref.Count; i++){
            tempList_ref[i].setTargetWP(_targetWP);
        }
        return tempList_ref;
    }*/

    public void setCastTime(float _castTime){
        if(canEdit){
            castTime = _castTime;      
        }else{
            Debug.Log("Can't edit Ability" + abilityName);
        }
    }

    public void setCooldown(float _cooldown){
        if(canEdit){
            cooldown = _cooldown;        
        }else{
            Debug.Log("Can't edit Ability" + abilityName);
        }
    }
    
    // public int getDeliveryType(){
    
    //     return deliveryType;
    
    // }
    // public void setDeliveryType(int _deliveryType){
    //     if(canEdit){
    //         deliveryType = _deliveryType;        
    //     }else{
    //         Debug.Log("Can't edit Ability" + abilityName);
    //     }
    // }
    public List<EffectInstruction> getEffectInstructions(){
        return eInstructs;
    }
    public void setEffectInstructions(List<EffectInstruction> _eInstructs){
        eInstructs = _eInstructs;
    }

    // public virtual GameObject createDeliveries(){
    //     GameObject delivery = Intantiate(abilityDeliveryPrefab);
    //     delivery.effects = effects.cloneEffects();

    // }
    public Ability_V2(string _abilityName, AbilityEff _abilityEff, float _castTime = 0.0f,
                    float _cooldown = 0.0f){
       
        abilityName = _abilityName;
        eInstructs = new List<EffectInstruction>();
        eInstructs.addEffect(_abilityEff);
        //if effects.Count != deliveryTypes.Count ERROR
        
        castTime = _castTime;
        cooldown = _cooldown;
        
    }
    public Ability_V2(string _abilityName, List<AbilityEff> _effects, float _castTime = 0.0f,
                     float _cooldown = 0.0f){
        
        abilityName = _abilityName;
        eInstructs = new List<EffectInstruction>();
        eInstructs.addEffects(_effects);
        
        castTime = _castTime;
        cooldown = _cooldown;
        
    }
    public Ability_V2(string _abilityName, List<EffectInstruction> _eInstucts, float _castTime = 0.0f,
                     float _cooldown = 0.0f){
        
        abilityName = _abilityName;
        eInstructs = _eInstucts;
        
        castTime = _castTime;
        cooldown = _cooldown;
        
    }
    
    public Ability_V2 clone(){
        // Returns a Copy of the ability with a COPY of the the name, a REF to the effect, and copy of castTime since it is just a value type

        //return new Ability(String.Copy(abilityName), abilityEffectPreset, castTime, cooldown, needsTarget);
        return new Ability_V2(String.Copy(abilityName), new List<EffectInstruction>(eInstructs), castTime, cooldown);

    }
    public bool NeedsTargetActor(){
        //In future I might be able to make the ability
        // auto matically update what target it needs based on the effects you
        // give it BUT for now this is what it's got to be
        return needsActor;
    }
    public bool NeedsTargetWP(){

        //In future I might be able to make the ability
        // auto matically update what target it needs based on the effects you
        // give it BUT for now this is what it's got to be

        return needsWP;
    }

}
