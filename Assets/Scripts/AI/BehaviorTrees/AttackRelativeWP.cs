using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackRelativeWP : ActionNode
{   public Ability_V2 ability;
    public Vector2 relativePoint;
    public bool castStarted;
    public bool castFinished;
    public bool randomRange;
    public float plusMinusX;
    public float plusMinusY;
    protected override void OnStart()
    {
        //castStarted = false;
        castFinished = false;
        context.actor.onAbilityCastHooks.AddListener(checkCastedAbility);
        // MirrorTestTools._inst.ClientDebugLog("AttackRelativeWP OnStart()");
        
    }

    protected override void OnStop()
    {
        context.actor.onAbilityCastHooks.RemoveListener(checkCastedAbility);
    }

    protected override State OnUpdate()
    {   
        if(context.actor.IsCasting == false){
            
            if(ability.getCastTime() > 0.0){
                context.agent.isStopped = true;
                
            }
            //Debug.Log(context.controller.target.GetComponent<Actor>().getActorName());
            
            if(randomRange){
                Vector2 temp = new Vector2(Random.Range(-plusMinusX, plusMinusX), Random.Range(-plusMinusY, plusMinusY));
                context.actor.castRelativeToGmObj(ability, (context.gameObject.GetComponent<Controller>() as EnemyController).arenaObject.gameObject, relativePoint + temp);
            }
            else{
                context.actor.castRelativeToGmObj(ability, (context.gameObject.GetComponent<Controller>() as EnemyController).arenaObject.gameObject, relativePoint);
            }
            //castStarted = true;
        }
        if(castFinished == false){
            return State.Running;
        }
        else{
            
            if(context.agent.isStopped){
               // Debug.Log("AttkRelWP: agent isStopped to false");
                context.agent.isStopped = false;
            }
                //Debug.Log("Attck: isStopped " + context.agent.isStopped.ToString());
                return State.Success;
        }
        
    }
    void checkCastedAbility(int _id){
        if(_id == ability.id){
            // MirrorTestTools._inst.ClientDebugLog("cast fired MATCH FOUND");
            castFinished = true;
        }
        else{
            //MirrorTestTools._inst.RpcDebugLog("cast fired no match found");
        }
    }
}
