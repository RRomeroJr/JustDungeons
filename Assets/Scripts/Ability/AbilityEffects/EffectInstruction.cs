using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EffectInstruction
{
    [SerializeField]public AbilityEff effect;
    [SerializeField]public int targetArg = 0;
    public void startEffect(Actor inTarget = null, NullibleVector3 inTargetWP = null, Actor inCaster = null){
        //Debug.Log(inTargetWP == null ? "eInstruct: No targetWP" : ("eInstruct: wp = " + inTargetWP.Value.ToString()));
        switch(targetArg){
            case(0):
                effect.startEffect(inTarget, inTargetWP, inCaster);
                break;
            case(1):
                effect.startEffect(inCaster, inTargetWP, inCaster);
                break;
            default:
                Debug.Log("EI: Could not start effect: " + effect.effectName);
                break;
        }
    }
    Actor getTarget(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null){
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
    public EffectInstruction clone(){
        EffectInstruction toReturn = new EffectInstruction();
        if(effect != null){
            toReturn.effect = effect.clone();
        }
        toReturn.targetArg = targetArg;
        return toReturn;
    }
}
