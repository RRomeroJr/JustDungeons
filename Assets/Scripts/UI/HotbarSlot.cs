using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlot: MonoBehaviour, IDropHandler
{
    public KeyCode key;
    public Hotbutton hotButton;
    public GameObject KeyPressedObj;
    public GameObject GlowObj;
    public void OnDrop(PointerEventData eventData){
        GameObject dropped = eventData.pointerDrag;
        hotButton = dropped.GetComponent<Hotbutton>();
        hotButton.parentSlot = transform;
        hotButton.transform.position = transform.position;
    }
    public bool AddHotbutton(GameObject _hotbuttonGO)
    {
        /* 
            Really need code here to check if the slot is not already filled
        */


        hotButton = _hotbuttonGO.GetComponent<Hotbutton>();
        hotButton.parentSlot = transform;
        hotButton.transform.position = transform.position;
        hotButton.transform.SetParent(transform);
        hotButton.transform.SetAsFirstSibling();
        return true;
    }
    void Update(){
        if(Input.GetKeyDown(key)){
            Debug.Log("HotbarSlot: " + name + "was activated by the \"" + key + "\" key");
        }
    }
    public void ActivateSlot(){
        KeyPressedObj.SetActive(true);
        if(transform.childCount > 2){
            
            try{
               UIManager.playerActor.castAbility3(transform.GetChild(0).GetComponent<Hotbutton>().ability);
            }
            catch{
                Debug.LogError("Activing slot " + name + " failed for some reason. Maybe there is no ability hooked up?");
            }
            
        }
        else{
            Debug.LogError(name + " Slot is empty!");
        }
    }
    public void DeactivateSlot(){
        KeyPressedObj.SetActive(false);
    }
    public void ActivateGlow(){
        GlowObj.SetActive(true);
    }
    public void DeactivateGlow(){
        GlowObj.SetActive(false);
    }
}