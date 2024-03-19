using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class TempDamageMeter : NetworkBehaviour
{
    // Use Uimanager to reference the player
    // interate over player's cooldowns
    // display them in the textmeshpro.SetText
    public TextMeshProUGUI tmpGUI;
    public List<MeterEntry> entryList;
    
    public float timePassed = 0.001f;
    public float updateMax = 1.0f;
    float updateTimer = 0.0f;
    public static TempDamageMeter instance;
    [SyncVar]
    /// <summary>
    /// True if atleast 1 player in combat
    /// </summary>
    public bool playerCombat = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);         
        }
        else
        {
            if(LevelManager.instance == this)
            {
                LevelManager.instance = null;
            }
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tmpGUI = GetComponent<TextMeshProUGUI>();
        entryList = new List<MeterEntry>();
        if(isServer)
        {
            GameManager.instance.OnActorEnterCombat.AddListener(OnActorEnterCombat);
            GameManager.instance.AllPlayersLeaveCombat.AddListener(AllPlayersLeaveCombat);
            playerCombat = GameManager.PlayersInCombat();
        }
        updateTimer = updateMax;
    }
    void OnActorEnterCombat(Actor _a)
    {
        if(_a.tag == "Player")
        {
            playerCombat = true;
        }
    }
    void AllPlayersLeaveCombat()
    {
        playerCombat = false;
        DisplayEntries();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCombat)
        {
            if(updateTimer >= updateMax)
            {
                DisplayEntries();
                updateTimer = 0.0f;
            }
            timePassed += Time.deltaTime;
            updateTimer += Time.deltaTime;
        }
    }

    public static void addToEntry(Actor _actor, int _amount){
        if(!instance || !instance.enabled)
        {
            return;
        }
        bool found = false;
        foreach(MeterEntry me in instance.entryList){
            if(me.actor == _actor){
                me.total += _amount;
                found = true;
            }
        }
        if(!found){
            MeterEntry meRef = new MeterEntry(_actor);
            meRef.total += _amount;
            instance.entryList.Add(meRef);
        }
        
    }

    void DisplayEntries()
    {
        string toDisplay = "";
        if(entryList.Count > 0){
            
            foreach(MeterEntry me in entryList){
                //Debug.Log("Displaying cooldowns");
                toDisplay = toDisplay + me.actor.ActorName+ "| " + ((int)((float)me.total / timePassed)).ToString() + "\n";
                tmpGUI.SetText(toDisplay);
            }
        }
        else{
            tmpGUI.SetText(toDisplay);
        }
    }

}
