using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
[CreateAssetMenu(fileName="Ability")]
public class Ability_V2 : ScriptableObject{
    [SerializeField] protected string abilityName;
    [SerializeField] protected List<EffectInstruction> eInstructs;
    [SerializeField] protected List<AbilityEff> effects;
    [SerializeField] protected List<int> targetArgs; //if theres an arg here I will use it if not assumed 0


    //[SerializeField] public bool needsTarget = true;
    [SerializeField] protected int deliveryType;
    [SerializeField] protected float castTime;
    [SerializeField] protected float cooldown;
    public bool needsActor;
    public bool needsWP;
    protected bool canEdit = true;
    

    public string getName(){
        return abilityName;
    }
    public AbilityEff getEffect(){
        return effects[0];
    }
    public List<AbilityEff> getEffects(){
        return effects;
    }
    public float getCastTime(){
        return castTime;
    }

    public float getCooldown(){
        return cooldown;
    }

    public void setName(string _abilityName){
        if(canEdit){
            abilityName = _abilityName;        
        }else{
            Debug.Log("Can't edit Ability" + abilityName);
        }
    }
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
    }
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
    
    public int getDeliveryType(){
    
        return deliveryType;
    
    }
    public void setDeliveryType(int _deliveryType){
        if(canEdit){
            deliveryType = _deliveryType;        
        }else{
            Debug.Log("Can't edit Ability" + abilityName);
        }
    }
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
    public Ability_V2(string _abilityName, AbilityEff _abilityEff, int _deliveryType = 0, float _castTime = 0.0f,
                    float _cooldown = 0.0f){
       
        abilityName = _abilityName;
        effects = new List<AbilityEff>() {_abilityEff};
        //if effects.Count != deliveryTypes.Count ERROR
        deliveryType = _deliveryType;
        castTime = _castTime;
        cooldown = _cooldown;
        
    }
    public Ability_V2(string _abilityName, List<AbilityEff> _effects, int _deliveryType = 0, float _castTime = 0.0f,
                     float _cooldown = 0.0f){
        
        abilityName = _abilityName;
        effects = _effects;
        deliveryType = _deliveryType;
        castTime = _castTime;
        cooldown = _cooldown;
        
    }
    public void cast(){
        
    }
    public Ability_V2 clone(){
        // Returns a Copy of the ability with a COPY of the the name, a REF to the effect, and copy of castTime since it is just a value type

        //return new Ability(String.Copy(abilityName), abilityEffectPreset, castTime, cooldown, needsTarget);
        return new Ability_V2(String.Copy(abilityName), new List<AbilityEff>(effects), deliveryType, castTime, cooldown);

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
