using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using OldBuff;

[System.Serializable]
[CreateAssetMenu(fileName="Dispell", menuName = "HBCsystem/Dispell")]
public class Dispell : AbilityEff
{
    
    
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        // Debug.Log("Apply Buff");
        if (_target.TryGetComponent(out IBuff buffHandler))
        {
            buffHandler.RemoveRandomBuff(b => (b.BuffSO.isDebuff == true) && b.BuffSO.dispellable);
        }
    }
    
    public Dispell(){
        isHostile = false;
    }
    
    public override AbilityEff clone()
    {
        Dispell temp_ref = ScriptableObject.CreateInstance(typeof (Dispell)) as Dispell;
        copyBase(temp_ref);

        //temp_ref.eInstructs = new List<EffectInstruction>();
        
        return temp_ref;
    }
}
