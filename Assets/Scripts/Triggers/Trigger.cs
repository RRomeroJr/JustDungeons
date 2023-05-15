using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : NetworkBehaviour
{
    [SyncVar]
    public bool triggered = false;
	public UnityEvent onActivate = new UnityEvent();
	public UnityEvent onDeactivate = new UnityEvent();
    public bool serverOnly = true;

    public void Update()
    {
        if(serverOnly && !isServer){
            return;
        }

        bool result = TriggerCheck();

        if(triggered == result)
        {
            return;
        }
        else
        {
            // Debug.Log("result was different");
        }
        if(!isServer)
        {
            // Debug.Log("Sending result to server");
            CmdSetTriggered(result);
            return;
        }

        //If you are the server
        triggered = result;
        TriggeredChanged();


    }


    public virtual bool TriggerCheck()
    {
        Debug.Log("Base Trigger TriggerCheck. Prob not intended. Nothing happens");

        return false;
    }
    [Command(requiresAuthority = false)]
    void CmdSetTriggered(bool _valFromClient)
    {
        // Debug.Log(gameObject.name + ": " + GetType() + ", CmdSetriggered-> " + _valFromClient);
        if(triggered == _valFromClient)
        {
            return;
        }
        triggered = _valFromClient;
        TriggeredChanged();
    }

   [Server]
    void Activate()
    {
        onActivate.Invoke();
        Debug.Log(gameObject.name + ": " + GetType() + ", Activated");

        
    }
    [Server]
    void Deactivate()
    {
        onDeactivate.Invoke();
        Debug.Log(gameObject.name + ": " + GetType() + ", Deactivated");
        
    }
    [Server]
    void TriggeredChanged()
    {
        if(triggered)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }
}
