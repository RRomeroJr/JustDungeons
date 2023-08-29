using TheKiwiCoder;
using UnityEngine;

[Attack]
public class Attack : ActionNode
{
    public Ability_V2 ability;
    bool castStarted;
    bool castFinished;
    public HBCTools.ContextualTarget contextualTarget = HBCTools.ContextualTarget.Blackboard;
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

            Transform _target = ContextualTargetToGmObj(contextualTarget).transformSafe();
            
            if (tryOnce)
            {
                return BoolToState(context.actor.castAbility3(ability, _target));
            }
            else
            {
                context.actor.castAbility3(ability, _target);
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
