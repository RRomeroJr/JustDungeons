using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TargetRole : ActionNode
{
    public Role role;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.controller.TargetRole(role))
        {
            return State.Success;
        }
        return State.Failure;
    }
}
