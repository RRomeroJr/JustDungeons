using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TargetInAggroRange : ActionNode
{
    public LayerMask targetMask;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Transform closest;
        Collider2D[] raycastHit = Physics2D.OverlapCircleAll((Vector2)context.transform.position, context.stats.aggroRange, targetMask); // May need to optimize with OverlapCircleNonAlloc

        if (raycastHit.Length > 0)
        {
            Debug.Log("Enemy Detected");
            closest = raycastHit[0].transform;
            // Find the closest target if multiple
            for (int i = 1; i < raycastHit.Length; i++)
            {
                if (Vector3.Distance(context.transform.position, raycastHit[i].transform.position) < Vector3.Distance(context.transform.position, closest.position))
                {
                    closest = raycastHit[i].transform;
                }
            }
            Debug.Log("here1");

            blackboard.target = closest;
            return State.Success;
        }
        Debug.Log("here2");

        blackboard.target = null;
        return State.Failure;
    }
}
