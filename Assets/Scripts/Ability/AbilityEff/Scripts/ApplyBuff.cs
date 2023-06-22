using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using OldBuff;

[System.Serializable]
[CreateAssetMenu(fileName="ApplyBuff", menuName = "HBCsystem/ApplyBuff")]
public class ApplyBuff : AbilityEff
{

    // [SerializeField]protected Buff buffID; //Make this actually an id in the future
    [SerializeField]protected BuffScriptableObject buffSO;
    
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        // Debug.Log("Apply Buff");
        // Debug.Log("Apply " + buffID.name + ": to" +(_target != null ? target.ActorName : " No target ") + ", " + (_caster != null ?  _caster.ActorName : " No caster"));

        _target.buffHandler.AddBuff(buffSO);

        // Buff tempBuff_ref = buffID.clone();
        // tempBuff_ref.target = _secondaryTarget;
        // tempBuff_ref.caster = _caster;
        // _target.applyBuff(tempBuff_ref);

        //below wasn't used
        //tempBuff_ref.eInstructs = eInstructs;
        
    }
    
    public ApplyBuff(){}
    
    public override AbilityEff clone()
    {
        ApplyBuff temp_ref = ScriptableObject.CreateInstance(typeof (ApplyBuff)) as ApplyBuff;
        copyBase(temp_ref);
        temp_ref.buffSO = buffSO;
        //temp_ref.eInstructs = new List<EffectInstruction>();
        
        return temp_ref;
    }
}
