using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckComboFinished : ActionNode
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
       
        if(context.gameObject.GetComponent<Multiboss>() == null){
            Debug.LogError("CheckComboFinish on GameObject, "+ context.gameObject.name + ",  with no Multiboss component");
            return State.Failure;
        }
        
        if(context.gameObject.GetComponent<Multiboss>().comboFinished == checkIf){
            return State.Success;
        }
        else{
            return State.Failure;
        }
    }
}
