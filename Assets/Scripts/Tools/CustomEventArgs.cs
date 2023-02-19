using System;

public class DamageEventArgs : EventArgs
{
    public float Damage { get; set; }
}

public class HealEventArgs : EventArgs
{
    public float Heal { get; set; }
}

public class StatusEffectChangedEventArgs : EventArgs
{
    public int Feared { get; set; }
    public int Silenced { get; set; }
    public int Stunned { get; set; }
    public int Dizzy { get; set; }
    public StatusEffectState NewEffect { get; set; }
}
