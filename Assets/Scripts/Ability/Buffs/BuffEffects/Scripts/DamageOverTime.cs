using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewDamageOverTime", menuName = ProjectPaths.buffEffectsMenu + "DamageOverTime")]
    public class DamageOverTime : BuffEffect
    {
        public override void ApplyEffect(Buff buff, float damageValue)
        {
            if (buff.Target.TryGetComponent(out IDamageOverTime t))
            {
                t.ApplyDamage(damageValue);
            }
        }
    }
}
