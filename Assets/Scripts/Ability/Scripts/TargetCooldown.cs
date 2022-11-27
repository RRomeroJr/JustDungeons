using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetCooldown 
{
    public Actor actor;
    public float remainingTime;  

    public TargetCooldown(){

    }
    public TargetCooldown(Actor _actor, float _remainingTime){
        actor = _actor;
        remainingTime = _remainingTime;
    }

}
