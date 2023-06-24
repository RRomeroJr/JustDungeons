using UnityEngine;
using BuffSystem;

public abstract class BuffEffect : ScriptableObject
{
    [SerializeField] protected string effectName;
    /// <summary>
    /// Used for effects that get applied multiple times
    /// </summary>
    public virtual void ApplyEffect(Buff buff, float effectValue)
    {
    }

    /// <summary>
    /// Used for effects that happen at the end of buff
    /// </summary>
    public virtual void EndEffect(Buff buff, float effectValue)
    {
    }

    /// <summary>
    /// Used for effects that happen when a buff is first applied
    /// </summary>
    public virtual void StartEffect(Buff buff, float effectValue)
    {
    }

    /// <summary>
    /// Effects that happen when stacks change
    /// </summary>
    public virtual void StacksChangedEffect(Buff buff, float effectValue, int amountChanged)
    {
    }
}
