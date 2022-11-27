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
        StartCoroutine(tryGetGCD());
    }
    IEnumerator tryGetGCD(){
        bool stay = true;
        while(stay){
            if(UIManager.playerController != null){
                bar.value = Controller.gcdBase - UIManager.playerController.globalCooldown;
                stay = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}