using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TargetInAttackRange : ActionNode
{
    public float attackRange;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (blackboard.target == null)
        {
            return State.Failure;
        }
        if (Vector3.Distance(context.transform.position, blackboard.target.position) < attackRange)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
