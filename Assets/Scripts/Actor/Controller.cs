using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;


public class Controller : NetworkBehaviour
{   [Header("For Now Needs To Be Assigned")]
    public Ability_V2 autoAttackClone;
    
    float tempspeed = 0.15f;
    [Header("Automatic")]
    public Actor actor;
    public NavMeshAgent agent;
    public bool autoAttacking;
    
    public GameObject followTarget;
    public bool resolvingMoveTo;
    public Vector2 facingDirection;
     public float globalCooldown = 0.0f;
    public const float gcdBase = 2.0f;
    [SyncVar]
    public bool autoAttackRequest = false;
    public float moveSpeed = 420;
    public List<Ability_V2> abilities;
    [SyncVar]
    public bool tryingToMove = false;
    public Vector2? moveDirection;
    
    public virtual void Awake(){
        
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        actor = GetComponent<Actor>();
    }
    public virtual void Start(){
        //autoAttackClone = AbilityData.instance.AutoAttack.clone();
        facingDirection = new Vector2(0.5f,-0.5f);
    }
    
    public void MoveTowards(Vector3 pos){
        
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, pos, tempspeed);
    }

    public void MoveInDirection(Vector2 _vect){ //vector with magnitude from 0 to 1 based on input 
        // _vect = (moveSpeed * Time.deltaTime) * _vect;

        // _vect.x *= (moveSpeed * Time.fixedDeltaTime);  //speed is just d is a distance in NavMeshAgent
        // _vect.y *= (moveSpeed * Time.fixedDeltaTime); // moveSpeed d/t / t == d/t^2
        if (moveDirection != null)
        {
            _vect = _vect.magnitude * (Vector2)moveDirection;
        }
        _vect = moveSpeed * _vect;
        gameObject.GetComponent<Rigidbody2D>().velocity = _vect;
    }
    
    public virtual void Update(){
        if(tag == "Player"){
            Debug.DrawLine(transform.position, (facingDirection * 2.5f) + (Vector2)transform.position, Color.green);
        } 
        
        if(globalCooldown > 0.0f){
            globalCooldown -= Time.deltaTime;
        }
        if(autoAttacking){
            if(actor.target != null){
                if(HBCTools.areHostle(actor, actor.target) == false){
                    autoAttacking = false;
                    return;
                }
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
        
        if(!resolvingMoveTo){
            if(followTarget != null){
                // if(!HBCTools.checkFacing(actor, followTarget)){
                //     facingDirection = HBCTools.ToNearest45(followTarget.transform.position - transform.position);
                // }
                GetComponent<NavMeshAgent>().SetDestination(followTarget.transform.position);
            }
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
    [Command]
    public void CmdSetTryingToMove(bool _valFromClient){
        tryingToMove = _valFromClient;
    }
    public bool moveToPoint(Vector2 pos){
        if(resolvingMoveTo){
            return false;
        }
        StartCoroutine(IE_moveToPoint(pos));
        return true;
    }
    public bool moveToPoint(Vector2 pos, float tempMoveSpeed){
        if(resolvingMoveTo){
            return false;
        }
        StartCoroutine(IE_moveToPoint(pos, tempMoveSpeed));
        return true;
    }
    public void moveOffOtherUnits(){
        moveToPoint(Vector2.up + (Vector2)transform.position);
    }
    IEnumerator IE_moveToPoint(Vector2 pos){
        /*
            I have an idea here. Make this method hold the target like the node does
            then set agent.destination to pos and check to see if it arrived every 0.2s
            or so 
        */
        if(resolvingMoveTo){
            Debug.Log("already resolving moving finishing early");
            agent.SetDestination(pos);
            yield return new WaitForSeconds(0.0f);
            
        }
        else{

            //Debug.Log("No Pending Path move to: " + pos);
            agent.SetDestination(pos);
            
            agent.stoppingDistance = 0;
            resolvingMoveTo = true;
            agent.isStopped = false;
            
            while(Mathf.Abs(Vector2.Distance(pos, gameObject.transform.position))
                        > agent.stoppingDistance + 0.1f){
                
                //Debug.Log("Pathing Spam");
                yield return new WaitForSeconds(0.2f);

            }
            //Debug.Log("Move To Finshed. Distance: " + Vector2.Distance(pos, gameObject.transform.position).ToString() + "Stopping distance: " + agent.stoppingDistance );
            resolvingMoveTo = false;
            if(followTarget != null){
                agent.stoppingDistance = getStoppingDistance(followTarget);
            }
            
        }
        
        
        
    }
     IEnumerator IE_moveToPoint(Vector2 pos, float tempMoveSpeed){
        float moveSpeedHolder = agent.speed;
        agent.speed = tempMoveSpeed;
        StartCoroutine(IE_moveToPoint(pos));
        while(!agent.isStopped){
            yield return new WaitForSeconds(0.2f);
        }

        agent.speed = moveSpeedHolder;
        
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
    public void SetFollowTarget(GameObject _target){
        followTarget = _target;
        if(followTarget != null){
            agent.stoppingDistance = getStoppingDistance(followTarget);
        }
        
    }
    [Command]
    protected void CmdSetFacingDirection(Vector2 _ClientFacingDirection){
        RpcSetFacingDirection(_ClientFacingDirection);
    }
    [ClientRpc(includeOwner = false)]
    protected void RpcSetFacingDirection(Vector2 _ownersFacingDirection){
        facingDirection = _ownersFacingDirection;
    }
    
    
}
