using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
using OldBuff;

[Serializable]
public abstract class AbilityEff: ScriptableObject
{
    public string effectName;
    public int id = -1;
    public float power;
    public float powerScale = 1.0f;
    public Buff parentBuff; //Do not set this inthe editor!
    public AbilityDelivery parentDelivery;
    public bool targetIsSecondary = false;
    [HideInInspector]
    public Actor target;
    [HideInInspector]
    public Actor caster;
    [HideInInspector]
    public NullibleVector3 targetWP;
    [HideInInspector]
    public NullibleVector3 targetWP2;
    public bool isHostile = true;
    

    //public int targetArg = 0; //0 = target, 1 = self

    //public List<Actor> specificTargets;
    public abstract void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null);
    public virtual void buffEndEffect(){

    }
    public virtual void buffStartEffect(){

    }
    public virtual void dispellEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){

    }
    public virtual void clientEffect(){
        
    }

    public abstract AbilityEff clone();
    public virtual void copyBase(AbilityEff _clone){
        _clone.effectName = effectName;
        _clone.id = id;
        _clone.power = power;
        _clone.powerScale = powerScale;
        _clone.targetIsSecondary = targetIsSecondary;
        _clone.isHostile = isHostile;
    }

}
