using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BelowHealthPercent : DecoratorNode
{
    public float percentage;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {   // if you are below the health %
        if(context.gameObject.GetComponent<Actor>().getHealthPercent() < percentage){
            // Don't cast
            // 
            return State.Failure;
             //Done
        }
        else{ //If you are above the health %
            child.Update(); // then cast something
             // not done
             return State.Success;
        }
    }
}
