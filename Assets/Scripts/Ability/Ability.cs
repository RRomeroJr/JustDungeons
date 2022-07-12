using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class Ability
{
    [SerializeField] protected string abilityName;
    [SerializeField] protected List<AbilityEffectPreset> abilityEffectPresets = new List<AbilityEffectPreset>();
    
    [SerializeField] protected float castTime;
    [SerializeField] protected float cooldown;
    //[SerializeField] public bool needsTarget = true;
    [SerializeField] protected int deliveryType;
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
    public Ability(string _abilityName, AbilityEffectPreset _abilityEffectPreset, int _deliveryType = 0, float _castTime = 0.0f, float _cooldown = 0.0f){
        abilityName = _abilityName;
        abilityEffectPresets.Add(_abilityEffectPreset);
        deliveryType = _deliveryType;
        castTime = _castTime;
        cooldown = 0.0f;
    }
    public Ability(string _abilityName, List<AbilityEffectPreset> _abilityEffectPresets, int _deliveryType = 0, float _castTime = 0.0f, float _cooldown = 0.0f){
        abilityName = _abilityName;
        abilityEffectPresets = _abilityEffectPresets;
        deliveryType = _deliveryType;
        castTime = _castTime;
        cooldown = _cooldown;
    }
    public Ability clone(){
        // Returns a Copy of the ability with a COPY of the the name, a REF to the effect, and copy of castTime since it is just a value type

        //return new Ability(String.Copy(abilityName), abilityEffectPreset, castTime, cooldown, needsTarget);
        return new Ability(String.Copy(abilityName), new List<AbilityEffectPreset>(abilityEffectPresets), deliveryType, castTime, cooldown);

    }
    public bool NeedsTargetActor(){
        switch(deliveryType){
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
