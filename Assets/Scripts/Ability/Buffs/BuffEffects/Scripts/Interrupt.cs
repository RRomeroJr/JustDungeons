using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewInterruptEffect", menuName = ProjectPaths.buffEffectsMenu + "Interrupt")]
    public class Interrupt : BuffEffect
    {
        public override void StartEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IInterrupt t))
            {
                t.InterruptCast();
            }
        }
    }
}

