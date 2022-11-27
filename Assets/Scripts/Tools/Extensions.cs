using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


//May need a namespace here in the future but for now it seems to work fine

public static class Extensions
{
    public static List<AbilityEffect> cloneEffects(this List<AbilityEffect> AE_list){
        List<AbilityEffect> listToReturn = new List<AbilityEffect>();
        if(AE_list.Count > 0){
            for(int i=0; i < AE_list.Count; i++){
                listToReturn.Add(AE_list[i].clone());
            }
            return listToReturn;
        }
        else{
            return listToReturn;
        }
    }
    public static List<AbilityEff> cloneEffects(this List<AbilityEff> AE_list){
        List<AbilityEff> listToReturn = new List<AbilityEff>();
        if(AE_list.Count > 0){
            for(int i=0; i < AE_list.Count; i++){
                listToReturn.Add(AE_list[i].clone());
            }
            return listToReturn;
        }
        else{
            return listToReturn;
        }
    }
    public static List<AbilityEffect> createEffects(this List<AbilityEffectPreset> AEP_list, bool can_edit = true){
        List<AbilityEffect> listToReturn = new List<AbilityEffect>();
        if(AEP_list.Count > 0){
            for(int i=0; i < AEP_list.Count; i++){
                listToReturn.Add(new AbilityEffect(AEP_list[i], can_edit));
            }
            return listToReturn;
        }
        else{
            return listToReturn;
        }
    }
    public static List<AbilityEffect> createEffects(this List<AbilityEffectPreset> AEP_list, Actor _caster, bool can_edit = true){
        List<AbilityEffect> listToReturn = new List<AbilityEffect>();
        if(AEP_list.Count > 0){
            for(int i=0; i < AEP_list.Count; i++){
                AbilityEffect temp = new AbilityEffect(AEP_list[i], can_edit);
                temp.setCaster(_caster);
                listToReturn.Add(temp);
            }
            return listToReturn;
        }
        else{
            return listToReturn;
        }
    }
    public static void addEffect(this List<EffectInstruction> _eInstruct_list, AbilityEff _effect){
        EffectInstruction tempRef = new EffectInstruction(_effect, 0);
        _eInstruct_list.Add(tempRef);
    }
    public static void addEffect(this List<EffectInstruction> _eInstruct_list, AbilityEff _effect, int _targetArg){
        EffectInstruction tempRef = new EffectInstruction(_effect, _targetArg);
        _eInstruct_list.Add(tempRef);
    }
    public static void addEffects(this List<EffectInstruction> _eInstruct_list, List<AbilityEff> _effects){
        foreach (AbilityEff eff in _effects){
            _eInstruct_list.addEffect(eff);
        }
    }
    public static void addEffects(this List<EffectInstruction> _eInstruct_list, List<AbilityEff> _effects, List<int> _targetArgs){
        if(_effects.Count == _targetArgs.Count){
            Debug.LogWarning("addEffects called with less tartgetArgs than effects. Remainder will be set to 0");
        }
        for (int i = 0; i < _effects.Count; i++)
        {
            if(i <_targetArgs.Count){
                _eInstruct_list.addEffect(_effects[i], _targetArgs[i]);
            }
            else{
                _eInstruct_list.addEffect(_effects[i], 0);
            }
        }
    }
    public static List<EffectInstruction> cloneInstructs(this List<EffectInstruction> eI_list){
        List<EffectInstruction> listToReturn = new List<EffectInstruction>();
        if(eI_list.Count > 0){
            for(int i=0; i < eI_list.Count; i++){
                listToReturn.Add(eI_list[i].clone());
            }
            return listToReturn;
        }
        else{
            return listToReturn;
        }
    }
    public static List<EffectInstruction> cloneInstructsNoEffectClone(this List<EffectInstruction> eI_list){
        List<EffectInstruction> listToReturn = new List<EffectInstruction>();
        if(eI_list.Count > 0){
            for(int i=0; i < eI_list.Count; i++){
                listToReturn.Add(eI_list[i].cloneNoEffectClone());
            }
            return listToReturn;
        }
        else{
            return listToReturn;
        }
    }
    public static NetworkConnection GetNetworkConnection(this MonoBehaviour _mo){
        return _mo.GetComponent<NetworkIdentity>().connectionToClient;
    }
    
    
}
