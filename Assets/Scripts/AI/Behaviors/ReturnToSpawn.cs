using TheKiwiCoder;
using UnityEngine;

[Movement]
public class ReturnToSpawn : ActionNode
{
    protected override void OnStart()
    {
        context.agent.destination = (Vector3)context.extra["spawnLocation"];
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
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
