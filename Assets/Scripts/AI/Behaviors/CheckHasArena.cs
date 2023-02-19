using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckHasArena : ActionNode
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
       
        if(checkIf == context.gameObject.GetComponent<EnemyController>().arenaObject != null ){
            return State.Success;
        }
        else{
            return State.Failure;
        }
    }
}
