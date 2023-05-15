using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Controller : NetworkBehaviour
{
    [Header("For Now Needs To Be Assigned")]
    public Ability_V2 autoAttackClone;
    
    [Header("Automatic")]
    protected Actor actor;
    protected AbilityHandler abilityHandler;
    public GameObject followTarget;
    public float globalCooldown = 0.0f;
    public const float gcdBase = 2.0f;
    public List<Ability_V2> abilities;

    // Movement
    [SyncVar]
    public float moveSpeed = 420;
    [SyncVar]
    public float moveSpeedModifier = 1.0f;
    [SyncVar]
    public Vector2? moveDirection;
    public Vector2 facingDirection;

    // Unity Components
    protected Rigidbody2D rb2d;
    protected NavMeshAgent agent;

    // State Values
    public bool holdDirection = false;
    public bool tryingToMove = false;
    public bool autoAttacking;
    public bool resolvingMoveTo;
    public bool autoAttackRequest = false;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        actor = GetComponent<Actor>();
        rb2d = GetComponent<Rigidbody2D>();
        abilityHandler = GetComponent<AbilityHandler>();
    }

    public virtual void Start()
    {
        //autoAttackClone = AbilityData.instance.AutoAttack.clone();
        facingDirection = new Vector2(0.5f, -0.5f);

        if (!isServer) { return; }
        // Server only logic below

        if (TryGetComponent(out BuffHandler b))
        {
            b.SpeedChanged += HandleSpeedChanged;
        }
    }

    public virtual void Update()
    {   
        Debug.DrawLine(transform.position, (facingDirection * 2.5f) + (Vector2)transform.position, Color.green);
        
        if (globalCooldown > 0.0f)
        {
            globalCooldown -= Time.deltaTime;
        }
        if (autoAttacking && actor.target != null)
        {
            if (HBCTools.areHostle(actor, actor.target) == false)
            {
                autoAttacking = false;
                return;
            }
            if (abilityHandler.CheckOnCooldown(autoAttackClone) == false)
            {
                if (autoAttackRequest == false)
                {
                    //request aa commad?
                    if (isServer)
                    {
                        handleAutoAttackRequest();
                    }
                    else
                    {
                        requestAutoAttack();
                    }
                }
            }
        }
        if (isServer)
        {
            if (autoAttackRequest && abilityHandler.CheckOnCooldown(autoAttackClone) == false)
            {
                autoAttackRequest = false;
            }
            if (!resolvingMoveTo && !holdDirection && followTarget != null)
            {
                /*

                    This should probably be in EnemyControler and not here. I didn't move it bc 
                    I don't remember why I added holdDirection to the conditions and I didn't 
                    want to break ChariotMan

                */
                // if(!HBCTools.checkFacing(actor, followTarget)){
                //     facingDirection = HBCTools.ToNearest45(followTarget.transform.position - transform.position);
                // }
                GetComponent<NavMeshAgent>().SetDestination(followTarget.transform.position);
            }
        }
    }

    public void MoveTowards(Vector3 pos)
    {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, pos, moveSpeed);
    }

    public void MoveInDirection(Vector2 _direction)
    { 
        MoveInDirection(_direction, moveSpeed);
    }
    public void MoveInDirection(Vector2 _direction, float _speed)
    { 
        // _direction.Normalize();
        
        // Vector2 _vect = moveSpeed * (Vector2)moveDirection * Time.fixedDeltaTime;
        // transform.position = (Vector2)transform.position + _vect;

        Vector2 _vect = _speed * _direction;
        Rigidbody2D _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(_vect);
    }
    protected virtual void MovementFacingDirection()
    {
        if(tryingToMove == false)
        {
            return;
        }
        HBCTools.Quadrant newVectQuad;
        newVectQuad = HBCTools.GetQuadrant(moveDirection.Value);
        if ((moveDirection.Value.x != 0.0f) && (moveDirection.Value.y != 0.0f))
        { 
            if (HBCTools.GetQuadrant(facingDirection) != newVectQuad)
            {
                facingDirection = HBCTools.QuadrantToVector(newVectQuad);
                CmdSetFacingDirection(facingDirection);
            }
        }
        
    }

    [Command]
    void requestAutoAttack()
    {
        handleAutoAttackRequest();
    }
    void handleAutoAttackRequest()
    {
        if (autoAttackRequest == true)
        {
            return;
        }
        if (abilityHandler.CheckOnCooldown(autoAttackClone) == false)
        {
            //request aa commad?

            autoAttackRequest = abilityHandler.CastAbility3(autoAttackClone); //eventually the req becomes true
        }
    }
    [Command]
    public void CmdSetTryingToMove(bool _valFromClient)
    {
        tryingToMove = _valFromClient;
    }
    public bool moveToPoint(Vector2 pos)
    {
        if (resolvingMoveTo)
        {
            return false;
        }
        resolvingMoveTo = true;
        StartCoroutine(IE_moveToPoint(pos));
        return true;
    }
    // public bool moveToPoint(Vector2 pos, float tempMoveSpeed){
    //     if(resolvingMoveTo){
    //         return false;
    //     }
    //     StartCoroutine(IE_moveToPoint(pos, tempMoveSpeed));
    //     return true;
    // }
    public void moveOffOtherUnits()
    {
        moveToPoint(Vector2.up + (Vector2)transform.position);
    }
    IEnumerator IE_moveToPoint(Vector2 pos)
    {
        /*
            I have an idea here. Make this method hold the target like the node does
            then set agent.destination to pos and check to see if it arrived every 0.2s
            or so 
        */
        
        //Debug.Log("No Pending Path move to: " + pos);
        agent.ResetPath();
        agent.SetDestination(pos);

        agent.stoppingDistance = 0;
        // resolvingMoveTo = true;
        agent.isStopped = false;

        while ((Vector2.Distance(pos, transform.position) > 0.1f))
        {
            if (!agent.hasPath && !agent.pathPending)
            {
                agent.SetDestination(pos);
            }
            yield return new WaitForSeconds(0.02f);
        }
        Debug.Log(Vector2.Distance(pos, transform.position) + pos.ToString() + transform.position);
        bool stoppedBefore = agent.isStopped;
        agent.ResetPath();
        bool stoppedAfter = agent.isStopped;
        Debug.Log("before: " + stoppedBefore + "after: " + stoppedAfter);
        //Debug.Log("Move To Finshed. Distance: " + Vector2.Distance(pos, gameObject.transform.position).ToString() + "Stopping distance: " + agent.stoppingDistance );
        resolvingMoveTo = false;
        if (followTarget != null)
        {
            agent.stoppingDistance = getStoppingDistance(followTarget);
        }
        
    }
    //  IEnumerator IE_moveToPoint(Vector2 pos, float tempMoveSpeed){
    //     float moveSpeedHolder = agent.speed;
    //     agent.speed = tempMoveSpeed;
    //     StartCoroutine(IE_moveToPoint(pos));
    //     while(resolvingMoveTo){
    //         yield return new WaitForSeconds(0.2f);
    //     }
    //     Debug.Log(actor.getActorName()+": Returning normal agent speed");
    //     agent.speed = moveSpeedHolder;

    // }

    float getStoppingDistance(GameObject _target)
    {
        float selfDiagonal;
        float tragetDiagonal;
        selfDiagonal = Mathf.Sqrt(Mathf.Pow(gameObject.GetComponent<Renderer>().bounds.size.x, 2)
                            + Mathf.Pow(gameObject.GetComponent<Renderer>().bounds.size.x, 2));
        tragetDiagonal = Mathf.Sqrt(Mathf.Pow(followTarget.GetComponent<Collider2D>().bounds.size.x, 2)
                            + Mathf.Pow(followTarget.GetComponent<Collider2D>().bounds.size.x, 2));
        return ((tragetDiagonal + selfDiagonal) / 2) * 0.9f;
    }
    public void SetFollowTarget(GameObject _target)
    {
        followTarget = _target;
        if (followTarget != null)
        {
            agent.stoppingDistance = getStoppingDistance(followTarget);
        }
    }
    [Command]
    protected void CmdSetFacingDirection(Vector2 _ClientFacingDirection)
    {
        RpcSetFacingDirection(_ClientFacingDirection);
    }
    [ClientRpc(includeOwner = false)]
    protected void RpcSetFacingDirection(Vector2 _ownersFacingDirection)
    {
        facingDirection = _ownersFacingDirection;
    }
    [Server]
    public void ServerSetFacingDirection(HBCTools.Quadrant _direction){
        facingDirection = HBCTools.QuadrantToVector(_direction);
        RpcSetFacingDirection(facingDirection);
    }

    [Server]
    private void HandleSpeedChanged(object sender, SpeedChangedEventArgs e)
    {
        // Convert slow multiplier to speed multiplier. 1.1 slow (10% slow) = 0.9 speed
        float tempSlow = 2.0f - e.Slow;
        moveSpeedModifier = tempSlow * e.Haste;
    }
}
