using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private EnemyController controller;

    [Header("Set in inspector")]
    public LayerMask mask;
    public LayerMask obstacleMask;

    [Header("Debug Settings")]
    public Transform target;
    public Vector3 targetPrev;
    public BoxCollider2D collider;
    public Vector3 spawnLocation;
    public NavMeshAgent agent;
    [SerializeField] private Vector3[] path;    // Not used currently,
    [SerializeField] private int targetIndex;   // might integrate with Navmeshplus for debugging purposes

    private void Awake()
    {
        spawnLocation = transform.position;
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<EnemyController>();
        collider = GetComponent<BoxCollider2D>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = controller.enemyStats.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDetection();
    }

    void LateUpdate()
    {
        if (target)
        {
            targetPrev = target.position;
        }
    }

    public void RequestPath()
    {
        agent.SetDestination(target.position);
    }

    public void RequestPath(Vector3 location)
    {
        agent.SetDestination(location);
    }


    public void MoveTowards()
    {
        agent.ResetPath();
        transform.position = Vector3.MoveTowards(transform.position, target.position, controller.enemyStats.moveSpeed * Time.deltaTime);
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

    // Return true if target is within aggro range and set target. If multiple are in range, closest target is set
    public bool EnemyDetection()
    {
        Transform closest;
        Collider2D[] raycastHit = Physics2D.OverlapCircleAll((Vector2)transform.position, controller.enemyStats.aggroRange, mask); // May need to optimize with OverlapCircleNonAlloc

        if (raycastHit.Length > 0)
        {
            Debug.Log("Enemy Detected");
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

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
        if (controller)
        {
            Gizmos.color = target ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, controller.enemyStats.aggroRange);
        }
    }
}
