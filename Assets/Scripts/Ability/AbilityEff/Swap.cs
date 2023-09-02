using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="Swap", menuName = "HBCsystem/Swap")]
public class Swap : AbilityEff
{
    public int school;
	
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        try
        {
       
            if(target == null){
                return;
            }
            if(caster == null){
                return;
            }
            Vector3 casterPos = caster.transform.position;
            caster.transform.position = target.transform.position;
            target.transform.position = casterPos;
        }
        catch{}
        
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
    
    public Swap(){
        
    }
    public override AbilityEff clone()
    {
        Swap temp_ref = ScriptableObject.CreateInstance(typeof (Swap)) as Swap;
        copyBase(temp_ref);
        temp_ref.school = school;
        
        return temp_ref;
    }
    
}