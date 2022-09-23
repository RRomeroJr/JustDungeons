using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class FollowTarget : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Vector2.Distance(context.controller.target.position, context.transform.position) <= context.boxCollider.size.x)
        {
            context.agent.ResetPath();
            return State.Success;
        }

        if (context.agent.destination != context.controller.target.position)
        {
            context.agent.destination = context.controller.target.position;
        }

        if (context.agent.pathPending)
        {
            return State.Running;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        return State.Running;
    }
}
