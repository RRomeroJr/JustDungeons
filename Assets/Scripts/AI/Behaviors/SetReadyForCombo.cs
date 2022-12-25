using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetReadyForCombo : ActionNode
{
    public int comboNumber = 0;
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
        
        context.gameObject.GetComponent<Multiboss>().lastCombo = comboNumber;
        context.gameObject.GetComponent<Multiboss>().comboReady = true;

        return State.Success;
        
    }
}
