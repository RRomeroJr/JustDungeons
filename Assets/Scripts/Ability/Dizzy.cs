using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
[CreateAssetMenu(fileName="Dizzy", menuName = "HBCsystem/Dizzy")]
public class Dizzy : AbilityEff
{   
    public int school = -1;
    public Vector2 moveAngle = Vector2.right;
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
       
        //Hopefully this rotates the moveVector in the y axis by power every frame
        

        if( (Input.GetKey("w")) || (Input.GetKey("a")) || (Input.GetKey("s")) || (Input.GetKey("d")) ){
            parentBuff.actor.GetComponent<Controller>().MoveTowards(moveAngle + (Vector2)parentBuff.actor.transform.position);
            Debug.DrawLine(parentBuff.actor.transform.position, (moveAngle * 2.5f) + (Vector2)parentBuff.actor.transform.position, Color.green);
        }
        else{
            moveAngle = Quaternion.Euler( 0, 0, power) * moveAngle;
            Debug.DrawLine(parentBuff.actor.transform.position, (moveAngle * 5.0f) + (Vector2)parentBuff.actor.transform.position, Color.red);
        }

        

        
    }
    public override void buffStartEffect()
    {
      parentBuff.actor.canMove = false;
    }
    public override void buffEndEffect()
    {
     parentBuff.actor.canMove = true;
    }
    public Dizzy(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
        targetIsSecondary = true;
    }
    public Dizzy(){
        
    }
    public override AbilityEff clone()
    {
        Dizzy temp_ref = ScriptableObject.CreateInstance(typeof (Dizzy)) as Dizzy;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.school = school;
        temp_ref.targetIsSecondary = targetIsSecondary;

        return temp_ref;
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.white;
        Gizmos.DrawLine(parentBuff.actor.transform.position, moveAngle + (Vector2)parentBuff.actor.transform.position);
    }
}
