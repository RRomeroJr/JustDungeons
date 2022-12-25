using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AllPartnersComboReady : ActionNode
{
    bool checkIf = true;
    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
       
        if(context.gameObject.GetComponent<Multiboss>() == null){
            Debug.LogError("CheckComboReady on GameObject, " + context.gameObject.name + ",  with no Multiboss component");
            return State.Failure;
        }
        
        if(context.gameObject.GetComponent<Multiboss>().AllPartnersComboReady()){
            return State.Success;
        }
        else{
            return State.Failure;
        }
    }
}
