using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ArenaSpawner: NetworkBehaviour
{
	public Arena ArenaPrefab;
    protected void Awake(){
        GameManager.instance.OnActorEnterCombat.AddListener(SpawnOrWait);
        
    }
    protected void Start(){
        
    }
    protected void SpawnOrWait(Actor _eventIn){
        if(NetworkServer.active == false){
            Debug.Log("Client ignoring spawn arena component");
            return;
        }
        
        if(GetComponent<Multiboss>() != null)
        {
            if(ArenaPrefab == null)
            {
                StartCoroutine(WaitForPartnerSpawnArena());
                return;
            }
        }

        SpawnArena(_eventIn);
    }

    protected void SpawnArena(Actor _eventIn){
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

            Arena arenaRef = CreateArenaInstance();
            // Arena arenaRef = Instantiate(ArenaPrefab, transform.position, Quaternion.identity);
            enemyController.arenaObject = arenaRef;
            arenaRef.mobList.Add(GetComponent<Actor>());
            NetworkServer.Spawn(arenaRef.gameObject);
            Destroy(this);
        }
    }
    protected virtual Arena CreateArenaInstance() // Made for easy override
    {
        Arena arenaRef = Instantiate(ArenaPrefab, transform.position, Quaternion.identity);
        return arenaRef;

    }
    protected IEnumerator WaitForPartnerSpawnArena(){
        EnemyController enemyController = GetComponent<EnemyController>();
        Multiboss mbComp = GetComponent<Multiboss>();
        while(enemyController.arenaObject == null)
        {
            foreach(Actor partner in mbComp.partners)
            {
                Arena partnerArena = partner.GetComponent<EnemyController>().arenaObject;
                if(partnerArena != null)
                {
                    enemyController.arenaObject = partnerArena;
                }
                if(partnerArena != null)
                {
                    break;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
        Debug.Log(gameObject.name + ": Partner areana object found destroying ArenaSpawner Comp" );
        Destroy(this);
    }
}