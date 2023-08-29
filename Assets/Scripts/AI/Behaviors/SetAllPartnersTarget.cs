using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SetAllPartnersTarget : SetTarget
{
    protected override void OnStart()
    {
 
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {

        if(setToNull)
        {
            context.transform.GetComponent<Multiboss>().SetAllPartnersTarget(null);
            return State.Success;
        }

        try
        {
            context.transform.GetComponent<Multiboss>().SetAllPartnersTarget(ContextTargetGetComponent<Actor>(contextualTarget));
            return State.Success;
        }
        catch
        {
            Debug.LogError(GetType() + ": Couldn't set targets with contextual target, " + contextualTarget);
            return State.Failure;
        }
       
    }
}
