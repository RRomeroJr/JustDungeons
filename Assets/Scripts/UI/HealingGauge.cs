using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingGauge : MonoBehaviour
{
    public Slider nextChargeBar;
    public Transform charges;
    public Transform chargesSp;
	
    void Start(){
        // nextChargeBar = GetComponent<Slider>();
        // charges = transform.Find("Charges");
        // charges = transform.Find("ChargesSp");
    }
    void Update(){
        UpdateHealingGauge();
    }
    void UpdateHealingGauge(){
        if(UIManager.Instance == null){ 
            return;
        }
        if(UIManager.playerActor == null){ 
            return;
        }


        
        nextChargeBar.maxValue = UIManager.playerActor.getResource(1).tickMax;
        nextChargeBar.value = UIManager.playerActor.getResource(1).tickTime;
        UpdateHealingGaugeCharges();

    }
    void UpdateHealingGaugeCharges()
    {
    
        for (int i = 0; i < 3; i++)
        {
            bool setCharge = ((int)UIManager.playerActor.getResource(1).amount >= i + 1);
            if(charges.GetChild(i).gameObject.active != setCharge)
            {
                charges.GetChild(i).gameObject.active = setCharge;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            bool setSpecial = ((int)UIManager.playerActor.getResource(2).amount >= i + 1);
            if(chargesSp.GetChild(i).gameObject.active != setSpecial)
            {
                chargesSp.GetChild(i).gameObject.active = setSpecial;
            }
        }

        // bool setSpecial = UIManager.playerActor.getResource(2).amount >= 3.0f;
        // if(chargesSp.GetChild(0).gameObject.active != setSpecial )
        // {
        //     chargesSp.GetChild(0).gameObject.active = setSpecial;
        
        // }
    }
}