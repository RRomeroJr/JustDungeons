using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TargetCheck : DecoratorNode
{
    public bool ContinueIfHasTargetIs = true;
    protected override void OnStart()
    {
       
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {  
        if((context.actor.target == null) == ContinueIfHasTargetIs){ //if continue condition is not met
            return State.Failure;
        }
        else{
            child.Update();
            return State.Success;
            
            
        }
    }
}
