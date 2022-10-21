using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="RestoreClassResource", menuName = "HBCsystem/RestoreClassResource")]
public class RestoreClassResource : AbilityEff
{   
    public int school = -1;
    public ClassResourceType crt;

    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
       _target.restoreResource(crt, (int)power);
    }
    public RestoreClassResource(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public RestoreClassResource(){}
    public override AbilityEff clone()
    {
        RestoreClassResource temp_ref = ScriptableObject.CreateInstance(typeof (RestoreClassResource)) as RestoreClassResource;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.school = school;
        temp_ref.targetIsSecondary = targetIsSecondary;
        temp_ref.crt = crt;
        return temp_ref;
    }
}
