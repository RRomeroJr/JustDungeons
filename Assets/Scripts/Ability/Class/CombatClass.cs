using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
[CreateAssetMenu(fileName="CombatClass", menuName = "HBCsystem/CombatClass")]
public class CombatClass : ScriptableObject{
    [SerializeField] public List<Ability_V2> abilityList;
    public List<Ability_V2> GetClassAbilities(){
        List<Ability_V2> toReturn = new List<Ability_V2>(abilityList);
        return toReturn;
    }
}
