using TheKiwiCoder;
using UnityEngine;

[Attack]
public class Attack : ActionNode
{
    public Ability_V2 ability;
    bool castStarted;
    bool castFinished;
    public bool targetSelf = false;
    public bool tryOnce = false;
    protected override void OnStart()
    {
        //castStarted = false;
        castFinished = false;
        if(!tryOnce)
        {
            context.actor.onAbilityCastHooks.AddListener(checkCastedAbility);
        }
    }

    protected override void OnStop()
    {
        if(!tryOnce){
            context.actor.onAbilityCastHooks.RemoveListener(checkCastedAbility);
        }
    }

    protected override State OnUpdate()
    {
        if(context.actor.IsCasting == false){

            // if((ability.getCastTime() > 0.0)&&(ability.castWhileMoving == false)){
            //     context.agent.isStopped = true;

            // }
            //Debug.Log(context.controller.target.GetComponent<Actor>().getActorName());
            Transform _target = targetSelf ? context.actor.transform : blackboard.target;
            if (_target != null)
            {
                if (tryOnce)
                {
                    return BoolToState(context.actor.castAbility3(ability, _target));
                }
                else
                {
                    context.actor.castAbility3(ability, _target);
                }
            }
            else // If no actor is attached to target, use the target's position
            {
                if (tryOnce)
                {
                    return BoolToState(context.actor.castAbilityRealWPs(ability, _WP: new NullibleVector3(blackboard.target.transform.position)));
                }
                else
                {
                    context.actor.castAbilityRealWPs(ability, _WP: new NullibleVector3(blackboard.target.transform.position));
                }
            }
        }
        else
        {
            if(tryOnce)
            {
                return State.Failure;
            }
        }
        if(castFinished == false){
            return State.Running;
        }
        else{
            
            // if((ability.getCastTime() > 0.0)&&(ability.castWhileMoving == false)){
            //     //Debug.Log("AttkRelWP: agent isStopped to false");
            //     context.agent.isStopped = false;
            // }
                //Debug.Log("Attck: isStopped " + context.agent.isStopped.ToString());
                return State.Success;
        }
    }
    void checkCastedAbility(int _id){
        if(_id == ability.id){
            castFinished = true;
        }
    }
        
}
