using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EffectInstruction
{
    [SerializeField]public AbilityEff effect;
    [SerializeField]public int targetArg = 0;
    [SerializeField]public float powerMod = 1.0f;
    public UnityEvent<EffectInstruction> onSendHooks;
    
    public void startEffect(Actor inTarget = null, NullibleVector3 inTargetWP = null, Actor inCaster = null, Actor inSecondaryTarget = null){
        //Debug.Log(inTargetWP == null ? "eInstruct: No targetWP" : ("eInstruct: wp = " + inTargetWP.Value.ToString()));
        effect.startEffect(inTarget, inTargetWP, inCaster, inSecondaryTarget);
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
    public void sendToActor(Actor inTarget = null, NullibleVector3 inTargetWP = null, Actor inCaster = null, Actor inSecondaryTarget = null){
        //Debug.Log(inTargetWP == null ? "eInstruct: No targetWP" : ("eInstruct: wp = " + inTargetWP.Value.ToString()));
        
        switch(targetArg){
            case(0):
                break;
            case(1):
                inTarget = inCaster;
                break;
            default:
                Debug.Log("EI: Unknown targetArg " + effect.effectName);
                break;
        }
        if(effect.targetIsSecondary){
            //Debug.Log("Target is 2ndary!");
            inSecondaryTarget = inTarget;
            inTarget = inCaster;
        }
        // switch(targetArg){
        //     case(0):
        //         inTarget.recieveEffect(this, inTargetWP, inCaster, inSecondaryTarget);
        //         break;
        //     case(1):
        //         inCaster.recieveEffect(this, inTargetWP, inCaster, inSecondaryTarget);
        //         break;
        //     default:
        //         Debug.Log("EI: Could not start effect: " + effect.effectName);
        //         break;
        // }
        //Debug.Log("sendToActor caster" + (inCaster != null ? inCaster.getActorName() : "caster is null"));
        effect.target = inTarget;
        effect.targetWP = inTargetWP;
        effect.caster = inCaster;
        if(onSendHooks != null){
            onSendHooks.Invoke(this);
        }
        inTarget.recieveEffect(this, inTargetWP, inCaster, inSecondaryTarget);
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
        toReturn.onSendHooks = onSendHooks;
        //toReturn.powerMod = powerMod;
        return toReturn;
    }
    public EffectInstruction cloneNoEffectClone(){
        EffectInstruction toReturn = new EffectInstruction();
        if(effect != null){
            toReturn.effect = effect;
        }
        //toReturn.effect.power *= powerMod;
        toReturn.targetArg = targetArg;
        toReturn.onSendHooks = onSendHooks;
        //toReturn.powerMod = powerMod;
        return toReturn;
    }
}
