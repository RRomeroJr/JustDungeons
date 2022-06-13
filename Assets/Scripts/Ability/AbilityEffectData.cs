using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityEffectData
{
    /*  
        Container for every Ability Effect in the game.
        Remeber to put particles in the resource folder or they won't be found!
    */
        //                           (Ability Name, Ability Type, Power, Duration, Tick Rate, GameObject particles) || 0=dmg, 1=heal
        public static AbilityEffect _oneOffDamageEffect = new AbilityEffect("Testerbolt Effect", 0, 8.0f, 0.0f, 0.0f);
        public static AbilityEffect _dotEffect = new AbilityEffect("Debugger\'s Futility Effect", 2, 30.0f, 9.0f, 3.0f, Resources.Load<GameObject>("particleTest"));// damage ^^
        public static AbilityEffect _oneOffHealEffect = new AbilityEffect("Quality Assured Effect", 1, 13.0f, 0.0f, 0.0f);
        public static AbilityEffect _hotEffect = new AbilityEffect("Sisyphean Resolve Effect", 3, 25.0f, 4.0f, 1.0f);// heals ^^

}
