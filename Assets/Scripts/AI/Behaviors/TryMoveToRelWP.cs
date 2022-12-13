using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TryMoveToRelWP  : ActionNode
{   public Vector2 relativePos;
    Vector2 realPos;
    public bool useMoveSpeed = false;
    public float moveSpeed;

    protected override void OnStart()
    {
        realPos = context.controller.arenaObject.transform.position + (Vector3)relativePos;
        Debug.Log("target: " + context.controller.arenaObject.transform.position + " + relativePos: " + relativePos + " = realPos: " + realPos);
        if(useMoveSpeed){
            context.controller.moveToPoint(realPos, moveSpeed);
        }
        else{
            context.controller.moveToPoint(realPos);
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(context.controller.arenaObject == null){
            Debug.Log("Skipping MoveToRelWP. Reuturning Sucess");
            return State.Success;
        }
        if(context.controller.resolvingMoveTo){
            Debug.Log("Already a MoveTo Command. Returning Failure");
            return State.Failure;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }
         
        return State.Success;
        
    }
    
}
