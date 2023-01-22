using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewFearEffect", menuName = ProjectPaths.buffEffectsMenu + "Fear")]
    public class Fear : BuffEffect
    {
        public override void EndEffect(GameObject target, float effectValue)
        {
            if (!target.TryGetComponent(out IFear t))
            {
                return;
            }
            t.Feared--;
            // Only remove fear if there are no other fears on the target
            if (t.Feared <= 0)
            {
                t.RemoveFear();
            }
        }

        public override void StartEffect(GameObject target, float effectValue)
        {
            if (target.TryGetComponent(out IFear t))
            {
                t.Feared++;
                t.ApplyFear();
            }
        }
    }
}
