using System;

public class DamageEventArgs : EventArgs
{
    public float Damage { get; set; }
}

public class HealEventArgs : EventArgs
{
    public float Heal { get; set; }
}
