using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AddIgnoreAggro : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        (context.controller as EnemyController).ignoreAggro += 1;
        return State.Success;
        
    }
}
