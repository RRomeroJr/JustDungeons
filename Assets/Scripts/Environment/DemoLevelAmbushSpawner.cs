using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Mirror;
using System.Threading;

public class DemoLevelAmbushSpawner: MonoBehaviour
{
    [System.Serializable]
    public class PrefabAndPosition
    {
        public GameObject prefab;
        public Vector2 moveToPos;
    }
    // public GameObject mobPrefab;
    public List<PrefabAndPosition> list;
    public UnityEvent<List<Actor>> SpawnListOut = new UnityEvent<List<Actor>>(); 
    void Start(){
    }

    public void spawnMobs(){
        List<Actor> spawnList = new List<Actor>();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject goRef = Instantiate(list[i].prefab, transform.position, Quaternion.identity);
            goRef.AddComponent<GeneralMobPack>();
            spawnList.Add(goRef.GetComponent<Actor>());
            goRef.GetComponent<NavMeshAgent>().SetDestination(list[i].moveToPos);
            NetworkServer.Spawn(goRef);
        }
        
        SpawnListOut?.Invoke(spawnList);
    }
    void OnDrawGizmosSelected()
    {
        foreach(var thing in list)
        {
            Debug.DrawLine(transform.position, thing.moveToPos, Color.red);
        }
    }
}