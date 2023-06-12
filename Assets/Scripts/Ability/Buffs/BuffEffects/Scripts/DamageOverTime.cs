using UnityEngine;

[CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewDamageOverTime", menuName = ProjectPaths.buffEffectsMenu + "DamageOverTime")]

public class DamageOverTime : BuffEffect
{
    public override void ApplyEffect(BuffSystem.Buff buff, float damageValue)
    {
        if (buff.Target.TryGetComponent(out IDamageOverTime t))
        {
            t.ApplyDamage(damageValue);
        }
    }
}
