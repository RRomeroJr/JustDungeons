using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorScript : MonoBehaviour
{
    public GameObject triggerObject;
    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerObject.GetComponent<TriggerBase>().isActive == true){
            if(!isOpen){
                transform.position = new Vector2(transform.position.x, transform.position.y + 2.0f);
                isOpen = true;
            }
        }
        else if(isOpen){
            transform.position = new Vector2(transform.position.x, transform.position.y - 2.0f);
            isOpen = false;
        }
    }

    public void openDoor(){
        transform.position = new Vector2(transform.position.x, transform.position.y + 2.0f);
    }
    public void closeDoor(){
        transform.position = new Vector2(transform.position.x, transform.position.y - 2.0f);
    }
}
