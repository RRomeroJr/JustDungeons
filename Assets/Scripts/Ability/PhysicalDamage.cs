using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="PhysicalDamage", menuName = "HBCsystem/PhysicalDamage")]
public class PhysicalDamage : AbilityEff
{
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null){
        _target.damageValue((int)power);
    }
    public PhysicalDamage(string _effectName, int _id = -1, float _power = 0){
        effectName = _effectName;
        id = _id;
        power = _power;
    }
    public PhysicalDamage(){}
    public override AbilityEff clone()
    {
        PhysicalDamage temp_ref = ScriptableObject.CreateInstance(typeof (PhysicalDamage)) as PhysicalDamage;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        

        return temp_ref;
    }
}
