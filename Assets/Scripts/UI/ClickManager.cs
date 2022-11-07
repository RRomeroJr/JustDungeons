using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;
using System;
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
    public LayerMask mask;

    void Start()
    {
        FindObjectOfType<UIRaycaster>().UIFrameClicked += OnUIFrameClicked;
    }

    void Update()
    {   
        if(isLocalPlayer){
            if (Input.GetMouseButtonDown(0)) {
                /*
                        Implement Clicking UI frames to get target somehow 
                */

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, mask);
                //Debug.Log("mousePos "+ mousePos.ToString());

                if (hit.collider != null) {
                    
                    Debug.Log("Clicked something: " + hit.collider.gameObject.name);

                    // set controller's target w/ actor hit by raycast

                    if(isServer){
                        playerActor.rpcSetTarget(hit.collider.gameObject.GetComponent<Actor>());
                    }else{
                        playerActor.cmdReqSetTarget(hit.collider.gameObject.GetComponent<Actor>());
                    }
                }else{
                    Debug.Log("Nothing clicked");
                }
            }
        }
    }

    private void OnUIFrameClicked(object source, UIFrameClickedEventArgs e)
    {
        if (isServer)
        {
            playerActor.rpcSetTarget(e.Frame.actor);
        }
        else
        {
            playerActor.cmdReqSetTarget(e.Frame.actor);
        }
    }

    [ClientRpc]
    void updateTargetToClients(Actor target){
        playerActor.target = target;
    }
    [Command]
    void reqTargetUpdate(Actor _actor){ //in future this should be some sort of act id or something
        updateTargetToClients(_actor);
    }
}
