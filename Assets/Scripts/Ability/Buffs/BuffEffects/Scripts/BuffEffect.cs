using UnityEngine;

public abstract class BuffEffect : ScriptableObject
{
    [SerializeField] protected string effectName;
    /// <summary>
    /// Used for effects that get applied multiple times
    /// </summary>
    public virtual void ApplyEffect(IBuff target)
    {
    }

    /// <summary>
    /// Used for effects that happen at the end of buff
    /// </summary>
    public virtual void EndEffect(IBuff target)
    {
    }

    /// <summary>
    /// Used for effects that happen when a buff is first applied
    /// </summary>
    public virtual void StartEffect(IBuff target)
    {
    }
}
