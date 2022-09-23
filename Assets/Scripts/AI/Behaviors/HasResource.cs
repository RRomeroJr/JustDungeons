using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class HasResource : ActionNode
{
    public Ability_V2 ability;
    float resourceCost;
    // store resourcetype
    protected override void OnStart()
    {
        // set resourcecost and resourcetype
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // call checkforresource
        return State.Success;
    }
}
