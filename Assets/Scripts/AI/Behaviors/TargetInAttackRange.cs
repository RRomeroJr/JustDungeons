using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TargetInAttackRange : ActionNode
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
        if (context.controller.TargetInRange(range))
        {
            return State.Success;
        }
        return State.Failure;
    }
}
