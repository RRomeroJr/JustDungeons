using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewSilenceEffect", menuName = ProjectPaths.buffEffectsMenu + "Silence")]
    public class Silence : BuffEffect
    {
        public override void EndEffect(IBuff t, float effectValue)
        {
            var target = t as ISilence;
            if (target != null)
            {
                target.Silenced--;
            }
        }

        public override void StartEffect(IBuff t, float effectValue)
        {
            var target = t as ISilence;
            if (target != null)
            {
                target.Silenced++;
            }
        }
    }
}
