using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class attack1stOffCD : ActionNode
{
    private Ability_V2 toCast = null;
    bool castStarted;
    bool castFinished;
    protected override void OnStart()
    {
        //Debug.Log("attack1stOffCD START");
        //castStarted = false;
        castFinished = false;
        
        if(context.controller.abilities.Count > 0){
                foreach (Ability_V2 a in context.controller.abilities){
                    //Debug.Log("checking " + a.getName());
                    if(context.actor.checkOnCooldown(a) == false){
                        
                        toCast = a;
                        break;
                    }else{
                        //Debug.Log(a.getName() + " was on cd");
                    }
                    
                }
            }
        else{
            Debug.LogError(context.actor.getActorName() + " has no abilities!");
        }
        
        if(toCast == null){
            Debug.LogError(context.actor.getActorName() + "Couldn't find an ability to cast");
            castFinished = true;
        }
        else{
            //Debug.Log("attack1stOffCD => " + toCast.getName());
            context.actor.onAbilityCastHooks.AddListener(checkCastedAbility);
        }
        
    }

    protected override void OnStop()
    {
        context.actor.onAbilityCastHooks.RemoveListener(checkCastedAbility);
    }

    protected override State OnUpdate()
    {
        if(toCast == null){
            
            return State.Success;
        }
        
        if(context.actor.IsCasting == false){
            
            if(toCast.getCastTime() > 0.0){
                context.agent.isStopped = true;
                
            }
            //Debug.Log(context.controller.target.GetComponent<Actor>().getActorName());
            
            context.actor.castAbility3(toCast, context.controller.target.GetComponent<Actor>());
            //castStarted = true;
        }
        if(castFinished == false){
            return State.Running;
        }
        else{
            
            if(context.agent.isStopped){
                //Debug.Log("AttkRelWP: agent isStopped to false");
                context.agent.isStopped = false;
            }
                //Debug.Log("Attck: isStopped " + context.agent.isStopped.ToString());
                return State.Success;
        }
    }
    void checkCastedAbility(int _id){
        if(_id == toCast.id){
            castFinished = true;
        }
    }
        
}
