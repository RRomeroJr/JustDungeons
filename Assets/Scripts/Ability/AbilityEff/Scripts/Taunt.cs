using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="Taunt", menuName = "HBCsystem/Taunt")]
public class Taunt : AbilityEff
{   
    public int school = -1;
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        try
        {
                    if(this.target.isTauntable())
        {
            // Debug.Log(this.target.name + " tauntable. taunting");
            this.target.GetComponent<EnemyController>().aggroTarget = this.caster;
            this.target.CheckStartCombatWith(_caster);
            // this.target.GetComponent<Controller>().SetFollowTarget(this.caster.gameObject);
        }
        }
        catch{}
        
    }
    public Taunt(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public Taunt(){}
    public override AbilityEff clone()
    {
        Taunt temp_ref = ScriptableObject.CreateInstance(typeof (Taunt)) as Taunt;
        copyBase(temp_ref);
        temp_ref.school = school;

        return temp_ref;
    }
}
