using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    
}
