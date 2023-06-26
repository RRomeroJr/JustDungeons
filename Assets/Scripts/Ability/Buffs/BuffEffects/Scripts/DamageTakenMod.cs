using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewDamageTakenMod", menuName = ProjectPaths.buffEffectsMenu + "DamageTaken")]
    public class DamageTakenMod : BuffEffect
    {
        public override void EndEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IDamageTakenMod t))
            {
                // Debug.Log(t.DamageTakenMod+  " - " +  effectValue);
                t.DamageTakenMod -= buff.Stacks * effectValue;
            }
        }

        public override void StartEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out IDamageTakenMod t))
            {
                // Debug.Log(t.DamageTakenMod+  " + " +  effectValue);
                t.DamageTakenMod += effectValue;
            }
        }
        


        public override void StacksChangedEffect(BuffSystem.Buff buff,  float effectValue, int amountChanged)
        {

            if (buff.Target.TryGetComponent(out IDamageTakenMod t))
            {
                t.DamageTakenMod += amountChanged  * effectValue;

            }
        }
        
    }
}
