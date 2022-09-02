using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAbilityData
{
    /*   
        Container for all abilities used by players? Maybe this will be broken up into classes later
    */
    // ------------------------>>>> LATER: use enum for delivery types <<<<------------------------
    public static Ability CastedDamage = new Ability("Testerbolt", AbilityEffectData.oneOffDamageEffect, 0, _castTime: 1.5f);
    public static Ability DoT = new Ability("Debugger\'s Futility", AbilityEffectData.dotEffect, 0, 0.0f, 3.5f);
    public static Ability CastedHeal = new Ability("Quality Assured", AbilityEffectData.oneOffHealEffect, 0, 1.5f, 4.2f);
    public static Ability HoT = new Ability("Sisyphean Resolve", AbilityEffectData.hotEffect, -1, 0.0f);

    /*public static Ability DoubleEffectAbility = new Ability("Double Effect-Bolt",
                                                        new List<AbilityEffectPreset>{AbilityEffectData.oneOffDamageEffect, AbilityEffectData.dotEffect},
                                                            0, 1.5f);
                                                        */
    public static Ability FreeAbilityIfHit = new Ability("Testerbolt x2", AbilityEffectData.DmgWithFollowUpEffect, 1, 2.5f);
    public static Ability AoE = new Ability("TB as AoE", AbilityEffectData.oneOffDamageEffect, 2, 0.5f, _duration: 5.0f);
    public static Ability DelayedDamage = new Ability("Hits aft 4.0s", AbilityEffectData.DelayedOneOffEffect, 0, _castTime: 1.5f);

    public static Ability Teleport = new Ability("Teleport", AbilityEffectData.TeleportEffect, -2);
    public static Ability Dash = new Ability("Dash", AbilityEffectData.DashEffect, -2);

    //public static Ability AoE2 = new Ability("TB as AoE2", AbilityEffectData.oneOffDamageEffect, 3);
    public static Ability DmgBuffBolt = new Ability("Dmg buff bolt", AbilityEffectData.BuffNextTB, -1, _castTime: 1.5f);
    public static Ability DoubleEffectAbility = new Ability("Double Effect-Bolt",
                                                        new List<AbilityEffectPreset>(){AbilityEffectData.oneOffDamageEffect, AbilityEffectData.dotEffect},
                                                            0, 1.5f);
    public static Ability_V2 shadowBolt = new Ability_V2("Shadowbolt", AbilityEffectData.shadowBoltEffect, _castTime : 1.5f);
    public static Ability_V2 mindBlast = new Ability_V2("Mind Blast", AbilityEffectData.mindBlastEffect, _castTime : 1.5f);
    public static Ability_V2 autoAttack = new Ability_V2("Auto attack", AbilityEffectData.autoAttackEffect);
    public static Ability_V2 ingite = new Ability_V2("Ignite", AbilityEffectData.igniteEffect, _castTime : 1.0f);
    public static Ability_V2 shadowFire = new Ability_V2("Shadowfire",
                     new List<AbilityEff>(){AbilityEffectData.shadowBoltEffect, AbilityEffectData.igniteEffect}, _castTime : 1.5f);
    public static Ability_V2 magicMissle = new Ability_V2("Magic Missle", AbilityEffectData.genericMagicEffect, _castTime : 1.0f);

}
