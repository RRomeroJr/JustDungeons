using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Arena : NetworkBehaviour
{
    public bool killPlayerOnExit = false;
    public List<Actor> mobList;
    public bool destroyIfListEmpty = true;
    public void OnTriggerExit2D(Collider2D other)
    {   if(killPlayerOnExit){
            other.gameObject.GetComponent<Actor>().setHealth(0);
        }
        
    }
    void Update(){
        if(isServer){
            if(destroyIfListEmpty){
                if(mobList.Count <= 0){
                    Destroy(gameObject);
                }
            }
        }
        
        
    }
    void FixedUpdate(){
        for (int i = 0; i < mobList.Count; i++)
        {
            if(mobList[i] == null){
                mobList.RemoveAt(i);
            }
        }
    }   
    
    void OnValidate(){

    }
    public void Start(){

    }
}
