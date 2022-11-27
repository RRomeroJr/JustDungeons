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
    public Vector2 moveDirection = Vector2.right;
    public GameObject indicatorPrefab;
    GameObject indicatorRef;
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
       
        //Hopefully this rotates the moveVector in the y axis by power every frame
        if(parentBuff.actor.isServer){
            indicatorRef.transform.position = parentBuff.actor.transform.position;
        }
        clientEffect();
        
    }
    public override void clientEffect()
    {
        //Debug.Log("calling dizzy clientside effect");
        
        if( (Input.GetKey("w")) || (Input.GetKey("a")) || (Input.GetKey("s")) || (Input.GetKey("d")) ){
            parentBuff.actor.GetComponent<Controller>().MoveTowards(moveDirection + (Vector2)parentBuff.actor.transform.position);
            Debug.DrawLine(parentBuff.actor.transform.position, (moveDirection * 2.5f) + (Vector2)parentBuff.actor.transform.position, Color.green);
        }
        else{
            
            moveDirection = Quaternion.Euler( 0, 0, power) * moveDirection;
            indicatorRef.transform.up = moveDirection;
            
            Debug.DrawLine(parentBuff.actor.transform.position, (moveDirection * 5.0f) + (Vector2)parentBuff.actor.transform.position, Color.red);
        }
    }
    public override void buffStartEffect()
    {
        indicatorRef = Instantiate(indicatorPrefab, parentBuff.actor.transform.position, Quaternion.identity);
        indicatorRef.transform.Rotate(moveDirection);
      parentBuff.actor.canMove = false;
    }
    public override void buffEndEffect()
    {
        Destroy(indicatorRef);
        Debug.Log(effectName + ": canMove = true;");
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
        temp_ref.powerScale = powerScale;
        temp_ref.school = school;
        temp_ref.targetIsSecondary = targetIsSecondary;
        temp_ref.indicatorPrefab = indicatorPrefab;
        return temp_ref;
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.white;
        Gizmos.DrawLine(parentBuff.actor.transform.position, moveDirection + (Vector2)parentBuff.actor.transform.position);
    }
}
