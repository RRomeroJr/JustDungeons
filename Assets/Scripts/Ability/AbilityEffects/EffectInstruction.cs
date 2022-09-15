using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EffectInstruction
{
    [SerializeField]public AbilityEff effect;
    [SerializeField]public int targetArg = 0;
    public void startEffect(Actor inTarget = null, Vector3? inTargetWP = null, Actor inCaster = null){
        switch(targetArg){
            case(0):
                effect.startEffect(_target: inTarget, _caster: inCaster);
                break;
            case(1):
                effect.startEffect(_target: inCaster, _caster: inCaster);
                break;
            default:
                Debug.Log("EI: Could not start effect: " + effect.effectName);
                break;
        }
    }
    Actor getTarget(Actor _target = null, Vector3? _targetWP = null, Actor _caster = null){
        switch(targetArg){
            case(0):
                return _target;
                break;
            case(1):
                return _caster;
                break;
            default:
                return null;
                break;
        }
    }
    public EffectInstruction(){

    }
    public EffectInstruction(AbilityEff _effect, int _targetArg){
        effect = _effect;
        targetArg = _targetArg;
    }
}
