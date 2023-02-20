using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewHasteEffect", menuName = ProjectPaths.buffEffectsMenu + "Haste")]
    public class Haste : BuffEffect
    {
        public override void EndEffect(GameObject target, float effectValue)
        {
            if (target.TryGetComponent(out IHaste t))
            {
                t.Haste = 1 / (1 + (effectValue / 100));
            }
        }

        public override void StartEffect(GameObject target, float effectValue)
        {
            if (target.TryGetComponent(out IHaste t))
            {
                t.Haste = 1 + (effectValue / 100);
            }
        }
    }
}
