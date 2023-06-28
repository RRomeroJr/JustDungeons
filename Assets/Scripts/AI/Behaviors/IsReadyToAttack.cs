using TheKiwiCoder;

[Attack]
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
