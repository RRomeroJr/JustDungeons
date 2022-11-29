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
        Vector2 angleFromTarget = (Vector2)(actorToCheck.transform.position - target.transform.position);
        angleFromTarget.Normalize();

        //Vector2.Angle(a1, a2)
        float angleDifference = Vector2.Angle(target.GetComponent<Controller>().facingDirection, angleFromTarget);

        if(angleDifference < 45.0f){
            return true;
        }
        else{
            return false;
        }
        
    }

}
