using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
[CreateAssetMenu(fileName="DisableMovement", menuName = "HBCsystem/DisableMovement")]
public class DisableMovement : AbilityEff
{   
    public int school = -1;
    
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
       
        
    }
    public override void buffStartEffect()
    {
    //Debug.Log("Dm start");
        //parentBuff.actor.canMove = false;
    }
    public override void buffEndEffect()
    {
        //Debug.Log("Dm end");
        //parentBuff.actor.canMove = true;
    }
    public DisableMovement(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
        targetIsSecondary = true;
    }
    public DisableMovement(){
        
    }
    public override AbilityEff clone()
    {
        DisableMovement temp_ref = ScriptableObject.CreateInstance(typeof (DisableMovement)) as DisableMovement;
        copyBase(temp_ref);
        temp_ref.school = school;

        return temp_ref;
    }
    
}
