using TheKiwiCoder;
using UnityEngine;

[Movement]
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
    float moveSpeedHolder;


    protected override void OnStart()
    {
        moveToStarted = false;

        debugDist = 0.0f;
        debugHasPath = context.agent.isStopped;

        try
        {
            realPos = ContextualTargetToGmObj(relTarget).transform.position + (Vector3)relativePos;
        }
        catch
        {
            Debug.LogError("MoveToRelWP: Could not get realPos using self");
            realPos = context.transform.position + (Vector3)relativePos;
        }

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.controller.arenaObject == null)
        {
            Debug.Log("Skipping MoveToRelWP. Returning Failure");
            return State.Failure;
        }
        if (!moveToStarted)
        {
            moveToStarted = context.controller.moveToPoint(realPos);
            if (useMoveSpeed && moveToStarted)
            {
                moveSpeedHolder = context.agent.speed;
                context.agent.speed = moveSpeed;
            }
            return State.Running;
        }
        else
        {

            // if(context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            // {
            //     return State.Failure;
            // }
            if ((Vector2.Distance(realPos, context.transform.position) <= stopRange) && !context.controller.resolvingMoveTo)
            {
                if (useMoveSpeed)
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
