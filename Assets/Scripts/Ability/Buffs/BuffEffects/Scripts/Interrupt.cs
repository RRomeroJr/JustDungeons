using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewInterruptEffect", menuName = ProjectPaths.buffEffectsMenu + "Interrupt")]
    public class Interrupt : BuffEffect
    {
        public override void StartEffect(GameObject target, float effectValue)
        {
            if (target.TryGetComponent(out IInterrupt t))
            {
                t.InterruptCast();
            }
        }
    }
}

