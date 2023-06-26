using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CancelCasts : ActionNode
{
    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.actor.abilityHandler.RpcResetCastVars();
        return State.Success;
    }
}
