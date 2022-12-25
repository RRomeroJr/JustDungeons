using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetComboReady : ActionNode
{
    public bool setTo = true;
    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
       
        if(context.gameObject.GetComponent<Multiboss>() == null){
            Debug.LogError("CheckComboReady on GameObject, "+ context.gameObject.name + ",  with no Multiboss component");
            return State.Failure;
        }

        context.gameObject.GetComponent<Multiboss>().comboReady = setTo;
        return State.Success;
        
    }
}
