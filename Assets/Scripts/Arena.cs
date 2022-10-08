using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Arena : NetworkBehaviour
{
    public bool killPlayerOnExit = false;
    public void OnTriggerExit2D(Collider2D other)
    {   if(killPlayerOnExit){
            other.gameObject.GetComponent<Actor>().setHealth(0);
        }
        
    }
    void OnValidate(){

    }
}
