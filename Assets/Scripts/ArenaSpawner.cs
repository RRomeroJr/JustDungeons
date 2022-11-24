using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ArenaSpawner: NetworkBehaviour
{
	public Arena ArenaPrefab;
    void Awake(){
        if(NetworkServer.active){
            
            EnemyController enemyController = GetComponent<EnemyController>();
            Actor actor = GetComponent<Actor>();
            if(enemyController != null){
                if(actor != null){
                    
                    Arena arenaRef = Instantiate(ArenaPrefab, transform.position, Quaternion.identity);
                    enemyController.arenaObject = arenaRef;
                    arenaRef.mobList.Add(GetComponent<Actor>());
                    NetworkServer.Spawn(arenaRef.gameObject);
                }
                else{
                    Debug.LogError(GetType().ToString() + ": Could not spawn Arena. No Actor not found");
                }
                
            }
            else{
                Debug.LogError(GetType().ToString() + ": Could not spawn Arena. EnemyController not found");
            }
            
        }
        else{
            Debug.Log("Client ignoring spawn arena component");
        }
        
    }
    void Start(){
        
        Destroy(this);
    }
}