using TheKiwiCoder;
using UnityEngine;

// Used to check if the set target is within a defined range
// Does not set a target, target has to be set by another behavior; FindTarget()
[TargetFinding]
public class IsTargetInRange : ActionNode
{
    public float range;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Vector2.Distance(blackboard.target.position, context.controller.transform.position) <= range)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
