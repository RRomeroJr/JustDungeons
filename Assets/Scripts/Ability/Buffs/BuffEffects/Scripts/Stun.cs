using UnityEngine;

[CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewStunEffect", menuName = ProjectPaths.buffEffectsMenu + "Stun")]
public class Stun : BuffEffect
{
    public override void EndEffect(IBuff t, float s)
    {
        var target = t as IStun;
        if (target != null)
        {
            target.Stunned--;
        }
    }

    public override void StartEffect(IBuff t, float s)
    {
        var target = t as IStun;
        if (target != null)
        {
            target.Stunned++;
        }
    }
}
