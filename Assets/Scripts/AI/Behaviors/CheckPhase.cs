using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhase : ActionNode
{
    public int phase;
    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        
        if(context.controller.phase == phase){
            return State.Success;
        }
        else{
            return State.Failure;
        }
    }
}
