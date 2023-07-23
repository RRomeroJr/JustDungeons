using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class Controller : NetworkBehaviour
{
    [Header("For Now Needs To Be Assigned")]
    public Ability_V2 autoAttackClone;

    [Header("Automatic")]
    protected Actor actor;
    protected AbilityHandler abilityHandler;
    public bool followTargetLocked = false;
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
    public bool holdPosistion = false;
    // public bool tryingToMove = false;
    public bool autoAttacking;
    public bool resolvingMoveTo;
    public bool autoAttackRequest = false;

    public virtual bool TryingToMove
    {
        get
        {
            if (agent.enabled == false)
            {
                return false;
            }
            return Mathf.Abs(agent.velocity.magnitude) > 0.0f && !agent.isStopped;
        }
    }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        actor = GetComponent<Actor>();
        rb2d = GetComponent<Rigidbody2D>();
        abilityHandler = GetComponent<AbilityHandler>();
        // GetComponent<AbilityHandler>().OnRequestingCast.AddListener(StopAgentToCast);
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
        if (tag != "Player" && !isServer)
        {
            return;
        }
        if (autoAttacking && actor.target != null)
        {
            if (HBCTools.areHostle(transform, actor.target.transform) == false)
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
            CheckToFollowSomething();

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
        // Debug.DrawLine(transform.position, (transform.position + (Vector3)(1f *_direction)), Color.cyan);
        Rigidbody2D _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(_vect);
    }

    protected virtual void MovementFacingDirection()
    {
        if (TryingToMove == false)
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

    // [Command]
    // public void CmdSetTryingToMove(bool _valFromClient)
    // {
    //     tryingToMove = _valFromClient;
    // }

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
        // Debug.Log(Vector2.Distance(pos, transform.position) + pos.ToString() + transform.position);
        // bool stoppedBefore = agent.isStopped;
        agent.ResetPath();
        // bool stoppedAfter = agent.isStopped;
        // Debug.Log("before: " + stoppedBefore + "after: " + stoppedAfter);
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
        /*
            This should probably be more intelligent. 
            
            Maybe a bunch of different versions like,
            GetStoppingDistanceMelee...
            or 
            GetStoppingDistance(Abilty_V2) that uses the ability range

            But for now it just defaults to standard melee range.

            Melee range as well should change depending on the size of both
            the attacker and the target. 

            if melee reange is 3.5f, it should be 3.5f from the the edges of the hitbox.

            Not just 3.5 from the center like it is now.
         */


        // float selfDiagonal;
        // float targetDiagonal;
        // selfDiagonal = Mathf.Sqrt(Mathf.Pow(gameObject.GetComponent<Renderer>().bounds.size.x, 2)
        //                     + Mathf.Pow(gameObject.GetComponent<Renderer>().bounds.size.x, 2));
        // targetDiagonal = Mathf.Sqrt(Mathf.Pow(followTarget.GetComponent<Collider2D>().bounds.size.x, 2)
        //                     + Mathf.Pow(followTarget.GetComponent<Collider2D>().bounds.size.x, 2));


        // Smallest of diagonals summed / 2 or 90% of melee range
        // return Mathf.Min(((targetDiagonal + selfDiagonal) / 2), (Ability_V2.meleeRange * 0.9f));

        //Straight up 90% of melee range
        return Mathf.Max(Ability_V2.meleeRange * 0.9f);
    }

    public bool SetFollowTarget(GameObject _target, bool ignoreLock = false)
    {
        if (followTargetLocked && !ignoreLock)
        {
            // Debug.Log("SetfollowTarget ignored. followTargetLocked");
            return false;
        }
        followTarget = _target;
        if (followTarget != null)
        {
            agent.stoppingDistance = getStoppingDistance(followTarget);
        }
        else
        {
            agent.ResetPath();
        }
        return true;
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
    public void ServerSetFacingDirection(HBCTools.Quadrant _direction)
    {
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

    public void FacePosistion(Vector2 _pos)
    {
        Vector2 result = HBCTools.ToNearest45(_pos - (Vector2)transform.position);

        if (facingDirection == result)
        {
            return;
        }

        facingDirection = result;
    }

    public bool ShouldFollowSomething()
    {
        if (followTarget == null)
        {
            return false;
        }
        if (!(resolvingMoveTo || holdDirection || holdPosistion))
        {
            if (!(actor.isCastImmobile() || actor.abilityHandler.RequestingCast))
            {
                return true;
            }
        }
        return false;
    }

    public void StopAgentToCast()
    {
        Debug.Log(name + ": StopAgentToCast");
        if (agent.enabled == false)
        {
            return;
        }
        Debug.Log(agent.isStopped + ": isStopped");
        if (agent.isStopped == false)
        {
            Debug.Log(name + ": Stopping");
            agent.velocity = Vector2.zero;
            agent.isStopped = true;
        }
    }

    public void CheckToFollowSomething()
    {
        if (!ShouldFollowSomething() || agent == null)
        {
            return;
        }

        agent.SetDestination(followTarget.transform.position);
        if (agent.enabled && agent.isStopped)
        {
            agent.isStopped = false;
            Debug.Log("Unstopping to follow something");
        }
    }
}
