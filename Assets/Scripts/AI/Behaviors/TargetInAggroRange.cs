using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TargetInAggroRange : ActionNode
{
    public LayerMask targetMask;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.controller.TargetDetection(targetMask))
        {
            return State.Success;
        }
        return State.Failure;
    }
}
