using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class Ability
{
    [SerializeField] protected string abilityName;
    [SerializeField] protected List<AbilityEffectPreset> abilityEffectPresets = new List<AbilityEffectPreset>();
    
    
    //[SerializeField] public bool needsTarget = true;
    [SerializeField] protected int deliveryType;
    [SerializeField] protected float speed;
    [SerializeField] protected float duration;
    [SerializeField] protected float aoeTickRate;
    [SerializeField] protected int aoeCap;
    [SerializeField] protected bool ignoreDuration;

    [SerializeField] protected float castTime;
    [SerializeField] protected float cooldown;
    protected bool canEdit = true;

    public string getName(){
        return abilityName;
    }
    public AbilityEffectPreset getEffectPreset(){
        return abilityEffectPresets[0];
    }
    public List<AbilityEffectPreset> getEffectPresets(){
        return abilityEffectPresets;
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
    public void setEffectPreset(AbilityEffectPreset _abilityEffectPreset){
        if(canEdit){
            List<AbilityEffectPreset> temp = new List<AbilityEffectPreset>();
            temp.Add(_abilityEffectPreset);
            abilityEffectPresets = temp;       
        }else{
            Debug.Log("Can't edit Ability" + abilityName);
        }
    }
    public void setEffectPresets(List<AbilityEffectPreset> _abilityEffectPresets){
        if(canEdit){
            abilityEffectPresets = _abilityEffectPresets;        
        }else{
            Debug.Log("Can't edit Ability" + abilityName);
        }
    }
    public void addEffect(AbilityEffectPreset _abilityEffectPreset){
        if(canEdit){
            abilityEffectPresets.Add(_abilityEffectPreset);      
        }else{
            Debug.Log("Can't edit Ability" + abilityName);
        }
    }
    public List<AbilityEffect> createEffects(){
        List<AbilityEffect> tempList_ref = abilityEffectPresets.createEffects();
        return tempList_ref;
    }
    public List<AbilityEffect> createEffects(Actor _caster){
        List<AbilityEffect> tempList_ref = abilityEffectPresets.createEffects(_caster);
        return tempList_ref;
    }
    public List<AbilityEffect> createEffects(Vector3 _targetWP){
        List<AbilityEffect> tempList_ref = abilityEffectPresets.createEffects();
        for(int i = 0; i < tempList_ref.Count; i++){
            tempList_ref[i].setTargetWP(_targetWP);
        }
        return tempList_ref;
    }

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
    public float getSpeed(){
        return speed;
    }
    public void setSpeed(float _speed){
        speed = _speed;
    }
    public float getDuration(){
        return duration;
    }
    public void setDuration(float _duration){
        duration = _duration;
    }
    public float getAoeTickRate(){
        return aoeTickRate;
    }
    public void setAoeTickRate(float _aoeTickRate){
        aoeTickRate = _aoeTickRate;
    }
    public int getAoeCap(){
        return aoeCap;
    }
    public void setAoeCap(int _aoeCap){
        aoeCap = _aoeCap;
    }
    public bool getIgnoreDuration(){
        return ignoreDuration;
    }
    public void setIgnoreDuration(bool _ignoreDuration){
        ignoreDuration = _ignoreDuration;
    }
    public Ability(string _abilityName, AbilityEffectPreset _abilityEffectPreset, int _deliveryType = 0, float _castTime = 0.0f,
                    float _cooldown = 0.0f, float _speed = 0.1f, float _duration = 8.0f, float _aoeTickRate = 1.5f, int _aoeCap = -1,
                     bool _ignoreDuration = false){
       
        abilityName = _abilityName;
        abilityEffectPresets = new List<AbilityEffectPreset>() {_abilityEffectPreset};
        //if abilityEffectPresets.Count != deliveryTypes.Count ERROR
        deliveryType = _deliveryType;
        castTime = _castTime;
        cooldown = _cooldown;
        speed = _speed;
        duration =_duration;
        aoeTickRate = _aoeTickRate;
        aoeCap = _aoeCap;
        ignoreDuration = _ignoreDuration;
    }
    public Ability(string _abilityName, List<AbilityEffectPreset> _abilityEffectPresets, int _deliveryType = 0, float _castTime = 0.0f,
                     float _cooldown = 0.0f, float _speed = 0.1f, float _duration = 8.0f, float _aoeTickRate = 1.5f, int _aoeCap = -1,
                     bool _ignoreDuration = false){
        
        abilityName = _abilityName;
        abilityEffectPresets = _abilityEffectPresets;
        deliveryType = _deliveryType;
        castTime = _castTime;
        cooldown = _cooldown;
        speed = _speed;
        duration =_duration;
        aoeTickRate = _aoeTickRate;
        aoeCap = _aoeCap;
        ignoreDuration = _ignoreDuration;
    }
    public Ability clone(){
        // Returns a Copy of the ability with a COPY of the the name, a REF to the effect, and copy of castTime since it is just a value type

        //return new Ability(String.Copy(abilityName), abilityEffectPreset, castTime, cooldown, needsTarget);
        return new Ability(String.Copy(abilityName), new List<AbilityEffectPreset>(abilityEffectPresets), deliveryType, castTime, cooldown);

    }
    public bool NeedsTargetActor(){
        switch(deliveryType){
            case -2:
                return false;
                break;
            case -1:
                return true;
                break;
            case 0:
                return true;
                break;
            case 1:
                return false;
                break;
            case 2:
                return false;
                break;
            default:
                Debug.Log("Unknown Delivery Type");
                return false;
                break;
        }
    }
    public bool NeedsTargetWP(){
        switch(deliveryType){
            case -2:
                return true;
                break;
            case -1:
                return false;
                break;
            case 0:
                return false;
                break;
            case 1:
                return true;
                break;
            case 2:
                return true;
                break;
            default:
                Debug.Log("Unknown Delivery Type");
                return false;
                break;
        }
    }

}
