using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public UnitFrame partyFrame;
    public UnitFrame partyFrame1;
    public UnitFrame partyFrame2;
    public UnitFrame partyFrame3;
    public Actor playerActor;
    
    // Start is called before the first frame update
    void Start()
    {
        /* Not sure if unit frames should have refences to actors
           like this. Later I might change this so the UIManager
         v   has refs to unitframes and correponding actors      v*/
        setUpUnitFrame(partyFrame, partyFrame.actor);
        setUpUnitFrame(partyFrame1, partyFrame1.actor);
        setUpUnitFrame(partyFrame2, partyFrame2.actor);
        setUpUnitFrame(partyFrame3, partyFrame3.actor);

    }
    public void setUpUnitFrame(UnitFrame unitFrame, Actor actor){
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
    
    void Update(){
        setUpUnitFrame(partyFrame, partyFrame.actor);
        setUpUnitFrame(partyFrame1, partyFrame1.actor);
        setUpUnitFrame(partyFrame2, partyFrame2.actor);
        setUpUnitFrame(partyFrame3, partyFrame3.actor);
        if(playerActor.target != null){ // do this for all frames?
            setUpUnitFrame(targetFrame, playerActor.target);
        }
    }

}
