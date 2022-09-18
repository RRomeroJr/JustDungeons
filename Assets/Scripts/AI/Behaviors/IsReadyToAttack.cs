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
        bool test;
        test = context.actor.checkOnCooldown(ability);
        Debug.Log(test);

        if (test)
        {
            Debug.Log("here10");

            return State.Failure;
        }
        Debug.Log("here10");
        return State.Success;
    }
}
