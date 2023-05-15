using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickTrigger: Trigger
{
    
    
    void Awake()
    {
        
        // testEvent.AddListener(DemoTestHook);
    }
    
    // public override void Update()
    // {
    //     //then do stuff in here
        
    //         DemoTestCheck();
        
    // }

    public override bool TriggerCheck()
    {
        //Checking for.. this object clicked
        if(Input.GetMouseButtonDown(1))
        {
            // Debug.Log("Input heard and read");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            ClickTrigger _clickTrigger = null;
            try
            {
                _clickTrigger = hit.collider.GetComponent<ClickTrigger>();
                if(_clickTrigger == this)
                {
                    // Debug.Log("Returning flipped triggered");
                    return !triggered;
                }
            }
            catch
            {
            }

        }
        return triggered;
    }
    

   
}