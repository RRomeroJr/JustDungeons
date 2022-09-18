using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TargetBehindObstacle : ActionNode
{
    public LayerMask obstacleMask;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector3 colliderPos = context.transform.position + (Vector3)context.boxCollider.offset;
        Vector3 direction = blackboard.target.position - (context.transform.position + (Vector3)context.boxCollider.offset);
        float distance = Vector3.Distance(colliderPos, blackboard.target.position);
        if (Physics2D.BoxCast(colliderPos, context.boxCollider.size, 0f, direction, distance, obstacleMask))
        {
            Debug.Log("Target behind obstacle");
            return State.Success;

        }
        Debug.Log("Target not behind obstacle");
        return State.Failure;
    }
}
