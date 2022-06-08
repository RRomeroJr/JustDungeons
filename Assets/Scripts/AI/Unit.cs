using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private EnemyController controller;

    [Header("Set in inspector")]
    public LayerMask mask;

    [Header("Debug Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3[] path;
    [SerializeField] private int targetIndex;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyDetection())
        {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, controller.enemyStats.moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Return true if target is within aggro range and set target. If multiple are in range, closest target is set
    public bool EnemyDetection()
    {
        Transform closest;
        Collider2D[] raycastHit = raycastHit = Physics2D.OverlapCircleAll((Vector2)transform.position, controller.enemyStats.aggroRange, mask); // May need to optimize with OverlapCircleNonAlloc

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
        Gizmos.color = target ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, controller.enemyStats.aggroRange);
    }
}
