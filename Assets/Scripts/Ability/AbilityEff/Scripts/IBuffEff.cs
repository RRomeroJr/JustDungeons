using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using OldBuff;

public interface IBuffEff
{
    public float? RemainingTimeOverride {get;set;}
    public float? TickRateOverride {get;set;}
    public uint? StacksOverride {get;set;}
    
}
