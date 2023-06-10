using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewSlowEffect", menuName = ProjectPaths.buffEffectsMenu + "Slow")]
    public class Slow : BuffEffect
    {
        public override void EndEffect(GameObject target, float effectValue)
        {
            if (target.TryGetComponent(out ISlow t))
            {
                t.Slow = 1 / (1 + (effectValue / 100));
            }
        }

        public override void StartEffect(GameObject target, float effectValue)
        {
            if (target.TryGetComponent(out ISlow t))
            {
                t.Slow = 1 + (effectValue / 100);
            }
        }
    }
}
