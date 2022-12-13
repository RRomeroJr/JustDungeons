using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TryMoveToRelWP  : ActionNode
{   public Vector2 relativePos;
    Vector2 realPos;
    public bool useMoveSpeed = false;
    public float moveSpeed;
    public bool successfulStart = false;
    protected override void OnStart()
    {
        if(context.controller.arenaObject == null){
            Debug.Log("Skipping MoveToRelWP. No arena object. Reuturning Failure");
            return;
        }
        realPos = context.controller.arenaObject.transform.position + (Vector3)relativePos;
        //Debug.Log("target: " + context.controller.arenaObject.transform.position + " + relativePos: " + relativePos + " = realPos: " + realPos);
        if(useMoveSpeed){
            successfulStart = context.controller.moveToPoint(realPos, moveSpeed);
        }
        else{
            successfulStart = context.controller.moveToPoint(realPos);
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(!successfulStart){
            Debug.Log("Unsuccessful start");
            return State.Failure;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Path invalid");
            return State.Failure;
        }
        Debug.Log("TryMoveRelWP success");
        return State.Success;
        
    }
    
}
