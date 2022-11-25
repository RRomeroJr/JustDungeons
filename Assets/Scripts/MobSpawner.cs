using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MobSpawner: MonoBehaviour
{
    public GameObject mobPrefab;
    void Start(){
        StartCoroutine(tempSpawnTestMob(mobPrefab));
        
    }
	IEnumerator tempSpawnTestMob(GameObject prefab){
        while(true){
            yield return new WaitForSeconds(15.0f);
            NetworkServer.Spawn(Instantiate(prefab, transform.position, Quaternion.identity));

        }
    }       
}