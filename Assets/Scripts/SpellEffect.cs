using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpellEffect
{
    public string effectName;
    public int effectType; // 0=damage, 1=heal, 2=DoT, 3=Hot, 4= something else... tbd
    public float power;
    public float duration;
    public float tickRate; // for now will be ceilinged
    
public SpellEffect(){
}
public SpellEffect(string inEffectName, int inEffectType, float inPower, float inDuration, float inTickRate){
    effectName = inEffectName;
    effectType = inEffectType;
    power = inPower;
    duration = inDuration;
    tickRate = MathF.Round(inTickRate);

}
}
