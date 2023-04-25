using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveTo : ActionNode
{   public Vector2 pos;
    public GameObject targetHolder;
    public bool useMoveSpeed = false;
    public float moveSpeed;
    public bool moveToStarted = false;
    float moveSpeedHolder;
    public float stopRange = 1.0f;

    protected override void OnStart()
    {
        moveToStarted = false;
        
        // try{
        //     realPos = ContextualTargetToGmObj(relTarget).transform.position + (Vector3)relativePos;
        // }
        // catch{
        //     Debug.LogError("MoveToRelWP: Could not get realPos using self");
        //     realPos = context.transform.position + (Vector3)relativePos;
        // }
        
        // if(useMoveSpeed){
        //     context.controller.moveToPoint(pos, moveSpeed);
        // }
        // else{
        //     context.controller.moveToPoint(pos);
        // }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {

       if(context.controller.arenaObject == null)
        {
            Debug.Log("Skipping MoveToRelWP. Returning Failure");
            return State.Failure;
        }
        if(!moveToStarted)
        {
            moveToStarted = context.controller.moveToPoint(pos);
            if(useMoveSpeed && moveToStarted)
            {
                moveSpeedHolder = context.agent.speed;
                context.agent.speed = moveSpeed;
            }
            return State.Running;
        }
        else
        {

            if(context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                return State.Failure;
            }
            if ((Vector2.Distance(pos, context.transform.position) <= stopRange) && !context.controller.resolvingMoveTo)
            {
                if(useMoveSpeed)
                {
                    context.agent.speed = moveSpeedHolder;
                }
                return State.Success;
            }
            else
            {
                return State.Running;
            }
        }
    }
    
}
