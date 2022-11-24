using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="Heal", menuName = "HBCsystem/Heal")]
public class Heal : AbilityEff
{   
    public int school;
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
       target.restoreValue((int)power + (int)(caster.mainStat * powerScale), fromActor: caster);
    }
    public Heal(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public Heal(){}
    public override AbilityEff clone()
    {
        Heal temp_ref = ScriptableObject.CreateInstance(typeof (Heal)) as Heal;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.powerScale = powerScale;
        temp_ref.school = school;
        temp_ref.targetIsSecondary = targetIsSecondary;

        return temp_ref;
    }
}
