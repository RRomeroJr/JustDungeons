using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SubIgnoreAggro : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        (context.controller as EnemyController).tauntImmune -= 1;
        return State.Success;
        
    }
}
