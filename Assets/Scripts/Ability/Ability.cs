using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    private string abilityName;
    private AbilityEffect AbilityEffect;

    private float castTime;

    public string getName(){
        return abilityName;
    }
    public AbilityEffect getEffect(){
        return AbilityEffect;
    }
    public float getCastTime(){
        return castTime;
    }

    public Ability(string inName, AbilityEffect inAbilityEffect, float inCastTime){
        abilityName = inName;
        AbilityEffect = inAbilityEffect;
        castTime = inCastTime;
    }
}
