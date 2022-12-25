using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetEnableNavAgent : ActionNode
{
    public bool setEnableTo = false;
    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
       
     
        context.agent.enabled = setEnableTo;

        return State.Success;
        
    }
}
