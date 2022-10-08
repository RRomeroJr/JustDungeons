using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetAutoAttack : ActionNode
{
    public bool setTrue = true;
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.controller.autoAttacking = setTrue;
        return State.Success;
            
    }
        
}
