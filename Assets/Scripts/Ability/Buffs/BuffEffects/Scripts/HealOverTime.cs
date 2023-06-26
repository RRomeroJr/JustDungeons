using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewHealOverTime", menuName = ProjectPaths.buffEffectsMenu + "HealOverTime")]
    public class HealOverTime : BuffEffect
    {
        public override void ApplyEffect(Buff buff, float healValue)
        {
            if (buff.Target.TryGetComponent(out IHealOverTime t))
            {
                t.ApplyHeal(healValue);
            }
        }
    }
}
