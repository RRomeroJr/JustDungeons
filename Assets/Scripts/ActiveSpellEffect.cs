using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSpellEffect
{
    public SpellEffect spellEffect;
    public float elaspedTime;
    public Actor caster;

    public ActiveSpellEffect(){}
    public ActiveSpellEffect(SpellEffect inSpellEffect, Actor inCaster){
        spellEffect = inSpellEffect;
        caster = inCaster;
        elaspedTime = 0.0f;
    }

    public string getEffectName(){
        return spellEffect.effectName;
        
    }
    public int getEffectType(){
        return spellEffect.effectType;
    }
    public float getPower(){
        return spellEffect.power;
    }
    public float getDuration(){
        return spellEffect.duration;
    }
    public float getTickRate(){
        return spellEffect.tickRate;
    }
}
