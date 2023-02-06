using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveToRelWP : ActionNode
{   
    public HBCTools.ContextualTarget relTarget;
    public Vector2 relativePos;
    Vector2 realPos;
    public bool useMoveSpeed = false;
    public float moveSpeed;
    public bool moveToStarted = false;
    public float stopRange = 1.0f;
    public float debugDist;
    public bool debugHasPath;

    protected override void OnStart()
    {
        moveToStarted = false;
        debugDist = 0.0f;
        debugHasPath = context.agent.isStopped;
        try{
            realPos = ContextualTargetToGmObj(relTarget).transform.position + (Vector3)relativePos;
        }
        catch{
            if(ContextualTargetToGmObj(relTarget) == null){
                Debug.Log("ContextualTargetToGmObj(relTarget) doesn't work");
            }
            else{
                Debug.Log("You have a gmObj wtf just **** work you ****");
            }
            Debug.LogError("MoveToRelWP: Could not get realPos using self");
            realPos = context.transform.position + (Vector3)relativePos;
        }
        //realPos = ContextualTargetToGmObj(relTarget).transform.position + (Vector3)relativePos;
//        Debug.Log("target: " + context.controller.arenaObject.transform.position + " + relativePos: " + relativePos + " = realPos: " + realPos);
        // if(useMoveSpeed){
        //     context.controller.moveToPoint(realPos, moveSpeed);
        // }
        // else{
        //     context.controller.moveToPoint(realPos);
        // }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Color lineColor;
        if(!moveToStarted){
            lineColor = Color.gray;
        }
        else if(context.agent.pathPending){
            lineColor = Color.cyan;
        }
        else if(context.agent.hasPath){
            if(context.agent.isStopped){
                lineColor = Color.red;
            }
            else{
                lineColor = Color.white;
            }
        }
        else{
            if(context.agent.isStopped){
                lineColor = Color.blue;
            }
            else{
                lineColor = Color.magenta;
            }
        }
        Debug.DrawLine(context.transform.position, realPos, lineColor);
        debugDist = Vector2.Distance(realPos, context.transform.position);
        debugHasPath = context.agent.hasPath;
        if(context.controller.arenaObject == null){
            Debug.Log("Skipping MoveToRelWP. Returning Failure");
            return State.Failure;
        }
        if(!moveToStarted){
            if(useMoveSpeed){
                moveToStarted = context.controller.moveToPoint(realPos, moveSpeed);
                return State.Running;

            }
            else{
                moveToStarted = context.controller.moveToPoint(realPos);
                return State.Running;
            }
        }
        else{
            

            if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                return State.Failure;
            }
            if((Vector2.Distance(realPos, context.transform.position) <= stopRange) && !context.agent.hasPath){
                return State.Success;
            }
            else{
                
                
                return State.Running;
            }
            // if (context.controller.resolvingMoveTo)
            // {
            //     return State.Running;
            // }
            // else{
                
            //     return State.Success;
            // }
        }
        
    }
    
}
