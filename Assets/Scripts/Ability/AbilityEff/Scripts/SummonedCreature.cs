using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using System;

public class SummonedCreature : NetworkBehaviour
{
    public Actor summoner;

    void Awake()
    {
    }
    void Start()
    {
        if(summoner == null){
            Debug.LogError(gameObject.name + ": No summoned creature with no summoner");
        }
        Debug.Log(GetType() + " start");
        summoner.OnEnterCombat.AddListener(OnSummonerEnterCombat);
        foreach(Actor summonerAttacker in summoner.attackerList)
        {
            GetComponent<Actor>().CheckStartCombatWith(summonerAttacker);
        }

    }

    void OnSummonerEnterCombat()
    {
        foreach(Actor summonerAttacker in summoner.attackerList)
        {
            GetComponent<Actor>().CheckStartCombatWith(summonerAttacker);
        }
    }
    void OnSummonerLeaveCombat()
    {
        /*
            Probably need a method here to end combat with things if the summoner is dead but w/e
        */
    }
    void OnDestroy()
    {
        summoner?.OnEnterCombat?.RemoveListener(OnSummonerEnterCombat);
    }
        
}