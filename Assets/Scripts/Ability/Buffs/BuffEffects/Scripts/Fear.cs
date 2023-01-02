using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = "Assets/Scripts/Ability/Buffs/BuffEffects/ScriptableObjects/NewFearEffect", menuName = "HBCsystem/Buffs/Fear")]
    public class Fear : BuffEffect
    {
        public override void EndEffect(IBuff t)
        {
            var target = t as IFear;
            if (target != null)
            {
                target.Feared--;
                target.RemoveFear();
            }
        }

        public override void StartEffect(IBuff t)
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
