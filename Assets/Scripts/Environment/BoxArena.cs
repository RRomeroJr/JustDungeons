using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class BoxArena : Arena
{
    
    void Awake(){

    }
    void Start(){
        base.Start();
        if(isServer){
                RpcSetSafeZone(transform.GetChild(0).transform.localScale);
            }
    }
    void OnValidate(){
        
        
        
    }
    
    
    [ClientRpc]
    void RpcSetSafeZone(Vector3 _hostScale){
        transform.GetChild(0).transform.localScale = _hostScale;
    }
    public override bool IsInSafezone(Actor _actor)
    {
        // Debug.Log("box safezxone");
        Vector2 distFromCenter = Safezone.transform.position - _actor.transform.position;
        if(Mathf.Abs(distFromCenter.x) > ((Safezone.transform.localScale.x * transform.localScale.x)/ 2.0f)){
            return false;
        }
        else if(Mathf.Abs(distFromCenter.y) > ((Safezone.transform.localScale.y * transform.localScale.y)/ 2.0f)){
            return false;
        }
        return true;
    }
}
