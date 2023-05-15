using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;


public class DoorScript : NetworkBehaviour
{
    
    // public UnityEvent oneTimeTriggerEvent;

    // Start is called before the first frame update
    void Start()
    {
        // oneTimeTriggerEvent.AddListener(OpenDoor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Move door towards open position 
    void OpenDoor(){
        GetComponent<Collider2D>().enabled = false;
        Color newColor = GetComponent<SpriteRenderer>().color;
        newColor.a = 0.33f;
        
        GetComponent<SpriteRenderer>().color= newColor;
    }
    [ClientRpc]
    public void RpcOpenDoor()
    {
        OpenDoor();
    }

    // Move door towards closed position
    void CloseDoor(){
        GetComponent<Collider2D>().enabled = true;
        Color newColor = GetComponent<SpriteRenderer>().color;
        newColor.a = 1f;
        GetComponent<SpriteRenderer>().color= newColor;
    }
    [ClientRpc]
    public void RpcCloseDoor()
    {
        CloseDoor();
    }
}
