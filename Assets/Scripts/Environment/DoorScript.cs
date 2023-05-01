using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DoorScript : MonoBehaviour
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
    public void OpenDoor(){
        GetComponent<Collider2D>().enabled = false;
        Color newColor = GetComponent<SpriteRenderer>().color;
        newColor.a = 0.33f;
        
        GetComponent<SpriteRenderer>().color= newColor;
    }

    // Move door towards closed position
    public void CloseDoor(){
        GetComponent<Collider2D>().enabled = true;
        Color newColor = GetComponent<SpriteRenderer>().color;
        newColor.a = 1f;
        GetComponent<SpriteRenderer>().color= newColor;
    }
}
