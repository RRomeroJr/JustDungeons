using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
    Handler for all UI HUD elements
*/
public class UIManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject castBarPrefab;
    /*
        Might be good to make these a list.
        that way can just have a function which
        up dates them all by stepping through the list
        then calling setUpFrame() on the them
    */
    public UnitFrame targetFrame;
    public List<UnitFrame> frames = new List<UnitFrame>();
    /*public UnitFrame partyFrame;
    public UnitFrame partyFrame1;
    public UnitFrame partyFrame2;
    public UnitFrame partyFrame3;*/
    public static Actor playerActor;
    public static Controller playerController;
    public GameObject cameraPrefab;
    public static GameObject nameplatePrefab;
    public static GameObject damageTextPrefab;
    
    void Awake(){
        nameplatePrefab = Resources.Load("Nameplate") as GameObject;
        damageTextPrefab = Resources.Load("DamageText") as GameObject;
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

    }

    // Update the max health and current health of all ally frames
    public void UpdateAllyFrames()
    {
        foreach (UnitFrame frame in frames)
        {
            if (frame.actor != null)
            {
                frame.healthBar.maxValue = frame.actor.getMaxHealth();
                frame.healthBar.value = frame.actor.getHealth();
            }
        }
    }

    public void updateUnitFrame(UnitFrame unitFrame, Actor actor){
        if(actor != null){
            //setting Actor
            unitFrame.actor = actor;
            //  Getting name
            unitFrame.unitName.text = actor.getActorName();
            //  Getting health current and max
            unitFrame.healthBar.maxValue = actor.getMaxHealth();
            unitFrame.healthBar.value = actor.getHealth();
            //  Getting apropriate healthbar color from actor
            unitFrame.healthFill.color = actor.unitColor;
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

    void Update(){
        UpdateAllyFrames();
        /*updateUnitFrame(partyFrame, partyFrame.actor);
        updateUnitFrame(partyFrame1, partyFrame1.actor);
        updateUnitFrame(partyFrame2, partyFrame2.actor);
        updateUnitFrame(partyFrame3, partyFrame3.actor);*/
        if(playerActor != null){ 
            updateUnitFrame(targetFrame, playerActor.target);
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

}
