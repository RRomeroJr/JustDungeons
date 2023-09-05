using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mirror;
using UnityEngine;

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
        actor.OnEnterCombat.AddListener(OnEnterCombat);
        //        agent.speed = enemyStats.moveSpeed;
    }

    public void FixedUpdate()
    {
        // if ((Mathf.Abs(agent.desiredVelocity.magnitude) > 0.0f && !agent.isStopped)
        //         != tryingToMove)
        // {
        //     tryingToMove = !tryingToMove;
        //     CmdSetTryingToMove(tryingToMove);
        // }
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
                    facingDirection = HBCTools.ToNearest45(abilityHandler.QueuedTarget.position - transform.position);
                }
                else if (abilityHandler.QueuedAbility.needsWP)
                {
                    facingDirection = HBCTools.ToNearest45(actor.getCastingWPToFace() - (Vector2)transform.position);
                }
            }
            else if (followTarget != null && !resolvingMoveTo)
            {
                facingDirection = HBCTools.ToNearest45(followTarget.transform.position - transform.position);

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
    public List<Transform> FindTargetsByRole(LayerMask targetMask, float range, Role role)
    {
        Collider2D[] raycastHits = Physics2D.OverlapCircleAll((Vector2)transform.position, range, targetMask);
        List<Transform> targets = new();

        foreach (Transform raycastHitTransform in raycastHits.Select(x => x.transform))
        {
            if (raycastHitTransform.GetComponent<Actor>().Role != role)
            {
                continue;
            }

            targets.Add(raycastHitTransform);
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

        return raycastHits[Random.Range(0, raycastHits.Length)].transform;
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
    public void AggroSearch(){
        Transform res = FindClosetTarget(LayerMask.GetMask("Player"), aggroRadius);

        if (res)
        {
            actor.CheckStartCombatWith(res.GetComponent<Actor>());
        }

    public bool Aggro(Actor _aggroTarget, bool _setAutoAttack = true, bool _checkStartCombatWith = true)
    {
        if (_aggroTarget == null)
            Debug.Log("aggroTarget was null");
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
                I could be alot of unecessary looping through attacker lists
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

    private void OnEnterCombat()
    {
        if (aggroTarget != null)
        {
            return;
        }

        Aggro(actor.FirstAliveAttacker(), _checkStartCombatWith: false);
        Debug.Log("1st aggro. Aggroing to.." + actor.target);
    }

    public void CheckStopToCast(Ability_V2 _toCast)
    {
        if (_toCast.getCastTime() > 0.0f || _toCast.isChannel)
        {
            StopAgentToCast();
        }
    }
}
