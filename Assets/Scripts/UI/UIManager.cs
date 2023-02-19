using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/*
    Handler for all UI HUD elements
*/
public class UIManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject castBarPrefab;
    public GameObject hotbuttonPrefab;
    public UnitFrame targetFrame;
    public List<UnitFrame> frames = new List<UnitFrame>();
    public static Actor playerActor;
    public static Controller playerController;
    public GameObject cameraPrefab;
    public static GameObject nameplatePrefab;
    public static GameObject damageTextPrefab;
    public Color defaultColor;
    public static UnityEvent<int> removeCooldownEvent = new UnityEvent<int>();
    public List<Ability_V2> glowList = new List<Ability_V2>();
    public UnityEvent<Ability_V2> StartAbiltyGlow = new UnityEvent<Ability_V2>();
    public UnityEvent<Ability_V2> EndAbilityGlow = new UnityEvent<Ability_V2>();
    public UnityEvent glowChecks;
   
    public static UIManager Instance{ get; private set;}
    void Awake(){
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        nameplatePrefab = Resources.Load("Nameplate") as GameObject;
        damageTextPrefab = Resources.Load("DamageText") as GameObject;
        //hotbuttonPrefab = Resources.Load("Hotbutton 1") as GameObject;
        
    } 
    // Start is called before the first frame update
    void Start()
    {
        if(cameraPrefab == null){
            Debug.LogError("Please add a camera prefab to UIManager.cameraPrefab");
        }
        /* Not sure if unit frames should have refences to actors
           like this. Later I might change this so the UIManager
         v   has refs to unitframes and correponding actors      v*/
        /*updateUnitFrame(partyFrame, partyFrame.actor);
        updateUnitFrame(partyFrame1, partyFrame1.actor);
        updateUnitFrame(partyFrame2, partyFrame2.actor);
        updateUnitFrame(partyFrame3, partyFrame3.actor);*/
        //setUpUnitFrame(partyFrame, partyFrame.actor);

        CustomNetworkManager.singleton.GamePlayers.CollectionChanged += AddPlayerFrame;
    }

    void AddPlayerFrame(object sender, NotifyCollectionChangedEventArgs e)
    {
        int i = 0;
        foreach (var player in CustomNetworkManager.singleton.GamePlayers)
        {
            updateUnitFrame(frames[i], player.GetComponent<Actor>());
            i++;
        }
    }

    // Update the max health and current health of all ally frames
    public void UpdateAllyFrames()
    {
        foreach (UnitFrame frame in frames)
        {
            if (frame.actor != null)
            {
                frame.healthBar.maxValue = frame.actor.getMaxHealth();
                frame.healthBar.value = frame.actor.Health;
            }
        }
    }

    public void updateUnitFrame(UnitFrame unitFrame, Actor actor){
        
        if(unitFrame.actor != actor ){
            
            unitFrame.actor = actor;
        }
        
        if(unitFrame.actor != null){        
            
            //  Getting name
            unitFrame.unitName.text = unitFrame.actor.getActorName();
            //  Getting health current and max
            unitFrame.healthBar.maxValue = actor.getMaxHealth();
            unitFrame.healthBar.value = actor.Health;
            //  Getting apropriate healthbar color from actor
            unitFrame.healthFill.color = unitFrame.actor.unitColor;
        }
        else{
            unitFrame.unitName.text = "No actor";
            unitFrame.healthBar.maxValue = 1.0f;
            unitFrame.healthBar.value = 1.0f;
            unitFrame.healthFill.color = defaultColor;
        }

    }
    
    // public void setUpUnitFrame(PointerEventData data){
    //     Debug.Log("Test");
    // }
    // public void setUpUnitFrame(UnitFrame unitFrame, Actor actor){   
    //     EventTrigger trigger = unitFrame.GetComponent<EventTrigger>();
    //     EventTrigger.Entry entry = trigger.triggers.Find(ent => ent.eventID == EventTriggerType.PointerClick);
    //     if(entry != null){
    //         Debug.LogError("UnitFrame has no PointerClick entry trigger.triggers.Count: " + trigger.triggers.Count.ToString());
    //     }
    //     entry.callback.AddListener((methodIWant) => { setTarget(); });

    // }
    void UpdateTargetFrame(){
        if(playerActor == null){ 
            return;
        }
        
        if(playerActor.target == null){
            if(targetFrame.gameObject.active){
                targetFrame.gameObject.SetActive(false);
            }
        }
        else{
            if(!targetFrame.gameObject.active){
                Debug.Log("Setting target to active");
                targetFrame.gameObject.SetActive(true);
                
            }
            
            //Debug.Log("Updating Targetframe");
            updateUnitFrame(targetFrame, playerActor.target);
        }

    }

    void Update(){
        UpdateAllyFrames();
        UpdateTargetFrame();
        CheckClassGlows();
        
    }
    void UpdateGlows(){
        int current = 0;
        foreach(Ability_V2 _a in glowList){
            bool alreadyChecked = glowList.IndexOf(_a) != current;
            if(!alreadyChecked){

            }
        }
    }
    void CheckClassGlows(){
        if(playerActor.combatClass == null){
            return;
        }
        int current = 0;
        foreach(GlowCheck _gc in playerActor.combatClass.classGlowChecks)
        {
            // Debug.Log("invoking class GlowCheck");
            _gc.glowChecks.Invoke();
        }
    }
    
    public void setTarget(){
        Debug.Log("Test");
    }
    public void setTargetGmObj(GameObject input){
        UnitFrame uF = input.GetComponent<UnitFrame>();
        //Debug.Log(uF != null ? "uF found" : "uF NULL");
        //Debug.Log(temp != null ? "temp actor found" : "temp actor NULL");
        //Debug.Log(playerActor != null ? "playerActor assigned" : "playerActor NULL");
        //playerActor.target = (temp != null ? temp : null);
        playerActor.target = (uF != null ? uF.actor : null);
    }
    public void SpawnHotbuttons(CombatClass _combatClass){
        
        if(hotbuttonPrefab == null){
            Debug.LogError("No Hotbutton Prefab in UIManager. Can't spawn ability hotbuttons");
        }
        Hotbutton hotbuttonInst = null;
        foreach(Ability_V2 a in _combatClass.abilityList){
            hotbuttonInst = Instantiate(hotbuttonPrefab, new Vector2(0.0f, 0.0f) + (Vector2)canvas.transform.position, Quaternion.identity, canvas.transform).GetComponent<Hotbutton>();
            hotbuttonInst.ability = a;
            hotbuttonInst.canvas = canvas.GetComponent<Canvas>();
            hotbuttonInst.SetUp();
        }
    }
    public void MakeGlow(Ability_V2 _ability){
        if(glowList.Contains(_ability) == false){
            glowList.Add(_ability);
            Debug.Log("UIManager: Making "+ _ability.name + " glow");
        }
    }
    


}
