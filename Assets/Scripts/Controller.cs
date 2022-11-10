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
    [SyncVar]
    public bool autoAttackRequest = false;
    
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
                    if(autoAttackRequest == false){
                        //request aa commad?
                        if(isServer){
                            handleAutoAttackRequest();
                        }
                        else{
                            requestAutoAttack();
                        }
                    }
                }
            }
 
        }
        if(isServer){
            if(autoAttackRequest == true){
                    if(actor.checkOnCooldown(autoAttackClone) == false){
                        autoAttackRequest = false;
                    
                    }
          
            }
        }
        

        if(followTarget != null){
            GetComponent<NavMeshAgent>().SetDestination(followTarget.transform.position);
        }
    }
    [Command]
    void requestAutoAttack(){
        
        handleAutoAttackRequest();
    }
    void handleAutoAttackRequest(){
        if(autoAttackRequest == true){
            return;
        }
        else if(actor.checkOnCooldown(autoAttackClone) == false){
            if(autoAttackRequest == false){
                //request aa commad?

                autoAttackRequest = actor.castAbility3(autoAttackClone); //eventually the req becomes true
               
            }
            
        }
    }
    public void moveToPoint(Vector2 pos){
        StartCoroutine(IE_moveToPoint(pos));
    }
    IEnumerator IE_moveToPoint(Vector2 pos){
        /*
            I have an idea here. Make this method hold the target like the node does
            then set agent.destination to pos and check to see if it arrived every 0.2s
            or so 
        */
        GameObject targetHolder = followTarget;
        agent.destination = pos;
            
        agent.stoppingDistance = 0;
        
        followTarget = null;
        agent.isStopped = false;
        while(!agent.isStopped){
            if (Mathf.Abs(Vector2.Distance(pos, gameObject.transform.position))
                    > agent.stoppingDistance + 0.1f){
                        //Debug.Log("Pathing Spam");
                yield return new WaitForSeconds(0.2f);
            }
            
            
        }
        agent.isStopped = true;
        followTarget = targetHolder;
        agent.stoppingDistance = getStoppingDistance(followTarget);
    }
    float getStoppingDistance(GameObject _target){
        float selfDiagonal;
        float tragetDiagonal;
        selfDiagonal = Mathf.Sqrt(Mathf.Pow(gameObject.GetComponent<Renderer>().bounds.size.x, 2)
                            + Mathf.Pow(gameObject.GetComponent<Renderer>().bounds.size.x, 2));
        tragetDiagonal = Mathf.Sqrt(Mathf.Pow(followTarget.GetComponent<Collider2D>().bounds.size.x, 2)
                            + Mathf.Pow(followTarget.GetComponent<Collider2D>().bounds.size.x, 2));
        return ((tragetDiagonal + selfDiagonal) /2) * 0.9f;

                            
    }
    
}
