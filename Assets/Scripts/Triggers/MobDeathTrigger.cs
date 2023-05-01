using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MobDeathTrigger: Trigger
{
	
    public List<Actor> actorsTocheck;
    
    void Awake()
    {
        // testEvent.AddListener(DemoTestHook);
    }
    
    // public override void Update()
    // {
    //     //then do stuff in here
        
    //         DemoTestCheck();
        
    // }

    public override bool TriggerCheck()
    {
        //Checking for.. atleast one alive

        bool atleastOneAlive = false;

        foreach(Actor actorToCheck in actorsTocheck)
        {
            if(actorToCheck != null){

                atleastOneAlive = actorToCheck.state == ActorState.Alive;
            }

            if(atleastOneAlive)
                break;
            
        }

        return !atleastOneAlive;
    }
   
}