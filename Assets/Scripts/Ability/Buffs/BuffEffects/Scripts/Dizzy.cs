using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewDizzyEffect", menuName = ProjectPaths.buffEffectsMenu + "Dizzy")]
    public class Dizzy : BuffEffect
    {
        public override void EndEffect(GameObject target, float effectValue)
        {
            if (target.TryGetComponent(out IDizzy t))
            {
                t.Dizzy--;
            }
        }

        public override void StartEffect(GameObject target, float effectValue)
        {
            if (target.TryGetComponent(out IDizzy t))
            {
                t.Dizzy++;
            }
        }
    }
}

