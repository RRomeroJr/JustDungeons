using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAbilityData
{
    /*   
        Container for all abilities used by players? Maybe this will be broken up into classes later
    */
    public static Ability CastedDamage = new Ability("Testerbolt", AbilityEffectData.oneOffDamageEffect, _castTime: 1.5f);
    public static Ability DoT = new Ability("Debugger\'s Futility", AbilityEffectData.dotEffect, 0, 0.0f, 3.5f);
    public static Ability CastedHeal = new Ability("Quality Assured", AbilityEffectData.oneOffHealEffect, 0, 1.5f, 4.2f);
    public static Ability HoT = new Ability("Sisyphean Resolve", AbilityEffectData.hotEffect, -1, 0.0f);

    public static Ability DoubleEffectAbility = new Ability("Double Effect-Bolt",
                                                        new List<AbilityEffectPreset>{AbilityEffectData.oneOffDamageEffect, AbilityEffectData.dotEffect},
                                                            0, 1.5f);
    public static Ability FreeAbilityIfHit = new Ability("Testerbolt x2", AbilityEffectData.DmgWithFollowUpEffect, 1, 2.5f);
    public static Ability AoE = new Ability("TB as AoE", AbilityEffectData.oneOffDamageEffect, 2, 0.5f);
    public static Ability DelayedDamage = new Ability("Hits aft 4.0s", AbilityEffectData.DelayedOneOffEffect, _castTime: 1.5f);

    public static Ability Teleport = new Ability("Teleport", AbilityEffectData.TeleportEffect, -2);
    public static Ability Dash = new Ability("Dash", AbilityEffectData.DashEffect, -2);

}
