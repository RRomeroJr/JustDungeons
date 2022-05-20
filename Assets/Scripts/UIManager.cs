using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UnitFrame partyFrame;
    public UnitFrame partyFrame1;
    public UnitFrame partyFrame2;
    public UnitFrame partyFrame3;
    
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
    void setUpUnitFrame(UnitFrame unitFrame, Actor actor){
        //  Getting name
        unitFrame.unitName.text = actor.name;
        //  Getting health current and max
        unitFrame.healthBar.maxValue = actor.maxHealth;
        unitFrame.healthBar.value = actor.health;
        //  Getting apropriate healthbar color from actor
        unitFrame.healthFill.color = actor.unitColor;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
