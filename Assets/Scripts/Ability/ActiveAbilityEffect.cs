using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[System.Serializable]
public class ActiveAbilityEffect
{
    /* 
                   Ability Effects that are currently active on some target Actor
    */
    public AbilityEffect abilityEffect;
    public Actor caster;
    public string effectName;
    public int effectType; // 0=damage, 1=heal, 2=DoT, 3=Hot, 4=something else... tbd
    public float power;
    public float duration;
    public float tickRate; // for now rounded

    public float lastTick; // time since last tick

    public float remainingTime;
    public bool start;
    public GameObject particles;

    public ActiveAbilityEffect(){
        start = false;
    }
    public ActiveAbilityEffect(AbilityEffect inAbilityEffect, Actor inCaster){
        abilityEffect = inAbilityEffect;
        caster = inCaster;
        effectName = inAbilityEffect.getEffectName();
        effectType = inAbilityEffect.getEffectType();
        power = inAbilityEffect.getPower();
        duration = inAbilityEffect.getDuration();
        remainingTime = inAbilityEffect.getDuration();
        tickRate = MathF.Round(inAbilityEffect.getTickRate());
        start = false;
        if(inAbilityEffect.particles != null)
            particles = inAbilityEffect.particles;
    }

    public string getEffectName(){
        return effectName;
    }
    public int getEffectType(){
        return effectType;
    }
    public float getPower(){
        return power;
    }
    public float getDuration(){
        return duration;
    }  
    public float getTickRate(){
        return tickRate;
    }
}
