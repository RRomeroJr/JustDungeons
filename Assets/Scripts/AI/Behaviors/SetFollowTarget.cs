using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetFollowTarget : ActionNode
{   
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {

        if (context.controller.followTarget == null){
            if(context.actor.target != null){
                context.controller.SetFollowTarget(context.actor.target.gameObject);
            }
        }

        if (context.agent.pathPending)
        {
            return State.Success;
        }
        else{
            return State.Failure;
        }




        // else if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        // {
        //     return State.Failure;
        // }
        
    }
    float getStoppingDistance(GameObject _target){
        float selfDiagonal;
        float tragetDiagonal;
        selfDiagonal = Mathf.Sqrt(Mathf.Pow(context.gameObject.GetComponent<Renderer>().bounds.size.x, 2)
                            + Mathf.Pow(context.gameObject.GetComponent<Renderer>().bounds.size.x, 2));
        tragetDiagonal = Mathf.Sqrt(Mathf.Pow(context.controller.followTarget.GetComponent<Collider2D>().bounds.size.x, 2)
                            + Mathf.Pow(context.controller.followTarget.GetComponent<Collider2D>().bounds.size.x, 2));
        return ((tragetDiagonal + selfDiagonal) /2) * 0.9f;

                            
    }
}
