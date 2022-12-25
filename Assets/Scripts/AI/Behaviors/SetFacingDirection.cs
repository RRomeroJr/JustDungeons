using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetFacingDirection : ActionNode
{
    public HBCTools.Quadrant direction;
    protected override void OnStart()
    {
        context.controller.facingDirection = HBCTools.QuadrantToVector(direction);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
       
        
        return State.Success;
        
    }
}
