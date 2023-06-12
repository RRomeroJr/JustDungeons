using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewHasteEffect", menuName = ProjectPaths.buffEffectsMenu + "Haste")]
    public class Haste : BuffEffect
    {
        public override void EndEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IHaste t))
            {
                t.Haste = 1 / (1 + (effectValue / 100));
            }
        }

        public override void StartEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IHaste t))
            {
                t.Haste = 1 + (effectValue / 100);
            }
        }
    }
}
