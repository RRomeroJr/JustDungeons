using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewSpeedModifierEffect", menuName = ProjectPaths.buffEffectsMenu + "SpeedModifier")]
    public class SpeedModifier : BuffEffect
    {
        public override void EndEffect(GameObject target, float effectValue)
        {
            if (target.TryGetComponent(out ISpeedModifier t))
            {
                t.SpeedModifier = 1 / effectValue;
            }
        }

        public override void StartEffect(GameObject target, float effectValue)
        {
            if (target.TryGetComponent(out ISpeedModifier t))
            {
                t.SpeedModifier = effectValue;
            }
        }
    }
}
