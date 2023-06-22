using UnityEngine;

namespace BuffSystem
{
    [CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewInterruptEffect", menuName = ProjectPaths.buffEffectsMenu + "Interrupt")]
    public class Interrupt : BuffEffect
    {
        public override void StartEffect(BuffSystem.Buff buff, float effectValue)
        {
            

            if (buff.Target.TryGetComponent(out ISilence t))
            {

                if (buff.Target.GetComponent<Actor>().IsCasting == false)
                {
                    try{
                        t.RemoveBuff(buff);
                        Debug.Log("target not casting ending inturrupt early");
                    }
                    catch{}
                }
                else
                {
                    Debug.Log("target was casting");
                }
                buff.Target.GetComponent<Actor>().interruptCast();
                t.Silenced++;
            }
        }
        public override void EndEffect(BuffSystem.Buff buff, float effectValue)
        {
            if (buff.Target.TryGetComponent(out ISilence t))
            {
                t.Silenced--;
           }
        }
    }
}

