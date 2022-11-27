using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="DeliveryKnockback", menuName = "HBCsystem/DeliveryKnockback")]
public class DeliveryKnockback : AbilityEff
{   
    public int school = -1;
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
       
       Vector2 force = new Vector2();
       force = _target.transform.position - parentDelivery.transform.position;
       force.Normalize();
       force *= power;

        _target.Knockback(force);
       
    }
    public DeliveryKnockback(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public DeliveryKnockback(){}
    public override AbilityEff clone()
    {
        DeliveryKnockback temp_ref = ScriptableObject.CreateInstance(typeof (DeliveryKnockback)) as DeliveryKnockback;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.powerScale = powerScale;
        temp_ref.school = school;
        temp_ref.targetIsSecondary = targetIsSecondary;

        return temp_ref;
    }
}
