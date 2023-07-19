using TheKiwiCoder;
using UnityEngine;

[Movement]
public class MoveTowardsRel : ActionNode
{
    public HBCTools.ContextualTarget contextualTarget;
    public bool useSpeed = true;
    public float speed;
    public float stopDistance;
    public Vector2 relWP;
    Vector2 wp;


    protected override void OnStart()
    {
        wp = ContextualTargetToGmObj(contextualTarget).transform.position + (Vector3)relWP;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Vector3.Distance(context.transform.position, (Vector3)wp) <= stopDistance)
        {
            return State.Success;
        }
        if (useSpeed)
        {
            context.transform.position = Vector3.MoveTowards(context.transform.position, wp, speed * Time.deltaTime);
        }
        else
        {
            context.transform.position = Vector3.MoveTowards(context.transform.position, wp, 10 * Time.deltaTime);

        }
        return State.Success;
    }
}
