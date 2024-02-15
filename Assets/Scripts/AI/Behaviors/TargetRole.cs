using TheKiwiCoder;
using UnityEngine;

[TargetFinding]
public class TargetRole : ActionNode
{
    public LayerMask targetMask;
    public float range;
    public Role role = Role.Everything;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        blackboard.targets = context.controller.FindTargetsByRole(targetMask, range, role);
        if (blackboard.targets.Count > 0)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
