using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using System;

public abstract class LevelManager : NetworkBehaviour
{
    public static LevelManager instance;

    public float timer = 600.0f;
    public uint mobCount = 0;
    public int encounterCounter = 0;
    
    public virtual void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);         
        }
        else
        {
            if(LevelManager.instance == this)
            {
                LevelManager.instance = null;
            }
            Destroy(this);
        }
    }

    public virtual void Start()
    {
   
    }
    public void IncreaseEncounterCounter()
    {
        encounterCounter++;
    }
    public void DecreasesEncounterCounter()
    {
        encounterCounter--;
    }
    public void IncreaseEncounterCounter(object sender,EventArgs args)
    {
        encounterCounter++;
    }
    public void DecreasesEncounterCounter(object sender, EventArgs args)
    {
        encounterCounter--;
    }
    public abstract void CheckIncreaseMobCount(Actor _actorThatDied);
    public abstract void CheckLevelComplete();
    public virtual void CheckLevelCompleteHelper(object sender, EventArgs args)
    {
        CheckLevelComplete();
    }
    public virtual void CheckLevelCompleteHelper(Actor _a)
    {
        CheckLevelComplete();
    }
    
    
}
