using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorScript : MonoBehaviour
{
    [Header("Door Settings")]
    public GameObject triggerObject;
    public float openHeight;
    public float speed;

    [Header("Debug Values")]
    [SerializeField] private bool isOpen;

    private Vector2 closedPosition;
    private Vector2 openPosition;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        closedPosition = gameObject.transform.position;
        openPosition = new Vector2(transform.position.x, transform.position.y + openHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerObject.GetComponent<TriggerBase>().isActive == true){
            OpenDoor();
        }
        else if(isOpen){
            CloseDoor();
        }
    }

    // Move door towards open position 
    public void OpenDoor(){
        transform.position = Vector2.MoveTowards(transform.position, openPosition, speed * Time.deltaTime);
        isOpen = true;
    }

    // Move door towards closed position
    public void CloseDoor(){
        transform.position = Vector2.MoveTowards(transform.position, closedPosition, speed * Time.deltaTime);
        if (transform.position.Equals(closedPosition))
        {
            isOpen = false;
        }
    }
}
