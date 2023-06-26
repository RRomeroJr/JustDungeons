using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewDamageOutMod", menuName = ProjectPaths.buffEffectsMenu + "DamageOut")]
    public class DamageOutMod : BuffEffect
    {
        public override void EndEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IDamageOutMod t))
            {
                // Debug.Log(t.DamageOutMod+  " - " +  effectValue);
                t.DamageOutMod -= buff.Stacks * effectValue;
            }
        }

        public override void StartEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IDamageOutMod t))
            {
                // Debug.Log(t.DamageOutMod+  " + " +  effectValue);
                t.DamageOutMod += effectValue;
            }
        }
        


        public override void StacksChangedEffect(BuffSystem.Buff buff,  float effectValue, int amountChanged)
        {

            if (buff.Target.TryGetComponent(out IDamageOutMod t))
            {
                t.DamageOutMod += amountChanged  * effectValue;

            }
        }
        
    }
}
