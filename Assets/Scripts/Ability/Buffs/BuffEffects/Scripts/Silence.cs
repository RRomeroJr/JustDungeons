using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewSilenceEffect", menuName = ProjectPaths.buffEffectsMenu + "Silence")]
    public class Silence : BuffEffect
    {
        public override void EndEffect(Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out ISilence t))
            {
                t.Silenced--;
            }
        }

        public override void StartEffect(Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out ISilence t))
            {
                t.Silenced++;
            }
        }
    }
}
