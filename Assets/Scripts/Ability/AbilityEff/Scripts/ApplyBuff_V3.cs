using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using OldBuff;

[System.Serializable]
[CreateAssetMenu(fileName="ApplyBuff_V3", menuName = "HBCsystem/ApplyBuff_V3")]
public class ApplyBuff_V3 : AbilityEff, IApplyBuff
{

    // [SerializeField]protected Buff buffID; //Make this actually an id in the future
    [SerializeField]protected OldBuff.BuffTemplate bt;
    public float? RemainingTimeOverride{get;set;}
    public float? TickRateOverride{get;set;}
    public int? StacksOverride{get;set;}

    
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        try
        {
            // Debug.Log("Apply Buff");
            // Debug.Log("Apply " + buffID.name + ": to" +(_target != null ? target.ActorName : " No target ") + ", " + (_caster != null ?  _caster.ActorName : " No caster"));

            // Debug.Log("ApplyBuff_V3 start effect");
            var tempBuff_ref = bt.CreateBuff();
            tempBuff_ref.actor = target;
            tempBuff_ref.caster = caster;
            tempBuff_ref.ApplyBuffOverrides(this);
            if(target.TryGetComponent(out BuffHandler_V2 _comp))
            {
                // Debug.Log("comp not null");
                _comp.AddBuff(tempBuff_ref);
            }
            else{Debug.LogError("couldn't find comp");}
           
            // _target.applyBuff(tempBuff_ref);

            //below wasn't used
            //tempBuff_ref.eInstructs = eInstructs;
        }
        catch(Exception e){
            Debug.LogError(e);
        }
    }
    
    public ApplyBuff_V3(){}
    
    public override AbilityEff clone()
    {
        ApplyBuff_V3 temp_ref = ScriptableObject.CreateInstance(typeof (ApplyBuff_V3)) as ApplyBuff_V3;
        copyBase(temp_ref);
        temp_ref.bt = bt;
        //temp_ref.eInstructs = new List<EffectInstruction>();
        
        return temp_ref;
    }
}
