using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckCasting : ActionNode
{
    public bool checkIf = true;
    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
       
        if(checkIf == context.gameObject.GetComponent<Actor>().IsCasting ){
            return State.Success;
        }
        else{
            return State.Failure;
        }
    }
}
