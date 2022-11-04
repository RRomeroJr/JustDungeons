using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeterEntry 
{
    public Actor actor;
    public int total = 0;  

    public MeterEntry(){

    }
   
    public MeterEntry(Actor _actor){
        actor = _actor;
    }
    
}
