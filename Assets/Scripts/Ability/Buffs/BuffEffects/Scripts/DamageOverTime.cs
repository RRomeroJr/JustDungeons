using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewSpeedModifierEffect", menuName = ProjectPaths.buffEffectsMenu + "SpeedModifier")]

public class DamageOverTime : BuffEffect
{
    public override void ApplyEffect(IBuff t, BuffScriptableObject buffValues)
    {
        var target = t as IDamageOverTime;
        if (target != null)
        {
            target.ApplyDamage(buffValues.DamagePerTick);
        }
    }
}
