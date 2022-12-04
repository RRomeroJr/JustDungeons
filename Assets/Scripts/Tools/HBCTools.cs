using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

public static class HBCTools
{
    public static bool checkIfBehind(Actor actorToCheck, Actor target){
        // get a vector corressonding to the distance btwn the actorToCheck and taget
        Vector2 angleFromTarget = (Vector2)(actorToCheck.transform.position - target.transform.position);
        angleFromTarget.Normalize();

        //Vector2.Angle(a1, a2)
        float angleDifference = Vector2.Angle(target.GetComponent<Controller>().facingDirection, angleFromTarget);

        if(angleDifference > 135.0f){
            return true;
        }
        else{
            return false;
        }
        
    }
    public static bool checkFacing(Actor actorToCheck, GameObject target){
        // get a vector corressonding to the distance btwn the actorToCheck and taget
        Vector2 angleFromTarget = (Vector2)(target.transform.position -actorToCheck.transform.position);
        angleFromTarget.Normalize();
        //Vector2.Angle(a1, a2)
        float angleDifference = Vector2.Angle(target.GetComponent<Controller>().facingDirection, angleFromTarget);
        
        Debug.Log("Diff btwn " + angleFromTarget.ToString() +target.GetComponent<Controller>().facingDirection +angleDifference.ToString());
        if(angleDifference < 45.0f){
            
            return true;
        }
        else{
            return false;
        }
    }
    public static Vector3 randomPointInRadius(Vector3 target, float radius){
        Vector3 toReturn;
        toReturn = new Vector3(UnityEngine.Random.Range(-radius, radius), UnityEngine.Random.Range(-radius, radius), 0);
        return target + toReturn;
    }
    public static bool areHostle(Actor a1, Actor a2){
        if(a1 == null){
            Debug.LogWarning("isHostle: arg1 was null");
            return false;
        }
        if(a2 == null){
            Debug.LogWarning("isHostle: arg2 was null");
            return false;
        }


        if(a1.gameObject.layer == LayerMask.NameToLayer("Player")){
            if(a2.gameObject.layer == LayerMask.NameToLayer("Enemy")){
                return true;
            }
        }
        if(a1.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            if(a2.gameObject.layer == LayerMask.NameToLayer("Player")){
                return true;
            }
        }
        return false;
    }
    public static bool NT_AuthoritativeClient(NetworkTransform _nt){
        if(_nt == null){
            Debug.LogError("NT_AuthoritativeClient: _nt == null");
            return false;
        }
        //if this client is the with Auth over the NetworkTransform
        if(_nt.isServer && _nt.isLocalPlayer){
            Debug.Log("has NT authority bc HOST's player: " + _nt.gameObject.name );
            return true;
        }
        //if this client is the with Auth over the NetworkTransform
        if(_nt.clientAuthority != _nt.isServer){
            
            if(_nt.netIdentity.hasAuthority){
                Debug.Log("client has NT authority: " + _nt.gameObject.name );
                return true;
            }
            if(_nt.isServer){
                Debug.Log("client bc isServer: " + _nt.gameObject.name );
                return true;
            }
        }
        Debug.Log("client does NOT have auth or nt" + _nt.gameObject.name );
        return false;
        
    }
}
