using TheKiwiCoder;
using UnityEngine;

[TargetFinding]
public class SetTarget : ActionNode
{
    public HBCTools.ContextualTarget contextualTarget;
    public bool setToNull = false; 
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }
    /// <summary>
    ///	Success if an Actor is found or setToNull sets context's target to null. Fails if an Actor cannot be found
    /// </summary>
    protected override State OnUpdate()
    {
        if(setToNull)
        {
            context.actor.SetTarget(null);
            return State.Success;
        }

        try
        {
            context.actor.SetTarget(ContextTargetGetComponent<Actor>(contextualTarget));
            return State.Success;
        }
        catch
        {
            Debug.LogError(GetType() + ": Couldn't find an Actor with contextual target, " + contextualTarget);
            return State.Failure;
        }

    }
}
