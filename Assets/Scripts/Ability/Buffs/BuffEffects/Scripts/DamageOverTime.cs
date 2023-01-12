using UnityEngine;

[CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewDamageOverTime", menuName = ProjectPaths.buffEffectsMenu + "DamageOverTime")]

public class DamageOverTime : BuffEffect
{
    public override void ApplyEffect(IBuff t, float damageValue)
    {
        var target = t as IDamageOverTime;
        if (target != null)
        {
            target.ApplyDamage(damageValue);
        }
    }
}
