using TheKiwiCoder;
using UnityEngine;

[TargetFinding]
public class TargetRandom : ActionNode
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
        if (context.controller.FindRandomTarget(targetMask, range) != null)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
