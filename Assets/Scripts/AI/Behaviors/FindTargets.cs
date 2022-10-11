using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

// Finds targets within a range using a raycast set to a certain layermask and by default sets target to closest
// Random will set the target to a random one within a range
// Will also construct a list of multiple targets within range as long as random is not set
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
        if (context.controller.FindTargets(targetMask, range))
        {
            return State.Success;
        }
        return State.Failure;
    }
}
