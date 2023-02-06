using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Attack : ActionNode
{
    public Ability_V2 ability;
    bool castStarted;
    bool castFinished;
    protected override void OnStart()
    {
        //castStarted = false;
        castFinished = false;
        context.actor.onAbilityCastHooks.AddListener(checkCastedAbility);
    }

    protected override void OnStop()
    {
        context.actor.onAbilityCastHooks.RemoveListener(checkCastedAbility);
    }

    protected override State OnUpdate()
    {
        
        if(context.actor.IsCasting == false){
            
            if((ability.getCastTime() > 0.0)&&(ability.castWhileMoving == false)){
                context.agent.isStopped = true;
                
            }
            //Debug.Log(context.controller.target.GetComponent<Actor>().getActorName());
            context.actor.castAbility3(ability, context.controller.target.GetComponent<Actor>());
            //castStarted = true;
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
