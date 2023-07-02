using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="SchoolDamage", menuName = "HBCsystem/SchoolDamage")]
public class SchoolDamage : AbilityEff
{   
    public int school = -1;
    public ActorStats scaleStat = ActorStats.MainStat;
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        if(caster != null){
            target.damageValue((int)power + (int)(caster.GetStat(scaleStat) * powerScale), fromActor: caster);
        }
        else{
           target.damageValue((int)power);

        }
    //    Debug.Log(power.ToString() + " + " + caster.mainStat.ToString() + " * " + powerScale.ToString());
    }
    public SchoolDamage(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public SchoolDamage(){}
    public override AbilityEff clone()
    {
        SchoolDamage temp_ref = ScriptableObject.CreateInstance(typeof (SchoolDamage)) as SchoolDamage;
        copyBase(temp_ref);
        // temp_ref.effectName = effectName;
        // temp_ref.id = id;
        // temp_ref.power = power;
        // temp_ref.powerScale = powerScale;
        temp_ref.school = school;
        temp_ref.scaleStat = scaleStat;
        // temp_ref.targetIsSecondary = targetIsSecondary;

        return temp_ref;
    }
}
