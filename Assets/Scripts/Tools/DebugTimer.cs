using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DebugTimer
{
    public float current = 0.0f;
    public float max = 1.0f;
    public bool completed;
    public bool Update(){
        completed = current >= max;
        if(completed){
            current = 0.0f;
        }
        current += Time.deltaTime;

        return completed;
         
    }
    public DebugTimer(){

    }
    public DebugTimer(float _max){
   
        max = _max;
    }


}