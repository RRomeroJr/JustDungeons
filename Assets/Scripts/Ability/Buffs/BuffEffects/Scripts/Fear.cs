using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewFearEffect", menuName = ProjectPaths.buffEffectsMenu + "Fear")]
    public class Fear : BuffEffect
    {
        public override void EndEffect(GameObject target, float effectValue, int stacks)
        {
            if (target.TryGetComponent(out IFear t))
            {
                t.Feared--;
            }
        }

        public override void StartEffect(GameObject target, float effectValue, int stacks)
        {
            if (target.TryGetComponent(out IFear t))
            {
                t.Feared++;
            }
        }
    }
}
