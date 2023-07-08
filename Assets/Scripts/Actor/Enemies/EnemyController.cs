using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using System.Data;

public enum CombatTemperment{
    Hostle,
    Neutral,
    Passive
}

public class EnemyController : Controller
{
    [Header("Set")]
    public LayerMask obstacleMask;
    [Header("Set If Needed")]

    public Arena arenaObject;
    [Header("Automatic")]
    public EnemySO enemyStats;

    public Transform target;
    public Vector3 spawnLocation;
    public Collider2D collider;
    public float aggroRadius = 7.0f;
    public CombatTemperment combatTemperment;
    public Actor aggroTarget;
    public int phase = 0;
    public uint tauntImmune = 0;
    protected override void Awake()
    {
        base.Awake();
        collider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        spawnLocation = transform.position;
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
            if(resolvingMoveTo)
            {
                _posToFace = (Vector2)agent.destination;
            }
            else if(followTarget != null)
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
        if(isServer){
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
            if(combatTemperment == CombatTemperment.Hostle && actor.inCombat == false)
            {
                AggroSearch();
            }                                                      
        }
        if(isServer && !holdDirection)
        {
            if(abilityHandler.IsCasting)
            {
                if(abilityHandler.QueuedAbility.needsActor)
                {
                    // Debug.Log("cast following actor");
                    facingDirection = HBCTools.ToNearest45(abilityHandler.QueuedTarget.transform.position - transform.position);
                }
                else if(abilityHandler.QueuedAbility.needsWP)
                {
                    facingDirection = HBCTools.ToNearest45(actor.getCastingWPToFace() - (Vector2)transform.position);
                }
            }
            else if(followTarget != null && !resolvingMoveTo)
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
    /// <remarks>Has side effect of settings the Controller and Actor Target to the closest target</remarks>
    /// <returns>List of Transforms</returns>
    public List<Transform> FindTargets(LayerMask targetMask, float range)
    {
        Collider2D[] raycastHits = Physics2D.OverlapCircleAll((Vector2)transform.position, range, targetMask); // May need to optimize with OverlapCircleNonAlloc
        List<Transform> targets = new();

        // If no target is found by raycast, set target to null
        if (raycastHits.Length <= 0)
        {
            target = null;
            actor.target = null;
            return targets;
        }

        Transform closest = null;
        foreach (Transform raycastHitTransform in raycastHits.Select(x => x.transform))
            {
            targets.Add(raycastHitTransform);
            if (closest == null || DistanceTo(raycastHitTransform) < DistanceTo(closest))
                {
                closest = raycastHitTransform;
                }
            }

            // Set target to closest
            target = closest;
            actor.target = target.GetComponent<Actor>();
        return targets;
        }

    /// <summary>
    /// Find all the targets that are in the mask, range, and has role
    /// </summary>
    /// <remarks>Has side effect of settings the Controller and Actor Target to the closest target</remarks>
    /// <returns>List of Transforms</returns>
    public List<Transform> FindTargetsByRole(LayerMask targetMask, float range, Role role)
    {
        Collider2D[] raycastHits = Physics2D.OverlapCircleAll((Vector2)transform.position, range, targetMask); // May need to optimize with OverlapCircleNonAlloc
        List<Transform> targets = new();

        // If no target is found by raycast, set target to null
        if (raycastHits.Length <= 0)
        {
        target = null;
        actor.target = null;
            return targets;
    }

        Transform closest = null;
        foreach (Transform raycastHitTransform in raycastHits.Select(x => x.transform))
        {
            if (raycastHitTransform.GetComponent<Actor>().Role != role)
            {
                continue;
                }

            targets.Add(raycastHitTransform);
            if (closest == null || DistanceTo(raycastHitTransform) < DistanceTo(closest))
                {
                closest = raycastHitTransform;
                }
            }

        // Set target to closest
            target = closest;
        actor.target = target.GetComponent<Actor>();
        return targets;
    }

    /// <summary>
    /// Find random target in the mask and range
    /// </summary>
    /// <remarks>Has side effect of settings the Controller and Actor Target to the random target</remarks>
    /// <returns>List of Transforms</returns>
    public Transform FindRandomTarget(LayerMask targetMask, float range)
    {
        Collider2D[] raycastHits = Physics2D.OverlapCircleAll((Vector2)transform.position, range, targetMask); // May need to optimize with OverlapCircleNonAlloc

        // If no target is found by raycast, set target to null
        if (raycastHits.Length <= 0)
        {
            target = null;
            actor.target = null;
            return null;
        }

        Transform random = raycastHits[Random.Range(0, raycastHits.Length)].transform;
        target = random;
        actor.target = random.GetComponent<Actor>();
        return random;
    }

    public float DistanceTo(Transform pos)
    {
        float distance = Vector2.Distance(transform.position, pos.transform.position);
        return distance;
    }
    void moveOffOtherUnitsOutOfCombat(){
        /*
            Couldn't figure this out. I wanted to detect if a mob was overlaping with another
            mob, but couldn't figure out an easy way to do it

            Do not use.
        */        
        LayerMask _mask = LayerMask.GetMask("Enemy");
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_mask);
        Collider2D[] _results = new Collider2D[]{};
        
        if(GetComponent<Collider2D>().OverlapCollider(contactFilter, _results) > 0){
            Debug.Log("overlap detected trying to move");
            
            }
        else{
            Debug.Log("No overlap");
        }
        
    }
    public void AggroSearch(){

        if(FindTargets(LayerMask.GetMask("Player"), aggroRadius))
        {
            actor.CheckStartCombatWith(actor.target);
            
        }

    }
    public bool Aggro(Actor _aggroTarget, bool _setAutoAttack = true, bool _checkStartCombatWith = true)
    {
        if(_aggroTarget == null)
            Debug.Log("aggroTarget was null");
        actor.target = _aggroTarget;
        aggroTarget = _aggroTarget;
        SetFollowTarget(_aggroTarget.gameObject);
        // followTarget = _aggroTarget.gameObject;
        autoAttacking = _setAutoAttack;
        
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
    void AggroSetTargetsCheck(){
        if(aggroTarget == null)
        {
            return;
        }
        
        if(tauntImmune > 0)
        {
            return;
        }
        if(actor.target != aggroTarget)
        {
            actor.target = aggroTarget;
        }
        if(followTarget != aggroTarget.gameObject)
        {
            SetFollowTarget(aggroTarget.gameObject);
        }
    }
    
    void OnEnterCombat()
    {
        if(aggroTarget != null)
        {
            return;
        }
        
        Aggro(actor.FirstAliveAttacker(), _checkStartCombatWith: false);
        Debug.Log("1st aggro. Aggroing to.." + actor.target);
    }
    public void CheckStopToCast(Ability_V2 _toCast)
    {
        if(_toCast.getCastTime() > 0.0f || _toCast.isChannel)
        {
            StopAgentToCast();
        }
    }
    
}
