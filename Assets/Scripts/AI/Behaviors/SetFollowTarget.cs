using TheKiwiCoder;

[Movement]
public class SetFollowTarget : ActionNode
{
    public bool setToNull = false;
    public bool setLockedTo = true;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (setToNull)
        {
            context.controller.followTargetLocked = setLockedTo;
            context.controller.SetFollowTarget(null, true);
            return State.Success;
        }
        else if (context.actor.target != null)
        {
            context.controller.followTargetLocked = setLockedTo;
            context.controller.SetFollowTarget(context.actor.target.gameObject, true);

            return State.Success;
        }

        return State.Failure;

    }
}
