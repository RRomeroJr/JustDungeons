using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ArenaSpawner: NetworkBehaviour
{
	public Arena ArenaPrefab;
    void Awake(){
        GameManager.instance.OnActorEnterCombat.AddListener(SpawnArena);
        
    }
    void Start(){
        
    }

    void SpawnArena(Actor _eventIn){
        if(NetworkServer.active == false){
            Debug.Log("Client ignoring spawn arena component");
            return;
        }



        if(_eventIn.gameObject == gameObject){
            EnemyController enemyController = GetComponent<EnemyController>();
            Actor actor = GetComponent<Actor>();

            if(enemyController == null)
            {
                Debug.LogError(GetType().ToString() + ": Could not spawn Arena. EnemyController not found");
                return;
            }

            if(actor == null)
            {
                Debug.LogError(GetType().ToString() + ": Could not spawn Arena. No Actor not found");
                return;
            }

            Arena arenaRef = Instantiate(ArenaPrefab, transform.position, Quaternion.identity);
            enemyController.arenaObject = arenaRef;
            arenaRef.mobList.Add(GetComponent<Actor>());
            NetworkServer.Spawn(arenaRef.gameObject);
            Destroy(this);
        }
    }
}