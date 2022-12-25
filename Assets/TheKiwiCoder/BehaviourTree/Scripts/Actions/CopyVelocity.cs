using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CopyVelocity : ActionNode
{
    public HBCTools.ContextualTarget contextualTarget;
   
    

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.gameObject.GetComponent<Rigidbody2D>().velocity = ContextualTargetToGmObj(contextualTarget).GetComponent<Rigidbody2D>().velocity;
        Debug.Log("copying y :" + ContextualTargetToGmObj(contextualTarget).GetComponent<Rigidbody2D>().velocity.y);
        return State.Success;
    }
}
