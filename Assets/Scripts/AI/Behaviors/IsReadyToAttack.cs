using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class IsReadyToAttack : ActionNode
{
    public Ability_V2 ability;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.actor.checkOnCooldown(ability))
        {
            return State.Failure;
        }
        return State.Success;
    }
}
