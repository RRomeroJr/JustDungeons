using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class Controller : NetworkBehaviour
{   float tempspeed = 0.02f;
    public Actor actor;
    public NavMeshAgent agent;
    public bool autoAttacking;
    public Ability_V2 autoAttackClone;
    public GameObject followTarget;
    public virtual void Start(){
        //autoAttackClone = AbilityData.instance.AutoAttack.clone();
    }
    
    public void MoveTowards(Vector3 pos){
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, pos, tempspeed);
    }
    
    public virtual void Update(){
        if(autoAttacking){
            
            if(actor.target != null){
                
                if(actor.checkOnCooldown(autoAttackClone) == false){
                    
                    actor.castAbility3(autoAttackClone);
                }
            }
        }
        if(followTarget != null){
            GetComponent<NavMeshAgent>().SetDestination(followTarget.transform.position);
        }
    }
}
