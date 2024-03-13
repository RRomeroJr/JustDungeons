using TheKiwiCoder;
using UnityEngine;

[TargetFinding]
public class TargetRoleRandom : ActionNode
{
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
        blackboard.targets = context.controller.FindTargetsByRole(LayerMask.GetMask("Player"), range, role);
        if (blackboard.targets.Count > 0)
        {
            blackboard.target = blackboard.targets[UnityEngine.Random.Range(0, blackboard.targets.Count)];
            return State.Success;
        }
        return State.Failure;
    }
}
