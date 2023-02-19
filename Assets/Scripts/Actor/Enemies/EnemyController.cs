using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

public class EnemyController : Controller
{
    [Header("Set")]
    public LayerMask obstacleMask;
    [Header("Set If Needed")]

    public Arena arenaObject;
    [Header("Automatic")]
    public EnemySO enemyStats;
    // The main behaviour tree asset
    public BehaviourTreeRunner treeRunner;
    //public List<Ability_V2> abilities;

    public Transform target;
    public List<Transform> multiTargets;
    public Vector3 spawnLocation;
    public BoxCollider2D collider;
    public int phase = 0;
    
    

    public override void Awake()
    {
        base.Awake();
        collider = GetComponent<BoxCollider2D>();
        treeRunner = GetComponent<BehaviourTreeRunner>();
        multiTargets = new List<Transform>();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        spawnLocation = transform.position;
//        agent.speed = enemyStats.moveSpeed;

        // Update context for behavior tree
        Context context = Context.CreateFromGameObject(gameObject);
        context = Context.AddEnemyContext(gameObject, context);
        treeRunner.UpdateContext(context);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(isServer && !holdDirection)
        {
            if(actor.IsCasting)
            {
                if(actor.getQueuedAbility().needsActor)
                {
                    Debug.Log("cast following actor");
                    facingDirection = HBCTools.ToNearest45(actor.getQueuedTarget().transform.position - transform.position);
                }
                else if(actor.getQueuedAbility().needsWP)
                {
                    facingDirection = HBCTools.ToNearest45(actor.getCastingWPToFace() - (Vector2)transform.position);
                }
            }
            else if(followTarget != null && !resolvingMoveTo)
            {
                facingDirection = HBCTools.ToNearest45(followTarget.transform.position - transform.position);
                
            }
        }
       
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

    // Returns true if target is in range and false if no targets are in range
    // Sets the target to the closest target if multiple or a random enemy if random = true
    // Uses a circle raycast centered on the enemy and checks if any gameobjects on the target layer are hit
    public bool FindTargets(LayerMask targetMask, float range)
    {
        Transform closest;
        Collider2D[] raycastHit = Physics2D.OverlapCircleAll((Vector2)transform.position, range, targetMask); // May need to optimize with OverlapCircleNonAlloc
        multiTargets.Clear();

        // If a target is found by raycastHit
        if (raycastHit.Length > 0)
        {
            closest = raycastHit[0].transform;
            multiTargets.Add(raycastHit[0].transform);
            // Find the closest target if multiple and save all targets in range
            for (int i = 1; i < raycastHit.Length; i++)
            {
                multiTargets.Add(raycastHit[i].transform);
                if (DistanceTo(raycastHit[i].transform) < DistanceTo(closest))
                {
                    closest = raycastHit[i].transform;
                }
            }
            // Set target to closest
            target = closest;
            actor.target = target.GetComponent<Actor>();
            return true;
        }
        // Sets target to null. No targets in range
        target = null;
        actor.target = null;
        return false;
    }

    public bool TargetRole(Role r)
    {
        Transform closest = null;
        target = null;
        if (multiTargets.Count > 0)
        {
            for (int i = multiTargets.Count - 1; i >= 0; i--)
            {
                if (multiTargets[i].GetComponent<Actor>().getRole() != r)
                {
                    multiTargets.RemoveAt(i);
                }
                else if (closest == null ||
                         DistanceTo(multiTargets[i]) < DistanceTo(closest))
                {
                    closest = multiTargets[i];
                }
            }
            target = closest;
        }
        return target == null ? false : true;
    }

    // Sets target to random within multiTarget list
    // Return true if a random target is selected, false if no target is found
    public bool TargetRandom()
    {
        if (multiTargets.Count > 0)
        {
            target = multiTargets[Random.Range(0, multiTargets.Count)].transform;
            actor.target = target.GetComponent<Actor>();
            return true;
        }
        return false;
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
}
