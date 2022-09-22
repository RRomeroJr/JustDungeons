using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemySO enemyStats;
    // The main behaviour tree asset
    public BehaviourTree tree;
    public List<Ability_V2> abilities;

    private Actor actor;

    // Storage container object to hold game object subsystems
    Context context;
    public Transform target;
    public List<Transform> multiTargets;
    public Vector3 spawnLocation;
    public BoxCollider2D collider;
    public LayerMask obstacleMask;
    private Dictionary<string, object> extra = new Dictionary<string, object>();
    public NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        multiTargets = new List<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = transform.position;
        ExtraValues();
        context = Context.CreateFromGameObject(gameObject, extra, enemyStats);
        tree = tree.Clone();
        tree.Bind(context);
        spawnLocation = transform.position;
        actor = gameObject.GetComponent<Actor>();
        collider = GetComponent<BoxCollider2D>();
        agent.speed = enemyStats.moveSpeed;
    }

    // Any extra values you want the behavior tree to have access to should be added here
    void ExtraValues()
    {
        extra["spawnLocation"] = spawnLocation;
    }

    // Update is called once per frame
    void Update()
    {
        if (tree)
        {
            tree.Update();
        }
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
                return true;
            }

            closest = raycastHit[0].transform;
            // Find the closest target if multiple
            for (int i = 1; i < raycastHit.Length; i++)
            {
                multiTargets.Add(raycastHit[i].transform);
                if (Vector2.Distance(transform.position, raycastHit[i].transform.position) < Vector2.Distance(transform.position, closest.position))
                {
                    closest = raycastHit[i].transform;
                }
            }
            target = closest;
            return true;
        }
        target = null;
        return false;
    }
}
