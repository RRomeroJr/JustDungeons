using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class AbilityEffectData
{
    /*  
        Container of Ability Effects and start/ hit/ finish effects.
        Remeber to put particles in the resource folder or they won't be found!
    */
        //                           (AbilityEffect effectName, Ability Type, Power, Duration, Tick Rate, GameObject particles) || 0=dmg, 1=heal
        public static AbilityEffectPreset oneOffDamageEffect = new AbilityEffectPreset("1 Off Dmg Effect", 0, 8.0f, 0.0f, 0.0f);
        public static AbilityEffectPreset dotEffect = new AbilityEffectPreset("DoT Effect", 2, 30.0f, 9.0f, 3.0f, Resources.Load<GameObject>("DoT_Particles"));// damage ^^
        public static AbilityEffectPreset oneOffHealEffect = new AbilityEffectPreset("1 Off Heal Effect", 1, 13.0f, 0.0f, 0.0f);
        public static AbilityEffectPreset hotEffect = new AbilityEffectPreset("HoT Effect", 3, 25.0f, 4.0f, 1.0f, Resources.Load<GameObject>("HoT_Particles"));// heals ^^
        public static AbilityEffectPreset DmgWithFollowUpEffect = new AbilityEffectPreset("1 off x2 Effect", 0, 8.0f, 0.0f, _finishAction: secondaryTestboltFinish);
        public static AbilityEffectPreset DelayedOneOffEffect = new AbilityEffectPreset("Delayed 1 off Effect", 0, 20.0f, 4.0f);

/*
        ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~vV*****Start/ Hit/ Finish Actions*****Vv~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
*/
        public static void secondaryTestboltFinish(Actor caster, Actor target){
            caster.freeCast(PlayerAbilityData.CastedDamage, target);
        }
        public static void secondaryDoT(Actor caster, Actor target){
            caster.freeCast(PlayerAbilityData.DoT, target);
        }
}
