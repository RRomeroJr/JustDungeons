using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="Ability")]
public class StackingDmgBuff : AbilityEff
{   
    public int school;

    //                          in this case target is the actor the buff is on
    public override void effectStart(Actor _target = null, Vector3? _targetWP = null, Actor _caster = null){
        if(parentBuff != null){
            //Debug.Log("Increased dmg: " + ((int)((power) * (float)parentBuff.stacks)).ToString());
            _target.damageValue((int)((power) * (float)parentBuff.stacks));
        }else{
            //Debug.Log("Ticking normal dmg");
            _target.damageValue((int)(power));
        }
    }
    public StackingDmgBuff(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public StackingDmgBuff(){}
    public override AbilityEff clone()
    {
        StackingDmgBuff temp_ref = ScriptableObject.CreateInstance(typeof (StackingDmgBuff)) as StackingDmgBuff;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.school = school;

        return temp_ref;
        //return new StackingDmgBuff(effectName, id, power, school);
    }
}
