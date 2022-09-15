using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="MagicDamage", menuName = "HBCsystem/MagicDamage")]
public class MagicDamage : AbilityEff
{   
    public int school;
    public override void startEffect(Actor _target = null, Vector3? _targetWP = null, Actor _caster = null){
       _target.damageValue((int)power);
    }
    public MagicDamage(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public MagicDamage(){}
    public override AbilityEff clone()
    {
        MagicDamage temp_ref = ScriptableObject.CreateInstance(typeof (MagicDamage)) as MagicDamage;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.school = school;

        return temp_ref;
    }
}
