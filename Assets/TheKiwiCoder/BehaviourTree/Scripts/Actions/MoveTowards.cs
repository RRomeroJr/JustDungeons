using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveTowards : ActionNode
{
    public bool useSpeed = true;
    public float speed;
    public float stopDistance;
    public Vector2 target;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(Vector3.Distance(context.transform.position, (Vector3)target ) <= stopDistance){
            return State.Success;
        }
        if(useSpeed){
            context.transform.position = Vector3.MoveTowards(context.transform.position, target, speed * Time.deltaTime);
        }
        else{
            context.transform.position = Vector3.MoveTowards(context.transform.position, target, 10 * Time.deltaTime);

        }
        return State.Success;
    }
    
}
