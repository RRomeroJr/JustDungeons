using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewHealingTakenMod", menuName = ProjectPaths.buffEffectsMenu + "HealingTaken")]
    public class HealingTakenMod : BuffEffect
    {
        public override void EndEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IHealingTakenMod t))
            {
                // Debug.Log(t.HealingTakenMod+  " - " +  effectValue);
                t.HealingTakenMod -= buff.Stacks * effectValue;
            }
        }

        public override void StartEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IHealingTakenMod t))
            {
                // Debug.Log(t.HealingTakenMod+  " + " +  effectValue);
                t.HealingTakenMod += effectValue;
            }
        }
        


        public override void StacksChangedEffect(BuffSystem.Buff buff,  float effectValue, int amountChanged)
        {

            if (buff.Target.TryGetComponent(out IHealingTakenMod t))
            {
                t.HealingTakenMod += amountChanged  * effectValue;

            }
        }
        
    }
}
