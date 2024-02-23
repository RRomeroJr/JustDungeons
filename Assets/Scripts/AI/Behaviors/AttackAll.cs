using TheKiwiCoder;

/// <remarks>
/// Differs from RepeatForEachTarget in that it doesn't wait for
/// castFinished callback and never returns a failed state
/// </remarks>
[Attack]
public class AttackAll : ActionNode
{
    public Ability_V2 ability;
    private int targetIndex;

    protected override void OnStart()
    {
        targetIndex = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (targetIndex >= blackboard.targets.Count)
        {
            return State.Success;
        }

        // Bit of a hack to get this to work. Because of the way abilities are queued in the AbilityHandler,
        // you can only cast a single ability per frame. This node cast an ability every frame till
        // all targets have abilities casted at them.
        context.actor.castAbility3(ability, blackboard.targets[targetIndex].transform);
        targetIndex++;

        return State.Running;
    }
}
