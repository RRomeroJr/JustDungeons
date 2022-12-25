using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ReadyAllForCombo : ActionNode
{
    public int comboNumber = 1;
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
        if(context.gameObject.GetComponent<Multiboss>().comboReady){
            return State.Success;
        }
        context.gameObject.GetComponent<Multiboss>().multibossCoordinator.comboEvent?.Invoke(comboNumber);
        return State.Success;
        
    }
}
