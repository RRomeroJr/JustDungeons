using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewFearEffect", menuName = ProjectPaths.buffEffectsMenu + "Fear")]
    public class Fear : BuffEffect
    {
        public override void ApplyEffect(Buff buff, float effectValue)
        {
            Debug.Log("fear tick");
            if (buff.Target.TryGetComponent(out MovementEffectsController mc))
            {
                mc.TickEffect(StatusEffectState.Feared);
            }
        }

        public override void EndEffect(Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IFear t))
            {
                t.Feared--;
            }
        }

        public override void StartEffect(Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IFear t))
            {
                t.Feared++;
            }
        }
    }
}
