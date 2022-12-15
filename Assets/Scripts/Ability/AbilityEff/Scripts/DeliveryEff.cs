using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

[System.Serializable]
public abstract class DeliveryEff : AbilityEff
{
    public List<EffectInstruction> eInstructs;
    
    
    
    //public EffectInstruction eInstruct;

    //public int targetArg = 0; //0 = target, 1 = self

    //public List<Actor> specificTargets;
    
}
