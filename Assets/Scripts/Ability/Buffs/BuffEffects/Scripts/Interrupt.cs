using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewInterruptEffect", menuName = ProjectPaths.buffEffectsMenu + "Interrupt")]
    public class Interrupt : BuffEffect
    {
        public override void StartEffect(GameObject target, float effectValue, int stacks)
        {
            if (target.TryGetComponent(out IInterrupt t))
            {
                t.InterruptCast();
            }
        }
    }
}

