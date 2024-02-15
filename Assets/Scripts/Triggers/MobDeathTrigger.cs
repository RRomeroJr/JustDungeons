using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MobDeathTrigger: Trigger
{
	
    public List<Actor> actorsTocheck;
    public bool waitToPopulate = false; 
    
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
            if(waitToPopulate){ // There was atleast 1 actor in the list so no longer waiting to populate
                waitToPopulate = false;
            }
            if(actorToCheck != null){
                if(actorToCheck.state == ActorState.Alive)
                {
                    atleastOneAlive = true;
                    break;
                }
            }
            
        }
       
        return !(atleastOneAlive || waitToPopulate);
    }

    public void PopulateActorList(List<Actor> _list)
    {
        foreach(Actor a in _list)
        {
            actorsTocheck.Add(a);
        }
    }
   
}