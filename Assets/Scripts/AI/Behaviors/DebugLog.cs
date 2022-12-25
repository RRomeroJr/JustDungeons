using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class DebugLog : ActionNode
{
    public string output;
    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
       
        Debug.Log(context.gameObject.name + ": " + output);
        return State.Success;
        
    }
}
