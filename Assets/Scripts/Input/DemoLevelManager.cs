using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using System.Linq.Expressions;
using Unity.VisualScripting;

public class DemoLevelManager : LevelManager
{
    public Actor Dionysius;
    public Actor ChariotMan;
    public Actor SaytrMelee;
    public Actor SaytrCaster;

    public override void Start()
    {
        base.Start();
        try
        {
            GameManager.instance.OnMobDeath.AddListener(OnMobDeath);
            Debug.LogFormat("{0} started successfully", GetType());
        }
        catch (NullReferenceException nre)
        {
            Debug.LogErrorFormat("{0} could not start properly:\n{1}", GetType(), nre.ToString());
        }
    }
    void OnMobDeath(Actor _a)
    {
        Debug.Log("OnMobDeath");
        CheckIncreaseMobCount(_a);
        CheckLevelComplete();

    }
    public override void CheckIncreaseMobCount(Actor _actorThatDied)
    {
        Debug.Log("CheckIncreaseMobCount");
        if(_actorThatDied == Dionysius || _actorThatDied == ChariotMan ||
            _actorThatDied == SaytrMelee || _actorThatDied == SaytrCaster)
        {
            return;
        }

        mobCount += 1;

    }
    public override void CheckLevelComplete()
    {
        if(Dionysius.state == ActorState.Alive || ChariotMan.state == ActorState.Alive
         || SaytrMelee.state == ActorState.Alive || SaytrCaster.state == ActorState.Alive)
        {
            // Level not done do nothing
            return;
        }
        if(mobCount < 10)
        {
            return;
        }
        //something else happens
        Debug.Log("YOU WIN! This is where I would call the WebScript to update a score.");
        Debug.Log("YOU WIN! This is where I would call the WebScript to update a score.");
    }
    void OnDestroy()
    {
        GameManager.instance.OnMobDeath.RemoveListener(OnMobDeath);
    }
}
