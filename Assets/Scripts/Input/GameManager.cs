using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UIManager uiManager;
    public GameObject player;
    //Assume that actor get you a COPY of the abililty that they want to cast
    // or.. I could copy it for them... and replace the ref with ref keyword
    public UnityEvent<Actor> OnMobDeath;
    public int dungeonScalingLevel = 0;
    public float dungeonHealthScaling = 0.1f;
    public float dungeonDamageScaling = 0.1f;
    public float timer = 600.0f;
    public uint mobCount = 0;
    public UnityEvent<Actor> OnActorEnterCombat = new UnityEvent<Actor>();
    public UnityEvent<Actor> OnActorLeaveCombat = new UnityEvent<Actor>();
    public UnityEvent AllPlayersLeaveCombat = new UnityEvent();
    public UnityEvent OnDungeonComplete = new UnityEvent();
    public const int maxDungeonLevel = 20;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);         
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // OnMobDeath.AddListener(RespawnDionysus);
        // OnMobDeath.AddListener(IncreaseMobCount);
        OnActorEnterCombat.AddListener(LogEnterCombat);
        OnActorLeaveCombat.AddListener(LogLevaveCombat);
        OnActorLeaveCombat.AddListener(CheckAllPlayersOutOfCombat);
        if(NetworkServer.active){
            NetworkServer.Spawn(gameObject);
            /* This doesn;t work bc this object exists before the server is started*/
        }
        else{
            
        } 
    }
    
    public void IncreaseMobCount(int _mobId)
    {
        mobCount += 1;
    }
    public void DecreaseMobCount()
    {
        mobCount -= 1;
    }

    void Update()
    {
    }
    void LogEnterCombat(Actor _eventIn){
        // Debug.Log(_eventIn.name + " has ENTERED combat");
    }
    void LogLevaveCombat(Actor _eventIn){
        // Debug.Log(_eventIn.name + " LEAVING combat");
    }

    public void logMobDeath(int _mobId){
        Debug.Log(MobData._inst.find(_mobId).ActorName+ " has died! spawning new one");
    }
    public void RespawnDionysus(Actor _a){
        if(NetworkServer.active == false){
            return;
        }
        Debug.Log("Respawning Dionysus after death in 7 secs");
        StartCoroutine(spawnInXSecs(7.0f, MobData._inst.find(1).gameObject));


    }

    IEnumerator spawnInXSecs(float time, GameObject prefab){
        while(time > 0){
            Debug.Log("Respawning in.." + time.ToString());
            if(time > 1.0f){
                yield return new WaitForSeconds(1.0f);
                time = time - 1.0f;
            }
            else{
                yield return new WaitForSeconds(time);
                break;
            }
            
            
        }
        NetworkServer.Spawn(Instantiate(prefab, Vector2.zero, Quaternion.identity ));
    }
    
    void CheckAllPlayersOutOfCombat(Actor _notUsed)
    {
        foreach(PlayerGame pg in CustomNetworkManager.singleton.GamePlayers)
        {
            if(pg.GetComponent<Actor>().IsInCombat())
            {
                return;
            }
        }

        AllPlayersLeaveCombat.Invoke();
    }
    /// <summary>
    /// Returns true if atleast one player in combat
    /// </summary>
    /// <returns></returns>
    public static bool PlayersInCombat()
    {
        foreach(PlayerGame pg in CustomNetworkManager.singleton.GamePlayers)
        {
            if(pg.GetComponent<Actor>().IsInCombat())
            {
                return true;
            }
        }

        return false;
    }
}
