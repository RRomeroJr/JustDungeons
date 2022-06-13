using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAbilityData
{
    /*   
        Container for all abilities used by players? Maybe this will be broken up into classes later
    */
    public static Ability _castedDamage = new Ability("Testerbolt", AbilityEffectData._oneOffDamageEffect, 1.5f);
    public static  Ability _instantAbility = new Ability("Debugger\'s Futility", AbilityEffectData._dotEffect, 0.0f, 3.5f);
    public static Ability _castedHeal = new Ability("Quality Assured", AbilityEffectData._oneOffHealEffect, 1.5f, 4.2f);
    public static Ability _instantAbility2 = new Ability("Sisyphean Resolve Effect", AbilityEffectData._hotEffect, 0.0f);

}
