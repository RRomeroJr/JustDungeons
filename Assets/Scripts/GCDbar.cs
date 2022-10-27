using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GCDbar: MonoBehaviour
{
    public Slider bar;
	
    void Start(){
        bar = GetComponent<Slider>();
    }
    void Update(){
        bar.maxValue = Controller.gcdBase;
        bar.value = Controller.gcdBase - UIManager.playerController.globalCooldown;
    }
}