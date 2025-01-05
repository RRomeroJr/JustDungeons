using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
using OldBuff;

[Serializable]
public abstract class AbilityEff_V2: ScriptableObject
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

    //public List<Actor> specificTargets;\
    /// <summary>
    ///	How does this effect "get to it's target"?
    /// </summary>
    public virtual void OnSendEffect(){
        // Default behavior is just give this effect to the target. If you want
        // a missle/ aoe then you would overwrite this method. 
        target.ReceiveEffect(this, targetWP, caster);
    }
    /// <summary>
    ///	What does this effect ultimately do?
    /// </summary>
    public abstract void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null);
    public virtual void buffEndEffect(){

    }
    public virtual void buffStartEffect(){

    }
    public virtual void OnBuffTick(){

    }
    public virtual void dispellEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){

    }
    public virtual void clientEffect(){
        
    }
    public virtual void OnWrite(NetworkWriter _writer){
        // Override this if you have an effect that needs to expose values to a client
    }
    public virtual void OnRead(NetworkReader _reader){
        // Override this if you have an effect that needs to expose values to a client
    }
    public virtual void OnUpdateData(BuffUpdateData _bud)
    {
    }

    public abstract AbilityEff_V2 clone();
    public virtual void copyBase(AbilityEff_V2 _clone){
        _clone.effectName = effectName;
        _clone.id = id;
        _clone.power = power;
        _clone.powerScale = powerScale;
        _clone.targetIsSecondary = targetIsSecondary;
        _clone.isHostile = isHostile;
        Debug.Log("New stuff in copyBase");
        _clone.target = target;
        _clone.caster = caster;
        _clone.targetWP = targetWP;
        _clone.targetWP2 = targetWP2;
    }

}
