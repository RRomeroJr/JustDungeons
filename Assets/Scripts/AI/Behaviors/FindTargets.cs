using TheKiwiCoder;
using UnityEngine;

// Finds targets within a range using a raycast set to a certain layermask and by default sets target to closest
// Random will set the target to a random one within a range
// Will also construct a list of multiple targets within range as long as random is not set
[TargetFinding]
public class FindTargets : ActionNode
{
    public LayerMask targetMask;
    public float range;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        blackboard.targets = context.controller.FindTargets(targetMask, range, out Transform closest);
        blackboard.target = closest;

        if (blackboard.targets.Count > 0)
        {
            // Debug.Log("FindTarget node seccessfully found a target!");
            return State.Success;
        }
        return State.Failure;
    }
}
