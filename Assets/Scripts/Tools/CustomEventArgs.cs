using System;
using System.Collections.Generic;

public enum CombatModIDs
{
    DamageTaken, DamageOut, HealingTaken, HealingOut
}
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

    public Dictionary<StatusEffectState, int> ToDictionary()
    {
        var dict = new Dictionary<StatusEffectState, int>
        {
            { StatusEffectState.Feared, Feared },
            { StatusEffectState.Silenced, Silenced },
            { StatusEffectState.Stunned, Stunned },
            { StatusEffectState.Dizzy, Dizzy }
        };
        return dict;
    }
}

public class SpeedChangedEventArgs : EventArgs
{
    public float Slow { get; set; }
    public float Haste { get; set; }
}
public class DamageTakenModChangedEventArgs : EventArgs
{
    public float eDamageTakenMod { get; set; }
}
public class CombatModChangedEventArgs : EventArgs
{
    public float eFloat { get; set; }
    public CombatModIDs eCombatModID { get; set; }
}
