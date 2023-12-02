using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using System.Threading;

public class MobSpawner: MonoBehaviour
{
    // public GameObject mobPrefab;
    public List<GameObject> mobPrefabList;
    public bool spawnOnStart = true;
    public UnityEvent<List<Actor>> SpawnListOut = new UnityEvent<List<Actor>>(); 
    void Start(){
        //StartCoroutine(tempSpawnTestMob(mobPrefab));
        if(NetworkServer.active && spawnOnStart){
            Debug.Log("spawning mobs on start");
            spawnMobs();

        }
    }
	IEnumerator tempSpawnTestMob(GameObject prefab){
        while(true){
            yield return new WaitForSeconds(15.0f);
            NetworkServer.Spawn(Instantiate(prefab, transform.position, Quaternion.identity));


        }
    }
    public void spawnMobs(){
        List<Actor> spawnList = new List<Actor>();
        for (int i = 0; i < mobPrefabList.Count; i++)
        {
            GameObject goRef = Instantiate(mobPrefabList[i], transform.position, Quaternion.identity);
            goRef.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f,0.01f));
            goRef.AddComponent<GeneralMobPack>();
            spawnList.Add(goRef.GetComponent<Actor>());
            NetworkServer.Spawn(goRef);
            //goRef.GetComponent<Controller>().moveOffOtherUnits();
        }
        
        SpawnListOut?.Invoke(spawnList);
    }
}