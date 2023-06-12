using UnityEngine;

[CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewStunEffect", menuName = ProjectPaths.buffEffectsMenu + "Stun")]
public class Stun : BuffEffect
{
    public override void EndEffect(BuffSystem.Buff buff, float effectValue)
    {
        if (buff.Target.TryGetComponent(out IStun t))
        {
            t.Stunned--;
        }
    }

    public override void StartEffect(BuffSystem.Buff buff, float effectValue)
    {
        if (buff.Target.TryGetComponent(out IStun t))
        {
            t.Stunned++;
        }
    }
}
