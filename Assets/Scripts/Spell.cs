using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{
    private string spellName;
    private SpellEffect spellEffect;

    private float castTime;

    public string getName(){
        return spellName;
    }
    public SpellEffect getEffect(){
        return spellEffect;
    }
    public float getCastTime(){
        return castTime;
    }

    public Spell(string inName, SpellEffect inSpellEffect, float inCastTime){
        spellName = inName;
        spellEffect = inSpellEffect;
        castTime = inCastTime;
    }
}
