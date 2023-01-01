using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = "Assets/Scripts/Ability/Buffs/BuffEffects/ScriptableObjects/NewSilenceEffect", menuName = "HBCsystem/Buffs/Silence")]
    public class Silence : BuffEffect
    {
        public override void EndEffect(IBuff t)
        {
            var target = t as ISilence;
            if (target != null)
            {
                target.Silenced--;
            }
        }

        public override void StartEffect(IBuff t)
        {
            var target = t as ISilence;
            if (target != null)
            {
                target.Silenced++;
            }
        }
    }
}
