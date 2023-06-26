using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CanCast : ActionNode
{
    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
       
        if (!(context.actor.IsCasting || context.actor.buffHandler.Silenced > 0))
        {
            return State.Success;
            
        }
        else{
            return State.Failure;
        }
    }
}
