using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewInterruptEffect", menuName = ProjectPaths.buffEffectsMenu + "Interrupt")]
    public class Interrupt : BuffEffect
    {
        public override void StartEffect(IBuff t, float effectValue)
        {
            var target = t as IInterrupt;
            if (target != null)
            {
                target.InterruptCast();
            }
        }
    }
}

