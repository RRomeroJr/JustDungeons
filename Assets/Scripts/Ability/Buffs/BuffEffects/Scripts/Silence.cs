using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewSilenceEffect", menuName = ProjectPaths.buffEffectsMenu + "Silence")]
    public class Silence : BuffEffect
    {
        public override void EndEffect(GameObject target, float effectValue, int stacks)
        {
            if (target.TryGetComponent(out ISilence t))
            {
                t.Silenced--;
            }
        }

        public override void StartEffect(GameObject target, float effectValue, int stacks)
        {
            if (target.TryGetComponent(out ISilence t))
            {
                t.Silenced++;
            }
        }
    }
}
