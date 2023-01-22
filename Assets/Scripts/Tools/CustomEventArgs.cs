using System;

public class DamageEventArgs : EventArgs
{
    public float Damage { get; set; }
}

public class HealEventArgs : EventArgs
{
    public float Heal { get; set; }
}

public class StateChangedEventArgs : EventArgs
{
    public ActorState ActorState { get; set; }
}
