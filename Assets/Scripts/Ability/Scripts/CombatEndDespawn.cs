using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CombatEndDespawn : NetworkBehaviour
{
	void Start()
    {
        if(isServer){
            // Debug.Log("AddListener");
            GameManager.instance.AllPlayersLeaveCombat.AddListener(DespawnObject);
        }
        // else
        // {
        //     Debug.Log("AddListener SKIPPED");
        // }


    }
    [Server]
    void DespawnObject()
    {
        // Debug.Log("DespawnObject");
        GameManager.instance.AllPlayersLeaveCombat.RemoveListener(DespawnObject);
        Destroy(gameObject);


    }
}