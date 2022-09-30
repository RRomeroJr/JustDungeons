using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="Charge", menuName = "HBCsystem/Charge")]
public class Charge : AbilityEff
{   
    public int school = -1;
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
       parentBuff.actor.transform.position = Vector2.MoveTowards(parentBuff.actor.transform.position, parentBuff.target.transform.position, power);
    }
    public Charge(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
        targetIsSecondary = true;
    }
    public Charge(){}
    public override AbilityEff clone()
    {
        Charge temp_ref = ScriptableObject.CreateInstance(typeof (Charge)) as Charge;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.school = school;
        temp_ref.targetIsSecondary = targetIsSecondary;

        return temp_ref;
    }
}
