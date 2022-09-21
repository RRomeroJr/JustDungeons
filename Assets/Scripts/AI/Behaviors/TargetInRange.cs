using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TargetInRange : ActionNode
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
        if (context.controller.TargetInRange(targetMask, range))
        {
            return State.Success;
        }
        return State.Failure;
    }
}
