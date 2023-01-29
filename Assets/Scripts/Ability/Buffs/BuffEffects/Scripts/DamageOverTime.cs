using UnityEngine;

[CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewDamageOverTime", menuName = ProjectPaths.buffEffectsMenu + "DamageOverTime")]

public class DamageOverTime : BuffEffect
{
    public override void ApplyEffect(GameObject target, float damageValue)
    {
        if (target.TryGetComponent(out IDamageOverTime t))
        {
            t.ApplyDamage(damageValue);
        }
    }
}
