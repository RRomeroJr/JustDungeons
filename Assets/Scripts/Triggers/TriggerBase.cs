using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TriggerBase : MonoBehaviour
{
    public bool isActive = false;
   // [SerializeField] private UnityEvent simpleTigger;
    //[SerializeField] private UnityEvent simpleTigger2;
    /*private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Player")){
            isActive = true;
            //simpleTigger.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {   
        if (other.CompareTag("Player")){
            isActive = false;
            //simpleTigger2.Invoke();
        }
    }*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
