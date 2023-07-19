using System;

[Serializable]
public class DamageableCooldown
{
    public IDamageable damageable;
    public float remainingTime;

    public DamageableCooldown(IDamageable d, float r)
    {
        damageable = d;
        remainingTime = r;
    }
}
