using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Scripts/Ability/Buffs/BuffEffects/ScriptableObjects/NewStunEffect", menuName = "HBCsystem/Buffs/Stun")]
public class Stun : BuffEffect
{
    public override void EndEffect(IBuff t)
    {
        var target = t as IStun;
        if (target != null)
        {
            target.Stunned--;
        }
    }

    public override void StartEffect(IBuff t)
    {
        var target = t as IStun;
        if (target != null)
        {
            target.Stunned++;
        }
    }
}
