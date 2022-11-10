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
        if((context.actor.target == null) == ContinueIfHasTargetIs){
            return State.Failure;
        }
        else{
            //This unit has a target
            switch (child.Update()) {
                case State.Running:
                    return State.Running;
                    break;
                case State.Failure:
                    return State.Failure;
                    break;
                case State.Success:
                    return State.Success;
                    break;


                default:
                    //unnecessary but I couldn't complie without it
                    return State.Failure;
                    break;
            }
            
        }
    }
}
