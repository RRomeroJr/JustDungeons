using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewSpeedModifierEffect", menuName = ProjectPaths.buffEffectsMenu + "SpeedModifier")]
    public class SpeedModifier : BuffEffect
    {
        public override void EndEffect(IBuff t, BuffScriptableObject buffValues)
        {
            var target = t as ISpeedModifier;
            if (target != null)
            {
                target.SpeedModifier = 1 / buffValues.SpeedModifier;
            }
        }

        public override void StartEffect(IBuff t, BuffScriptableObject buffValues)
        {
            var target = t as ISpeedModifier;
            if (target != null)
            {
                target.SpeedModifier = buffValues.SpeedModifier;
            }
        }
    }
}
