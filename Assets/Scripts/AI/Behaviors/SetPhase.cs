using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetPhase : ActionNode
{
    public int phase = 0;
    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
       
     
        context.controller.phase = phase;

        return State.Success;
        
    }
}
