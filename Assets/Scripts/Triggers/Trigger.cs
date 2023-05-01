using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public bool triggered = false;
	public UnityEvent onActivate = new UnityEvent();
	public UnityEvent onDeactivate = new UnityEvent();

    public void Update()
    {
        bool result = TriggerCheck();

        if(triggered == result)
        {
            return;
        }

        triggered = result;
        if(result)
        {
            onActivate.Invoke();
        }
        else
        {
            onDeactivate.Invoke();
        }


    }


    public virtual bool TriggerCheck()
    {
        Debug.Log("Base Trigger TriggerCheck. Prob not intended. Nothing happens");

        return false;
    }
}
