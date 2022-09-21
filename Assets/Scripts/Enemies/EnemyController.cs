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
    public Vector3 spawnLocation;
    public BoxCollider2D collider;
    public LayerMask obstacleMask;
    private Dictionary<string, object> extra = new Dictionary<string, object>();
    public NavMeshAgent agent;

    void Awake(){
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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

    public bool TargetDetection(LayerMask targetMask)
    {
        Transform closest;
        Collider2D[] raycastHit = Physics2D.OverlapCircleAll((Vector2)transform.position, enemyStats.aggroRange, targetMask); // May need to optimize with OverlapCircleNonAlloc

        if (raycastHit.Length > 0)
        {
            closest = raycastHit[0].transform;
            // Find the closest target if multiple
            for (int i = 1; i < raycastHit.Length; i++)
            {
                if (Vector3.Distance(transform.position, raycastHit[i].transform.position) < Vector3.Distance(transform.position, closest.position))
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

    public bool TargetInRange(float range)
    {
        if (target == null)
        {
            return false;
        }
        if (Vector3.Distance(transform.position, target.position) < range)
        {
            return true;
        }
        return false;
    }

    public bool targetBehindObstacle()
    {
        Vector3 colliderPos = transform.position + (Vector3)collider.offset;
        Vector3 direction = target.position - (transform.position + (Vector3)collider.offset);
        float distance = Vector3.Distance(colliderPos, target.position);
        if (Physics2D.BoxCast(colliderPos, collider.size, 0f, direction, distance, obstacleMask))
        {
            Debug.Log("Target behind obstacle");
            return true;
        }
        Debug.Log("Target not behind obstacle");
        return false;
    }
}
