using UnityEngine;

[CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewStunEffect", menuName = ProjectPaths.buffEffectsMenu + "Stun")]
public class Stun : BuffEffect
{
    public override void EndEffect(GameObject target, float effectValue, int stacks)
    {
        if (target.TryGetComponent(out IStun t))
        {
            t.Stunned--;
        }
    }

    public override void StartEffect(GameObject target, float effectValue, int stacks)
    {
        if (target.TryGetComponent(out IStun t))
        {
            t.Stunned++;
        }
    }
}
