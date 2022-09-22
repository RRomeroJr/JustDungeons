using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetTarget : ActionNode
{
    public Vector3 target;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (target != null)
        {
            return State.Failure;
        }
        //context.controller.target = target;
        return State.Success;
    }
}
