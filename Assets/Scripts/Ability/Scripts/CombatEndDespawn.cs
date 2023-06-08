using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CombatEndDespawn: NetworkBehaviour
{
	void Awake()
    {
        if(isServer){
            GameManager.instance.AllPlayersLeaveCombat.AddListener(DespawnObject);
        }

    }
    [Server]
    void DespawnObject()
    {
        Destroy(gameObject);
    }
}