using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RepeatForEachTarget : DecoratorNode
{
    private List<Transform> targets;
    private int targetsLeft;

    protected override void OnStart()
    {
        // Create new list so if blackboard list changes while this node is running, it doesn't affect the node.
        targets = new List<Transform>(blackboard.targets);
        targetsLeft = targets.Count;
        if (targetsLeft > 0)
        {
            blackboard.target = targets[targetsLeft - 1];
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        switch (child.Update())
        {
            case State.Running:
                return State.Running;

            case State.Failure:
                return State.Failure;

            case State.Success:
                targetsLeft--;
                if (targetsLeft <= 0)
                {
                    return State.Success;
                }
                blackboard.target = targets[targetsLeft - 1];
                return State.Running;

            default:
                return State.Failure;
        }
    }
}
