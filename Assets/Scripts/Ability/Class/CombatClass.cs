using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


[System.Serializable]
[CreateAssetMenu(fileName = "Assets/Scripts/Ability/Class/Resources/CombatClass", menuName = "HBCsystem/CombatClass")]
public class CombatClass : ScriptableObject{
    public ClassStats classStats;
    public List<Ability_V2> abilityList;
    public List<ClassResource> classResources;
    public RuntimeAnimatorController rac;
    public List<GlowCheck> classGlowChecks;
    public List<Ability_V2> GetClassAbilities(){
        List<Ability_V2> toReturn = new List<Ability_V2>(abilityList);
        return toReturn;
        //Need a copy method here just like class resources
    }
    public List<ClassResource> GetClassResources(){
        List<ClassResource> toReturn = new List<ClassResource>();
        foreach(ClassResource _cr in classResources){
            toReturn.Add(_cr.Copy());
        }
        return toReturn;
    }
    
}
