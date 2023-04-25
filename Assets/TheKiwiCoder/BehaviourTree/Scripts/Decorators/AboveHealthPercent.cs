using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AboveHealthPercent : DecoratorNode
{
    public float percentage;
    public bool mustComplete =false;
    bool triggered = false;
    protected override void OnStart()
    {
        triggered = false;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        /*
            The idea is that the req to update only goes thru if the cond is true. Failure means
            either

            1) cond not met
            or
            2) child beneath was Failure
        */
        bool isAbovePercent = context.gameObject.GetComponent<Actor>().getHealthPercent() > percentage;
        if (isAbovePercent == false)
        {
            if (mustComplete && triggered)
            {
                return child.Update();
            }
            
            return State.Failure;

        }
        State childStatus = child.Update();
        if (!triggered)
        {
            triggered = true;
        }
        return childStatus;
    }
}
