using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

[System.Serializable]
[CreateAssetMenu(fileName="Fear", menuName = "HBCsystem/Fear")]
public class Fear : AbilityEff
{   
    public int school = -1;
    public float navMeshSpeed = 3.0f; //in the future this should change the moveSpeedMod
    bool hasAuthOverNT = false;
    
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
       if(hasAuthOverNT){
            //parentBuff.actor.transform.position = Vector2.MoveTowards(parentBuff.actor.transform.position, parentBuff.target.transform.position, power);

            //parentBuff.actor.GetComponent<NavMeshAgent>().SetDestination(HBCtools.randomPointInRadius(parentBuff.Actor.transform.position, 4.0f));
            if(parentBuff.actor.tag == "Player"){
                 while( parentBuff.actor.GetComponent<NavMeshAgent>()
                    .SetDestination(HBCTools.randomPointInRadius(parentBuff.actor.transform.position, 4.0f)) == false){
                        Debug.Log("Fear path found: PLAYER");
                }
            }
            else{
                 Debug.Log("Fear path found: NPC");
                 parentBuff.actor.GetComponent<Controller>()
                    .moveToPoint(HBCTools.randomPointInRadius(parentBuff.actor.transform.position, 4.0f));
                
                
            }
       }
    }
    public override void buffStartEffect()
    {
        if(NetworkServer.active){
            parentBuff.actor.feared += 1;
            parentBuff.actor.GetComponent<Controller>().autoAttacking = false;
        }
        if(HBCTools.NT_AuthoritativeClient( parentBuff.actor.GetComponent<NetworkTransform>() ))
        {
            hasAuthOverNT = true;
        }

        if(hasAuthOverNT){
            //Debug.Log(effectName + ": Buff Start Effect");
            parentBuff.actor.GetComponent<NavMeshAgent>().enabled = true;
            parentBuff.actor.GetComponent<NavMeshAgent>().speed = navMeshSpeed;
            if(parentBuff.actor.tag == "Player"){
                 while( parentBuff.actor.GetComponent<NavMeshAgent>()
                    .SetDestination(HBCTools.randomPointInRadius(parentBuff.actor.transform.position, 4.0f)) == false){
                        Debug.Log("Fear path found: PLAYER");
                }
            }
            else{
                 Debug.Log("Fear path found: NPC");
                 parentBuff.actor.GetComponent<Controller>()
                    .moveToPoint(HBCTools.randomPointInRadius(parentBuff.actor.transform.position, 4.0f));
                
                
            }
           
            
        }
            
    }
        
    
    public override void buffEndEffect()
    {
        if(NetworkServer.active){
            parentBuff.actor.feared  -= 1;
            
        }
        
        //Debug.Log(effectName + ": Buff End Effect");
        if(hasAuthOverNT)
        {
            parentBuff.actor.GetComponent<NavMeshAgent>().ResetPath();
            if(parentBuff.actor.gameObject.tag == "Player"){
                parentBuff.actor.GetComponent<NavMeshAgent>().enabled = false;
            }

        }

    }
    public Fear(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
        targetIsSecondary = true;
    }
    public Fear(){
        
    }
    public override AbilityEff clone()
    {
        Fear temp_ref = ScriptableObject.CreateInstance(typeof (Fear)) as Fear;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.powerScale = powerScale;
        temp_ref.school = school;
        temp_ref.targetIsSecondary = targetIsSecondary;

        return temp_ref;
    }
}
