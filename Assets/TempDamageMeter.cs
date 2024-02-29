using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempDamageMeter : MonoBehaviour
{
    // Use Uimanager to reference the player
    // interate over player's cooldowns
    // display them in the textmeshpro.SetText
    public TextMeshProUGUI tmpGUI;
    public List<MeterEntry> entryList;
    public float timePassed = 0.01f;
    public static TempDamageMeter instance;

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
    }

    // Update is called once per frame
    void Update()
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
        timePassed += Time.deltaTime;
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
}
