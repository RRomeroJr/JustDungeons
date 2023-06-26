using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewSlowEffect", menuName = ProjectPaths.buffEffectsMenu + "Slow")]
    public class Slow : BuffEffect
    {
        public override void EndEffect(Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out ISlow t))
            {
                t.Slow = 1 / (1 + (effectValue / 100));
            }
        }

        public override void StartEffect(Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out ISlow t))
            {
                t.Slow = 1 + (effectValue / 100);
            }
        }
    }
}
