using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewStunEffect", menuName = ProjectPaths.buffEffectsMenu + "Stun")]
    public class Stun : BuffEffect
    {
        public override void EndEffect(Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IStun t))
            {
                t.Stunned--;
            }
        }

        public override void StartEffect(Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IStun t))
            {
                t.Stunned++;
            }
        }
    }
}
