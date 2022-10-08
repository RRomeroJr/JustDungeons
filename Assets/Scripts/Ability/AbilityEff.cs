using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

[System.Serializable]
public abstract class AbilityEff: ScriptableObject
{
    public string effectName;
    public int id = -1;
    public float power;
    public Buff parentBuff; //Do not set this inthe editor!
    public AbilityDelivery parentDelivery;
    public bool targetIsSecondary = false;

    //public int targetArg = 0; //0 = target, 1 = self

    //public List<Actor> specificTargets;
    public abstract void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null);
    public virtual void buffEndEffect(){

    }
    public virtual void buffStartEffect(){

    }
    public virtual void dispellEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){

    }

    public abstract AbilityEff clone();

}
