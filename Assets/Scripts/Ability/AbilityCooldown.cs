using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityCooldown 
{
    private string abilityName;
    public float remainingTime;  

    public AbilityCooldown(){

    }
    public AbilityCooldown(Ability _ability){
        abilityName = _ability.getName();
        remainingTime = _ability.getCooldown();
    }
    public AbilityCooldown(Ability_V2 _ability){
        abilityName = _ability.getName();
        remainingTime = _ability.getCooldown();
    }
    public string getName(){
        return abilityName;
    }
}
