using TheKiwiCoder;
using UnityEngine;

[Movement]
public class MoveToRole : ActionNode
{
    Transform targetTransform;
    public LayerMask targetMask;
    public Role roles = Role.Everything;
    public float range;
    public bool useMoveSpeed = false;
    public float moveSpeed;
    public bool moveToStarted = false;
    float moveSpeedHolder;
    public float stopRange = 1.0f;

    protected override void OnStart()
    {
        moveToStarted = false;
        var tempList = context.controller.FindTargetsByRole(targetMask, range, roles);
        if(tempList != null && tempList.Count != 0){
            Debug.Log(tempList.Count);
            targetTransform = tempList[Random.Range(0, tempList.Count)];
        }

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(targetTransform == null){
            // Debug.Log(context.gameObject.name + "->" + GetType() + " No " + roles.ToString());
            return State.Failure;
        }
        if (!moveToStarted)
        {
            moveToStarted = context.controller.moveToPoint(targetTransform.GetComponent<Collider2D>().bounds.BottomCenter());
            if (useMoveSpeed && moveToStarted)
            {
                moveSpeedHolder = context.agent.speed;
                context.agent.speed = moveSpeed;
            }
            return State.Running;
        }
        else
        {

            if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                return State.Failure;
            }
            if (!context.controller.resolvingMoveTo)
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
