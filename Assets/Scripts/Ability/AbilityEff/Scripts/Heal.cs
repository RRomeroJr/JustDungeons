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
        if(target == null){
            Debug.LogError(name + "| no target. returning");
        }
       if(caster !=null){
            target.restoreValue((int)power + (int)(caster.mainStat * powerScale), fromActor: caster);
        }
        else{
            target.restoreValue((int)power);
        }
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
        copyBase(temp_ref);
        temp_ref.school = school;

        return temp_ref;
    }
}
