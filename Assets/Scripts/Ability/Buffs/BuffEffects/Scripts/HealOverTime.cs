using UnityEngine;

[CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewHealOverTime", menuName = ProjectPaths.buffEffectsMenu + "HealOverTime")]

public class HealOverTime : BuffEffect
{
    public override void ApplyEffect(IBuff t, float healValue)
    {
        var target = t as IHealOverTime;
        if (target != null)
        {
            target.ApplyHeal(healValue);
        }
    }
}
