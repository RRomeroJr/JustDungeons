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
    public Vector2 facingDirection;
     public float globalCooldown = 0.0f;
    public const float gcdBase = 2.0f;
    public virtual void Start(){
        //autoAttackClone = AbilityData.instance.AutoAttack.clone();
        facingDirection = Vector2.down;
    }
    
    public void MoveTowards(Vector3 pos){
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, pos, tempspeed);
    }
    
    public virtual void Update(){
        if(globalCooldown > 0.0f){
            globalCooldown -= Time.deltaTime;
        }
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
