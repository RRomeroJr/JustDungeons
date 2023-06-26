using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewHealingOutMod", menuName = ProjectPaths.buffEffectsMenu + "HealingOut")]
    public class HealingOutMod : BuffEffect
    {
        public override void EndEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IHealingOutMod t))
            {
                // Debug.Log(t.HealingOutMod+  " - " +  effectValue);
                t.HealingOutMod -= buff.Stacks * effectValue;
            }
        }

        public override void StartEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IHealingOutMod t))
            {
                // Debug.Log(t.HealingOutMod+  " + " +  effectValue);
                t.HealingOutMod += effectValue;
            }
        }
        


        public override void StacksChangedEffect(BuffSystem.Buff buff,  float effectValue, int amountChanged)
        {

            if (buff.Target.TryGetComponent(out IHealingOutMod t))
            {
                t.HealingOutMod += amountChanged  * effectValue;

            }
        }
        
    }
}
