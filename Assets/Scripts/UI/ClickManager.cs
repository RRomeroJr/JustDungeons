using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;
/*
    Richie: 
        This should handle clicking of characters/ objects in the world
    as well as UI elements inorder to interact with them/ get them
    as your current target
*/
public class ClickManager : NetworkBehaviour
{
    /*
        Note:
                RR: This relies on a GameObejct existing with the Unity tag "MainCamera" and there
                    being only one of them
    */



    public Text targetName;
    public Slider targetHealthBar;
    public Image targetHealthBarFill;
    public Actor playerActor;


    void Update()
    {   
        if(isLocalPlayer){
            if (Input.GetMouseButtonDown(0)) {
                /*
                        Implement Clicking UI frames to get target somehow 
                */

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                Debug.Log("mousePos "+ mousePos.ToString());

                if (hit.collider != null) {
                    Debug.Log("Clicked something");

                    // set controller's target w/ actor hit by raycast
                    playerActor.target = hit.collider.gameObject.GetComponent<Actor>();
                    updateTargetToClients(hit.collider.gameObject);
                    
                }else{
                    Debug.Log("Clicked nothing");
                }
            }
        }
    }
    [ClientRpc]
    void updateTargetToClients(GameObject target_GmObj){
        playerActor.target = target_GmObj.GetComponent<Actor>();
    }
}
