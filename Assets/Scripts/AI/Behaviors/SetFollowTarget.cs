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

        if (context.agent.destination != context.actor.target.transform.position)
        {
            context.controller.followTarget = context.actor.target.gameObject;
            
            
            context.agent.stoppingDistance = getStoppingDistance(context.controller.followTarget);
        }

        if (context.agent.pathPending)
        {
            return State.Running;
        }
        else if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }
        else{
            return State.Success;
        }
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
