using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

public static class HBCTools
{
    public enum Quadrant{
        UpRight,
        UpLeft,
        DownLeft,
        DownRight
    }
    public enum ContextualTarget{
        ArenaObject,
        Self,
        Target,
        FollowTarget,
        Blackboard,
        AggroTarget
    }
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
    public static bool checkIfFlank(Actor actorToCheck, Actor target){
        // get a vector corressonding to the distance btwn the actorToCheck and taget
        Vector2 angleFromTarget = (Vector2)(actorToCheck.transform.position - target.transform.position);
        angleFromTarget.Normalize();

        //Vector2.Angle(a1, a2)
        float angleDifference = Vector2.Angle(target.GetComponent<Controller>().facingDirection, angleFromTarget);

        if((45.0f <= angleDifference)&&(angleDifference < 135.0f)){
            return true;
        }
        else{
            return false;
        }
        
    }
    public static bool checkFacing(Actor actorToCheck, GameObject target){
        // get a vector corressonding to the distance btwn the actorToCheck and taget
        Vector2 angleFromTarget = (Vector2)(target.transform.position  - actorToCheck.transform.position );
        angleFromTarget.Normalize();
        //Vector2.Angle(a1, a2)
        float angleDifference = Vector2.Angle(actorToCheck.GetComponent<Controller>().facingDirection, angleFromTarget);
        
        //Debug.Log("Diff btwn " + angleFromTarget.ToString() +actorToCheck.GetComponent<Controller>().facingDirection +angleDifference.ToString());
        if(angleDifference < 90.0f){
            
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
    public static bool areHostle(Transform a1, Transform a2){
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
            // Debug.LogError("NT_AuthoritativeClient: _nt == null");
            return false;
        }
        //if this client is the with Auth over the NetworkTransform
        if(_nt.isServer && _nt.isLocalPlayer){
            // Debug.Log("has NT authority bc HOST's player: " + _nt.gameObject.name );
            return true;
        }
        //if this client is the with Auth over the NetworkTransform
        if(_nt.clientAuthority != _nt.isServer){
            
            if(_nt.netIdentity.hasAuthority){
                // Debug.Log("client has NT authority: " + _nt.gameObject.name );
                return true;
            }
            if(_nt.isServer){
                // Debug.Log("client bc isServer: " + _nt.gameObject.name );
                return true;
            }
        }
        // Debug.Log("client does NOT have auth or nt" + _nt.gameObject.name );
        return false;
        
    }
    public static Vector2 ToNearest45(Vector2 _input){
        Vector2 toReturn;
        
        if(_input.x >= 0.0f){
            toReturn.x = 0.5f;
        }
        else{
            toReturn.x = -0.5f;
        }
        if(_input.y >= 0.0f){
            toReturn.y = 0.5f;
        }
        else{
            toReturn.y = -0.5f;
        }

        return toReturn;
    }
    public static Quadrant GetQuadrant(Vector2 _vect){
        if((_vect.x >= 0.0f) && (_vect.y >= 0.0f)){
            return Quadrant.UpRight;
        }
        else if((_vect.x < 0.0f) && (_vect.y >= 0.0f)){
            return Quadrant.UpLeft;
        }
        else if((_vect.x < 0.0f) && (_vect.y < 0.0f)){
            return Quadrant.DownLeft;
        }
        else{
            return Quadrant.DownRight;
        }
        
    }
    public static Vector2 QuadrantToVector(Quadrant _quad){
        Vector2 toReturn;
        if(_quad == Quadrant.UpRight){
            toReturn = Vector2.right + Vector2.up;
            
        }
        else if(_quad == Quadrant.UpLeft){
            toReturn = Vector2.left + Vector2.up;
        }
        else if(_quad == Quadrant.DownLeft){
            toReturn = Vector2.left + Vector2.down;
        }
        else{
            toReturn = Vector2.right + Vector2.down;
        }
        toReturn.Normalize();
        return toReturn;
    }
    public static Vector3 GetMousePosWP(){
        Vector3 scrnPos = Input.mousePosition;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(scrnPos);
        worldPoint.z = 0.0f;
        return worldPoint;
    }
    public static Vector3 CameraBottomLeftWU(){
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        worldPoint.z = 0.0f;
        return worldPoint;
    }
    public static Vector3 CameraTopRightWU(){
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        worldPoint.z = 0.0f;
        return worldPoint;
    }
    public static float contertToPixels(float _unityUnits){
        return Camera.main.WorldToScreenPoint(Camera.main.transform.position + new Vector3(_unityUnits, 0 , 0)).x;
    }
    public static Vector2 InputInstantOnset(){
        return new Vector2(
            Mathf.Clamp(Input.GetAxis("Horizontal") + Input.GetAxisRaw("Horizontal"), -1, 1 ),
            Mathf.Clamp(Input.GetAxis("Vertical") + Input.GetAxisRaw("Vertical"), -1, 1 ));
    }
    
}
