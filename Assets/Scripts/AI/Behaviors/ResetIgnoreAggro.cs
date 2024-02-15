using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ResetIgnoreAggro : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        (context.controller as EnemyController).ignoreAggro = 0;
        return State.Success;
        
    }
}
