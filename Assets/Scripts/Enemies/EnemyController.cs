using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

public class EnemyController : Controller
{
    public EnemySO enemyStats;
    // The main behaviour tree asset
    public BehaviourTreeRunner treeRunner;
    public List<Ability_V2> abilities;

    public Transform target;
    public List<Transform> multiTargets;
    public Vector3 spawnLocation;
    public BoxCollider2D collider;
    public LayerMask obstacleMask;
    public Arena arenaObject;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        actor = GetComponent<Actor>();
        collider = GetComponent<BoxCollider2D>();
        treeRunner = GetComponent<BehaviourTreeRunner>();
        multiTargets = new List<Transform>();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        spawnLocation = transform.position;
        agent.speed = enemyStats.moveSpeed;

        // Update context for behavior tree
        Context context = Context.CreateFromGameObject(gameObject);
        context = Context.AddEnemyContext(gameObject, context);
        treeRunner.UpdateContext(context);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        /*for(i = 0; i < AbilityList.Count(); i++){
            if(AbilityList[i].CD is ready?){
                
                // Resolve AbilityList[i] some how
                // maybe AbilityList has a some field that corresponds
                // to how the Ability should be cast.
                // 0 = onClostest, 1 = onHightestThreat, ect.
                
            }
        }
        */
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
    public bool FindTargets(LayerMask targetMask, float range, bool random = false)
    {
        Transform closest;
        Collider2D[] raycastHit = Physics2D.OverlapCircleAll((Vector2)transform.position, range, targetMask); // May need to optimize with OverlapCircleNonAlloc
        multiTargets.Clear();

        // If a target is found by raycastHit
        if (raycastHit.Length > 0)
        {
            // Set target to a random target within range
            if (random)
            {
                target = raycastHit[Random.Range(0, raycastHit.Length)].transform;
                actor.target = target.GetComponent<Actor>();
                return true;
            }

            closest = raycastHit[0].transform;
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
                    if (multiTargets[i] == closest)
                    {
                        closest = null;
                    }
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

    public float DistanceTo(Transform pos)
    {
        float distance = Vector2.Distance(transform.position, pos.transform.position);
        return distance;
    }
}
