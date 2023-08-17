using TheKiwiCoder;

[Movement]
public class MoveToPosition : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.agent.destination != blackboard.moveToPosition)
        {
            context.agent.destination = blackboard.moveToPosition;
        }

        if (context.agent.remainingDistance < 0.1)
        {
            return State.Success;
        }

        if (context.agent.pathPending)
        {
            return State.Running;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            return State.Running;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        return State.Success;
    }
}
