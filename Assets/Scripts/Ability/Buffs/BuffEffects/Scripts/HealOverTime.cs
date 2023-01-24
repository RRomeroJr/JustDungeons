using UnityEngine;

[CreateAssetMenu(fileName = ProjectPaths.buffEffects + "NewHealOverTime", menuName = ProjectPaths.buffEffectsMenu + "HealOverTime")]

public class HealOverTime : BuffEffect
{
    public override void ApplyEffect(GameObject target, float healValue)
    {
        if (target.TryGetComponent(out IHealOverTime t))
        {
            t.ApplyHeal(healValue);
        }
    }
}
