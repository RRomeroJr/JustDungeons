using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using Mirror;
using UnityEngine;
using TheKiwiCoder;

public enum CombatTemperment
{
    Hostle,
    Neutral,
    Passive
}

public class EnemyController : Controller
{
    [Header("Set If Needed")]

    public Arena arenaObject;
    [Header("Automatic")]

    public float aggroRadius = 7.0f;
    public Vector2 resetPoint = Vector2.zero;
    public CombatTemperment combatTemperment;
    public Actor aggroTarget;
    public int phase = 0;
    public uint tauntImmune = 0;
    public uint ignoreAggro = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        resetPoint = transform.position;
        actor.OnEnterCombat.AddListener(OnEnterCombat);
        actor.OnLeaveCombat.AddListener(OnLeaveCombat);
        //        agent.speed = enemyStats.moveSpeed;
    }

    public void FixedUpdate()
    {
        if (!isServer) { return; }
        // if ((Mathf.Abs(agent.desiredVelocity.magnitude) > 0.0f && !agent.isStopped)
        //         != tryingToMove)
        // {
        //     tryingToMove = !tryingToMove;
        //     CmdSetTryingToMove(tryingToMove);
        // }
        // CheckOverlaps();
        moveDirection = agent.desiredVelocity;
        moveDirection.Value.Normalize();

        MovementFacingDirection();
        
    }
    protected override void MovementFacingDirection()
    {
        // Debug.Log("PlayerController MovementFacingDirection");
        if (TryingToMove == false && holdDirection == false)
        {
            Vector2 _posToFace = Vector2.zero;
            if (resolvingMoveTo)
            {
                _posToFace = (Vector2)agent.destination;
            }
            else if (followTarget != null)
            {
                _posToFace = (Vector2)followTarget.transform.position;
            }
            FacePosistion(_posToFace);
        }
        else
        {
            base.MovementFacingDirection();
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!isServer) { return; }
        // Server only logic below

        AggroSetTargetsCheck();

        base.Update();

        // if(abilityHandler.IsCasting){
        //     if(actor.getQueuedAbility().castWhileMoving == false){
        //         if(agent.isStopped == false){
        //             agent.isStopped = true;
        //             /*
        //                 This is real ugly and stupid but idc. Couldn't figure out why Dio was moving
        //                 during ring aoe cast    
        //             */
        //         }
        //     }
        // }

        // if (!resolvingMoveTo && (followTarget != null))
        // {
        //     agent.SetDestination(followTarget.transform.position);

        // }
        if (combatTemperment == CombatTemperment.Hostle && actor.inCombat == false)
        {
            AggroSearch();
        }

        if (!holdDirection)
        {
            if (abilityHandler.IsCasting)
            {
                if (abilityHandler.QueuedAbility.needsActor)
                {
                    // Debug.Log("cast following actor");
                    // facingDirection = HBCTools.ToNearest45(abilityHandler.QueuedTarget.position - transform.position);
                    ServerSetFacingDirection(HBCTools.GetQuadrant(abilityHandler.QueuedTarget.position - transform.position));
                }
                else if (abilityHandler.QueuedAbility.needsWP)
                {
                    ServerSetFacingDirection(HBCTools.GetQuadrant(actor.getCastingWPToFace() - (Vector2)transform.position));
                }
            }
            else if (followTarget != null && !resolvingMoveTo)
            {
                ServerSetFacingDirection(HBCTools.GetQuadrant(followTarget.transform.position - transform.position));
            }
        }
        //    Debug.DrawLine(transform.position, (moveDirection.Value) + (Vector2)transform.position, Color.cyan);
    }
    // RR: Now that I'm writing this out it would be easier if 
    //     abilitys had cooldowns before I start doing this

    //void onClosest(Ability)
    //void onHighestThreat(Ability)
    //void onLowestTreat()
    //void atPercentHealth(Ability)
    //void onHealer()
    //void onRanged()
    //void onMelee()
    //void onTank()

    /// <summary>
    /// Find all the targets that are in the mask and in range
    /// </summary>
    /// <returns>Transform</returns>
    public Transform FindClosetTarget(LayerMask targetMask, float range)
    {
        Collider2D[] raycastHits = Physics2D.OverlapCircleAll((Vector2)transform.position, range, targetMask);
        Transform closest = null;

        foreach (Collider2D collider in raycastHits)
        {
            if (closest == null || DistanceTo(collider.transform) < DistanceTo(closest))
            {
                closest = collider.transform;
            }
        }

        return closest;
    }

    /// <summary>
    /// Find all the targets that are in the mask and in range
    /// </summary>
    /// <returns>List of Transforms</returns>
    public List<Transform> FindTargets(LayerMask targetMask, float range)
    {
        Collider2D[] raycastHits = Physics2D.OverlapCircleAll((Vector2)transform.position, range, targetMask);
        List<Transform> targets = new();

        foreach (Collider2D collider in raycastHits)
        {
            targets.Add(collider.transform);
        }

        return targets;
    }

    /// <summary>
    /// Find all the targets that are in the mask, range, and has role
    /// </summary>
    /// <returns>List of Transforms</returns>
    public List<Transform> FindTargetsByRole(LayerMask targetMask, float range, Role roleMask)
    {
        Collider2D[] raycastHits = Physics2D.OverlapCircleAll((Vector2)transform.position, range, targetMask);
        List<Transform> targets = new();
        
        if(roleMask == Role.Everything)
        {
            foreach (Transform raycastHitTransform in raycastHits.Select(x => x.transform))
            {
                targets.Add(raycastHitTransform);
            }
        }
        else if(roleMask != Role.None)
        {
            foreach (Transform raycastHitTransform in raycastHits.Select(x => x.transform))
            {
                if ((raycastHitTransform.GetComponent<Actor>().Role & roleMask) != 0) //if Role matches mask
                {
                    targets.Add(raycastHitTransform);
                }
            }
        }
        else // roleMask == Role.None
        {
            foreach (Transform raycastHitTransform in raycastHits.Select(x => x.transform))
            {
                if(raycastHitTransform.GetComponent<Actor>().Role == Role.None)
                {
                    targets.Add(raycastHitTransform);
                }
            }
        }

        return targets;
    }

    /// <summary>
    /// Find random target in the mask and range
    /// </summary>
    /// <returns>Transform</returns>
    public Transform FindRandomTarget(LayerMask targetMask, float range)
    {
        Collider2D[] raycastHits = Physics2D.OverlapCircleAll((Vector2)transform.position, range, targetMask);

        return raycastHits.Length > 0 ? raycastHits[Random.Range(0, raycastHits.Length)].transform : null;
    }

    private float DistanceTo(Transform pos)
    {
        return Vector2.Distance(transform.position, pos.transform.position);
    }

    private void moveOffOtherUnitsOutOfCombat()
    {
        /*
            Couldn't figure this out. I wanted to detect if a mob was overlaping with another
            mob, but couldn't figure out an easy way to do it

            Do not use.
        */
        LayerMask _mask = LayerMask.GetMask("Enemy");
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_mask);
        Collider2D[] _results = new Collider2D[] { };

        if (GetComponent<Collider2D>().OverlapCollider(contactFilter, _results) > 0)
        {
            Debug.Log("overlap detected trying to move");
        }
        else
        {
            Debug.Log("No overlap");
        }
    }

    public void AggroSearch()
    {
        Transform res = FindClosetTarget(LayerMask.GetMask("Player"), aggroRadius);

        if (res)
        {
            actor.CheckStartCombatWith(res.GetComponent<Actor>());
        }
    }
    
    public bool Aggro(Actor _aggroTarget, bool _setAutoAttack = true, bool _checkStartCombatWith = true)
    {
        if (_aggroTarget == null)
            Debug.Log("aggroTarget was null");
        // Debug.Log(gameObject.name + "aggroing to " + _aggroTarget.name);
        actor.target = _aggroTarget;
        aggroTarget = _aggroTarget;
        if(combatTemperment != CombatTemperment.Passive)
        {
            SetFollowTarget(_aggroTarget.gameObject);
            autoAttacking = _setAutoAttack;
        }
        
        if(_checkStartCombatWith){
            actor.CheckStartCombatWith(_aggroTarget);

            /* There is a annoying sistuation where, an actor hits another actor
                > Actor adds them to each other's attackers > Combat starts
                > OnEnterCombat here fires and calls Aggro > Then aggro would 
                reduntantly check to add the _aggrotarget as an attacker

                So I made this conditional here to stop that reduntant check.
                I could be alot of unnecessary looping through attacker lists
                otherwise when there are many actors involved
             */
        }

        return true;
    }

    [Server]
    private void AggroSetTargetsCheck()
    {
        if (aggroTarget == null)
        {
            return;
        }
        
        if(ignoreAggro > 0)
        {
            return;
        }
        if (!(actor.abilityHandler.IsCasting && actor.abilityHandler.RequestingCast))
        {
            if (actor.target != aggroTarget)
            {
                actor.target = aggroTarget;
            }
        }
        if((combatTemperment != CombatTemperment.Passive)&&(followTarget != aggroTarget.gameObject))
        {
            SetFollowTarget(aggroTarget.gameObject);
        }
    }

    protected override void OnEnterCombat()
    {
        if (aggroTarget != null)
        {
            return;
        }

        Aggro(actor.FirstAliveAttacker(), _checkStartCombatWith: false);
        resetPoint = transform.position;
        Debug.Log(gameObject.name + ": 1st aggro. Aggroing to.." + actor.target);
    }
    protected override void OnLeaveCombat()
    {
        StartCoroutine(Reset());
        Debug.Log(gameObject.name + ": " + actor.name + " OnLeaveCombat");
    }

    public void CheckStopToCast(Ability_V2 _toCast)
    {
        if (_toCast.getCastTime() > 0.0f || _toCast.isChannel)
        {
            StopAgentToCast();
        }
    }
    public IEnumerator Reset()
    {
        if(gameObject.TryGetComponent(out BuffHandler _comp))
        {
            _comp.RemoveAll();
        }
        actor.attackerList.Clear();
        SetFollowTarget(null, true);
        aggroTarget = null;
        actor.SetTarget(null);
        GetComponent<BehaviourTreeRunner>().tree.rootNode.state = Node.State.Success;
        while(true)
        {
            if(moveToPoint(resetPoint))
            {
                Debug.Log("moving to resetPoint: " +  resetPoint);
                break;
            }
            yield return new WaitForSeconds(0.0167f);
        }
        while(resolvingMoveTo)
        {
            yield return new WaitForSeconds(0.0167f);
        }
        //refull hp here
        //this could go wrong if max hp is buffed. Need a way to clear all effects 1st
        actor.Health = actor.MaxHealth;
        resetPoint = transform.position;
        GetComponent<BehaviourTreeRunner>().tree.Reset();
        GetComponent<BehaviourTreeRunner>().tree.rootNode.state = Node.State.Running;
    }

    void CheckOverlaps()
    {

        /*
            This actually isn't going to work because if there are more enemies then can fit in the radius
            they will bump each other out of melee range

            I actually need them to pathfind to a point on the circle that doesn't overlap with anyone.
            And just like in wow keep circling if none are found
        */
        if(!followTarget)
        {
            return;
        }
        if(resolvingMoveTo)
        {
            return;
        }

        float _dist = Mathf.Abs((transform.position - followTarget.transform.position).magnitude);
        if(_dist <= agent.stoppingDistance)
        {
            if(agent.obstacleAvoidanceType != UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance)
            {
                agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
            }
        }
        else
        {
            if(agent.obstacleAvoidanceType != UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance)
            {
                agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
            }
        }
    }
}
