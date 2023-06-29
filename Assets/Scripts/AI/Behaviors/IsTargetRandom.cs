using TheKiwiCoder;

[TargetFinding]
public class TargetRandom : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.controller.TargetRandom())
        {
            return State.Success;
        }
        return State.Failure;
    }
}
