using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewDizzyEffect", menuName = ProjectPaths.buffEffectsMenu + "Dizzy")]
    public class Dizzy : BuffEffect
    {
        public override void EndEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IDizzy t))
            {
                t.Dizzy--;
            }
        }

        public override void StartEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IDizzy t))
            {
                t.Dizzy++;
            }
        }
    }
}

