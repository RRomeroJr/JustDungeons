using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class HoldDirection : ActionNode
{
    public bool setTo = true;
    protected override void OnStart()
    {
        context.controller.holdDirection = setTo;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
       
        
        return State.Success;
        
    }
}
