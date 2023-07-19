using TheKiwiCoder;
using UnityEngine;

[Attack]
public class CircleAttack : ActionNode
{
    public Ability_V2 ability;
    public Vector2 relativePoint;
    public Vector2 relativePoint2;
    public HBCTools.ContextualTarget relativeTo;
    public bool castStarted;
    public bool castFinished;
    public bool randomRange;
    public float plusMinusX;
    public float plusMinusY;
    public Vector2 shootDirection = Vector2.right;
    public float spinStep;
    public float rotated = 0.0f;
    // moveDirection = Quaternion.Euler( 0, 0, power) * moveDirection;
    protected override void OnStart()
    {
        castStarted = false;
        castFinished = false;
        context.actor.onAbilityCastHooks.AddListener(checkCastedAbility);
        rotated = 0.0f;
        // MirrorTestTools._inst.ClientDebugLog("AttackRelativeWP OnStart()");

    }

    protected override void OnStop()
    {
        context.actor.onAbilityCastHooks.RemoveListener(checkCastedAbility);
    }

    protected override State OnUpdate()
    {
        Debug.DrawLine(context.transform.position, (Vector3)shootDirection + context.transform.position, Color.magenta);
        if (!castStarted)
        {

            if (ability.getCastTime() > 0.0)
            {
                context.agent.isStopped = true;

            }
            //Debug.Log(context.controller.target.GetComponent<Actor>().getActorName());
            Vector2 randomOffset = Vector2.zero;


            if (randomRange)
            {
                randomOffset = new Vector2(Random.Range(-plusMinusX, plusMinusX), Random.Range(-plusMinusY, plusMinusY));

            }
            switch (relativeTo)
            {
                case HBCTools.ContextualTarget.ArenaObject:
                    NullibleVector3 realPos1 = new NullibleVector3();
                    NullibleVector3 realPos2 = new NullibleVector3();
                    realPos1.Value = (context.gameObject.GetComponent<Controller>() as EnemyController).arenaObject.transform.position + (Vector3)relativePoint;
                    realPos2.Value = (context.gameObject.GetComponent<Controller>() as EnemyController).arenaObject.transform.position + (Vector3)relativePoint2;
                    realPos1.Value += (Vector3)randomOffset;

                    castStarted = context.actor.castAbilityRealWPs(ability, _WP: realPos1, _WP2: realPos2);
                    break;
                case HBCTools.ContextualTarget.Self:
                    relativePoint += randomOffset;
                    castStarted = context.actor.castAbility3(ability, _relWP: new NullibleVector3(relativePoint), _relWP2: new NullibleVector3(shootDirection));


                    break;
                default:
                    relativePoint += randomOffset;
                    castStarted = context.actor.castAbility3(ability, _relWP: new NullibleVector3(relativePoint), _relWP2: new NullibleVector3(shootDirection));
                    Debug.LogError("Unknown RelativeTarget. trying to usee actor");
                    break;

                    //context.actor.castRelativeToGmObj(ability, (context.gameObject.GetComponent<Controller>() as EnemyController).arenaObject.gameObject, relativePoint + randomOffset);
            }

            //castStarted = true;
        }
        if (castFinished == false)
        {
            return State.Running;
        }
        else
        {
            if (rotated >= 360.0f)
            {
                if (context.agent.isStopped)
                {
                    // Debug.Log("AttkRelWP: agent isStopped to false");
                    context.agent.isStopped = false;
                }
                //Debug.Log("Attck: isStopped " + context.agent.isStopped.ToString());
                return State.Success;
            }
        }

        rotated += spinStep;
        shootDirection = Quaternion.Euler(0, 0, spinStep) * shootDirection;
        castFinished = false;
        castStarted = false;
        return State.Running;

    }
    void checkCastedAbility(int _id)
    {
        if (_id == ability.id)
        {
            // MirrorTestTools._inst.ClientDebugLog("cast fired MATCH FOUND");
            castFinished = true;
        }
        else
        {
            //MirrorTestTools._inst.RpcDebugLog("cast fired no match found");
        }
    }
}
