using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Attack : ActionNode
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
        context.actor.castAbility3(ability, blackboard.target.GetComponent<Actor>());
        return State.Success;
    }
}
