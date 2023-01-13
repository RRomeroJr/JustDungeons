using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hotbar: MonoBehaviour
{
    public List<HotbarSlot> slots = new List<HotbarSlot>();
     

    void Update(){
      if(Input.GetButtonDown("Hotbar1_1")){
        if(slots[0] == null){
            Debug.Log(name +  " slot " + 1 + " is empty!");
            return;
        }
        // Debug.Log(name +  " firing button " + 1);
        slots[0].ActivateSlot();
      }
      if(Input.GetButtonDown("Hotbar1_2")){
        if(slots[1] == null){
            Debug.Log(name +  " slot " + 2 + " is empty!");
            return;
        }
        // Debug.Log(name +  " firing button " + 2);
        slots[1].ActivateSlot();
      }
      if(Input.GetButtonDown("Hotbar1_3")){
        if(slots[2] == null){
            Debug.Log(name +  " slot " + 3+ " is empty!");
            return;
        }
        // Debug.Log(name +  " firing button " + 3);
        slots[2].ActivateSlot();
      }
      if(Input.GetButtonDown("Hotbar1_4")){
        if(slots[3] == null){
            Debug.Log(name +  " slot " + 4 + " is empty!");
            return;
        }
        // Debug.Log(name +  " firing button " + 4);
        slots[3].ActivateSlot();
      }
      if(Input.GetButtonDown("Hotbar1_5")){
        if(slots[4] == null){
            Debug.Log(name +  " slot " + 5 + " is empty!");
            return;
        }
        // Debug.Log(name +  " firing button " + 5);
        slots[4].ActivateSlot();
      }
      if(Input.GetButtonDown("Hotbar1_6")){
        if(slots[5] == null){
            Debug.Log(name +  " slot " + 6 + " is empty!");
            return;
        }
        // Debug.Log(name +  " firing button " + 6);
        slots[5].ActivateSlot();
      }
      if(Input.GetButtonDown("Hotbar1_7")){
        if(slots[6] == null){
            Debug.Log(name +  " slot " + 7 + " is empty!");
            return;
        }
        // Debug.Log(name +  " firing button " + 7);
        slots[6].ActivateSlot();
      }
      if(Input.GetButtonDown("Hotbar1_8")){
        if(slots[7] == null){
            Debug.Log(name +  " slot " + 8 + " is empty!");
            return;
        }
        // Debug.Log(name +  " firing button " + 8);
        slots[7].ActivateSlot();
      }
      if(Input.GetButtonUp("Hotbar1_1")){
        if(slots[0] == null){
            
            return;
        }
        // Debug.Log(name +  " deactivating button " + 1);
        slots[0].DeactivateSlot();
      }
      if(Input.GetButtonUp("Hotbar1_2")){
        if(slots[1] == null){
          
            return;
        }
        // Debug.Log(name +  " deactivating button " + 2);
        slots[1].DeactivateSlot();
      }
      if(Input.GetButtonUp("Hotbar1_3")){
        if(slots[2] == null){
           
            return;
        }
        // Debug.Log(name +  " deactivating button " + 3);
        slots[2].DeactivateSlot();
      }
      if(Input.GetButtonUp("Hotbar1_4")){
        if(slots[3] == null){
       
            return;
        }
        // Debug.Log(name +  " deactivating button " + 4);
        slots[3].DeactivateSlot();
      }
      if(Input.GetButtonUp("Hotbar1_5")){
        if(slots[4] == null){
         
            return;
        }
        // Debug.Log(name +  " deactivating button " + 5);
        slots[4].DeactivateSlot();
      }
      if(Input.GetButtonUp("Hotbar1_6")){
        if(slots[5] == null){
        
            return;
        }
        // Debug.Log(name +  " deactivating button " + 6);
        slots[5].DeactivateSlot();
      }
      if(Input.GetButtonUp("Hotbar1_7")){
        if(slots[6] == null){
       
            return;
        }
        // Debug.Log(name +  " deactivating button " + 7);
        slots[6].DeactivateSlot();
      }
      if(Input.GetButtonUp("Hotbar1_8")){
        if(slots[7] == null){
           
            return;
        }
        // Debug.Log(name +  " deactivating button " + 8);
        slots[7].DeactivateSlot();
      }
    }
}