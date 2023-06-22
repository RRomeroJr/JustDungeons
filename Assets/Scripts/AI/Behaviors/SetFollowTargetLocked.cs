using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetFollowTargetLocked : ActionNode
{
    public bool setTo = true;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.controller.followTargetLocked = setTo;
        return State.Success;
        
    }
}
