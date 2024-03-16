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
        castStarted = false;
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
        if(!castStarted){

            Transform _target = ContextualTargetToGmObj(contextualTarget).transformSafe();
            
            if (tryOnce)
            {
                return BoolToState(context.actor.castAbility3(ability, _target));
            }
            else
            {
                castStarted = context.actor.castAbility3(ability, _target);
            }
        }
      
        if(!castFinished){
            return State.Running;
        }
        else
        {
            return State.Success;
        }
    }
    void checkCastedAbility(int _id){
        if(_id == ability.id){
            castFinished = true;
        }
    }
        
}
