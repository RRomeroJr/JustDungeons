using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="RefundCooldown", menuName = "HBCsystem/RefundCooldown")]
public class RefundCooldown : AbilityEff
{
    public int school;
    public Ability_V2 ability;
	
    public override GameObject startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        try
        {        target.RefundCooldown(ability, power);
            return null;
        }
        catch{ return null; }
    }
    public override void clientEffect()
    {
        
    }
    public override void buffStartEffect()
    {
        
    }
    public override void buffEndEffect()
    {
       
    }
    
    public RefundCooldown(){
        isHostile = false;
    }
    public override AbilityEff clone()
    {
        RefundCooldown temp_ref = ScriptableObject.CreateInstance(typeof (RefundCooldown)) as RefundCooldown;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.powerScale = powerScale;
        temp_ref.school = school;
        temp_ref.targetIsSecondary = targetIsSecondary;
        temp_ref.ability = ability;
        
        return temp_ref;
    }
    
}
