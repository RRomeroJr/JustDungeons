using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

// Finds targets within a range using a raycast set to a certain layermask and by default sets target to closest
// Random will set the target to a random one within a range
// Will also construct a list of multiple targets within range as long as random is not set
public class TargetPartner : ActionNode
{
    public GameObject optMobTypePrefab;
    public float range;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(context.gameObject.GetComponent<Multiboss>() == null){
            return State.Failure;
        }
        if(context.gameObject.GetComponent<Multiboss>().partners.Count <= 0){
            return State.Failure;
        }
        if(optMobTypePrefab == null){
            blackboard.target = context.gameObject.GetComponent<Multiboss>().partners[0].transform;
            context.actor.target = context.gameObject.GetComponent<Multiboss>().partners[0];
            return State.Success;
        }
        else{
            foreach(Actor a in context.gameObject.GetComponent<Multiboss>().partners){
                if(a.mobId == optMobTypePrefab.GetComponent<Actor>().mobId){
                    blackboard.target = a.transform;
                    context.actor.target = a;
                    return State.Success;
                }
            }
        }
        return State.Failure;
    }
}
