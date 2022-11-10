using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="Taunt", menuName = "HBCsystem/Taunt")]
public class Taunt : AbilityEff
{   
    public int school = -1;
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        this.target.target = _caster;
        // this.target.GetCompontent<Controller>();
    }
    public Taunt(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public Taunt(){}
    public override AbilityEff clone()
    {
        Taunt temp_ref = ScriptableObject.CreateInstance(typeof (Taunt)) as Taunt;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.powerScale = powerScale;
        temp_ref.school = school;
        temp_ref.targetIsSecondary = targetIsSecondary;

        return temp_ref;
    }
}
