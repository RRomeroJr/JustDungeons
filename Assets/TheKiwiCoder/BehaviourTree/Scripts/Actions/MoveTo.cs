using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveTo : ActionNode
{   public Vector2 pos;
    public GameObject targetHolder;
    protected override void OnStart()
    {
        context.agent.destination = pos;
            
        context.agent.stoppingDistance = 0;
        
        targetHolder = context.controller.followTarget;
        context.controller.followTarget = null;
        context.agent.isStopped = false;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }
        
        if (Mathf.Abs(Vector2.Distance(pos, context.gameObject.transform.position))
                    > context.agent.stoppingDistance + 1.0)
        {
            return State.Running;
        }
        else{
            context.agent.isStopped = true;
            context.controller.followTarget = targetHolder;
            context.agent.stoppingDistance = getStoppingDistance(context.controller.followTarget);
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
