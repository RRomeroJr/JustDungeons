using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CircleArena : Arena
{
    public float arenaRadius;
    public float killZoneRadius;
    void Awake(){
        if(killZoneRadius != transform.localScale.x / 2){
            killZoneRadius = transform.localScale.x / 2;
        }
        if(arenaRadius != transform.GetChild(0).transform.localScale.x / 2){
            arenaRadius = (transform.localScale.x  * transform.GetChild(0).transform.localScale.x) / 2;
            
        }
    }
    void Start(){
        if(isServer){
                RpcSetSafeZone(transform.GetChild(0).transform.localScale);
            }
    }
    void OnValidate(){
        
            
        if(arenaRadius > killZoneRadius){
            killZoneRadius = arenaRadius;
        }
        if(!killPlayerOnExit){
            killZoneRadius = arenaRadius;
        }
        transform.localScale = new Vector3(1,1,0) * (2 * killZoneRadius);
        transform.GetChild(0).transform.localScale = new Vector3(1,1,0) * ( arenaRadius / killZoneRadius);
        //GetComponent<CircleCollider2D>().radius = arenaRadius;
        
        
    }
    
    [ClientRpc]
    void RpcSetSafeZone(Vector3 _hostScale){
        transform.GetChild(0).transform.localScale = _hostScale;
    }
}
