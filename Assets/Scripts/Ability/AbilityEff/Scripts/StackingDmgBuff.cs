using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="StackingDmgBuff", menuName = "HBCsystem/StackingDmgBuff")]
public class StackingDmgBuff : AbilityEff
{   
    public int school;

    //                          in this case target is the actor the buff is on
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        try
        {        if (!_target.TryGetComponent(out Actor targetActor))
        {
            return;
        }
        
        if (parentBuff != null){
            //Debug.Log("Increased dmg: " + ((int)((power) * (float)parentBuff.stacks)).ToString());
            targetActor.damageValue((int)((power) * (float)parentBuff.stacks));
        }else{
            //Debug.Log("Ticking normal dmg");
            targetActor.damageValue((int)(power));
        }
        }
        catch{}
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
        copyBase(temp_ref);
        temp_ref.school = school;
        

        return temp_ref;
        //return new StackingDmgBuff(effectName, id, power, school);
    }
}
