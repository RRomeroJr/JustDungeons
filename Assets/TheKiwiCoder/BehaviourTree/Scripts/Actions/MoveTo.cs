using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveTo : ActionNode
{   public Vector2 pos;
    public GameObject targetHolder;
    public bool useMoveSpeed = false;
    public float moveSpeed;

    protected override void OnStart()
    {
        if(useMoveSpeed){
            context.controller.moveToPoint(pos, moveSpeed);
        }
        else{
            context.controller.moveToPoint(pos);
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {

       if(context.controller.arenaObject == null){
            Debug.Log("Skipping MoveTo. Reuturning Sucess");
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }
        
        if (context.controller.resolvingMoveTo)
        {
            return State.Running;
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
