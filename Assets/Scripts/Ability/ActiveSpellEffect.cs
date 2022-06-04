using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[System.Serializable]
public class ActiveSpellEffect
{
    /* 
                   Spell Effects that are currently active on some target Actor
    */
    public SpellEffect spellEffect;
    public Actor caster;
    public string effectName;
    public int effectType; // 0=damage, 1=heal, 2=DoT, 3=Hot, 4=something else... tbd
    public float power;
    public float duration;
    public float tickRate; // for now rounded

    public float lastTick; // time since last tick

    public float remainingTime;
    public bool start;

    public ActiveSpellEffect(){
        start = false;
    }
    public ActiveSpellEffect(SpellEffect inSpellEffect, Actor inCaster){
        spellEffect = inSpellEffect;
        caster = inCaster;
        effectName = inSpellEffect.getEffectName();
        effectType = inSpellEffect.getEffectType();
        power = inSpellEffect.getPower();
        duration = inSpellEffect.getDuration();
        remainingTime = inSpellEffect.getDuration();
        tickRate = MathF.Round(inSpellEffect.getTickRate());
        start = false;
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
