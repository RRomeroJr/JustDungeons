using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AboveHealthPercent : DecoratorNode
{
    public float percentage;
    public bool mustComplete =false;
    bool triggered = true;
    protected override void OnStart()
    {
        triggered = false;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {   // if you are below the health %
        if(context.gameObject.GetComponent<Actor>().getHealthPercent() < percentage){
            // Don't cast
            // 
            //
            if((triggered)&&(mustComplete)){
                
                switch (child.Update()) { 
                    // if You are below the healthj percent AND
                    // everything below this node MUST FINISH
                    // wait unitl you get EITHER d Seccess or Failure
                    case State.Running:
                        Debug.Log("AHP recieved a running");
                        break;
                    case State.Failure:
                        Debug.Log("breaking out of mustComplete due to failure");
                        return State.Failure;
                    
                    
                    case State.Success:
                        Debug.Log("AboveHealthPercent success");
                        return State.Failure;
                        
                        
                }
                return State.Running;
            }
            else{
                return State.Failure;
            }
        }
        else{ //If you are above the health %
            child.Update(); // then cast something
            
            return State.Success;
        }
    }
}
