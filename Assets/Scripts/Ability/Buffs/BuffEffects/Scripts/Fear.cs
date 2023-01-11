using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewFearEffect", menuName = ProjectPaths.buffEffectsMenu + "Fear")]
    public class Fear : BuffEffect
    {
        public override void EndEffect(IBuff t, float effectValue)
        {
            var target = t as IFear;
            if (target == null)
            {
                return;
            }
            target.Feared--;
            // Only remove fear if there are no other fears on the target
            if (target.Feared <= 0)
            {
                target.RemoveFear();
            }
        }

        public override void StartEffect(IBuff t, float effectValue)
        {
            var target = t as IFear;
            if (target != null)
            {
                target.Feared++;
                target.ApplyFear();
            }
        }
    }
}
