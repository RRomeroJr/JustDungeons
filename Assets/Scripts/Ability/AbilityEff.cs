using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public abstract class AbilityEff: ScriptableObject
{
    public string effectName;
    public int id = -1;
    public float power;
    public Buff parentBuff; //Do not set this inthe editor!
    
    //public int targetArg = 0; //0 = target, 1 = self

    //public List<Actor> specificTargets;
    public abstract void effectStart(Actor _target = null, Vector3? _targetWP = null, Actor _caster = null);

    public abstract AbilityEff clone();

}
