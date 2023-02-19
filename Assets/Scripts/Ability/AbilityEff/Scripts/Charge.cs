using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
[CreateAssetMenu(fileName="Charge", menuName = "HBCsystem/Charge")]
public class Charge : AbilityEff
{   
    public int school = -1;
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
       //parentBuff.actor.transform.position = Vector2.MoveTowards(parentBuff.actor.transform.position, parentBuff.target.transform.position, power);

        parentBuff.actor.GetComponent<NavMeshAgent>().SetDestination(parentBuff.target.transform.position);
        if(Vector2.Distance(parentBuff.actor.transform.position, parentBuff.target.transform.position) < 1.8f){
            parentBuff.onFinish();
        }
    }
    public override void buffStartEffect()
    {
        //Debug.Log(effectName + ": Buff Start Effect");
        parentBuff.actor.GetComponent<NavMeshAgent>().enabled = true;
        
    }
    public override void buffEndEffect()
    {
        
        //Debug.Log(effectName + ": Buff End Effect");
        parentBuff.actor.GetComponent<NavMeshAgent>().ResetPath();
        if(parentBuff.actor.gameObject.tag == "Player"){
            parentBuff.actor.GetComponent<NavMeshAgent>().enabled = false;
        }
        
        
    }
    public Charge(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
        targetIsSecondary = true;
    }
    public Charge(){
        
    }
    public override AbilityEff clone()
    {
        Charge temp_ref = ScriptableObject.CreateInstance(typeof (Charge)) as Charge;
        copyBase(temp_ref);

        temp_ref.school = school;
        

        return temp_ref;
    }
}
