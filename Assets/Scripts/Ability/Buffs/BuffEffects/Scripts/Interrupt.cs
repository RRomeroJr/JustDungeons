using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewInterruptEffect", menuName = ProjectPaths.buffEffectsMenu + "Interrupt")]
    public class Interrupt : BuffEffect
    {
        public override void ApplyEffect(IBuff t)
        {
            var target = t as IInterrupt;
            if (target != null)
            {
                target.InterruptCast();
            }
        }
    }
}

