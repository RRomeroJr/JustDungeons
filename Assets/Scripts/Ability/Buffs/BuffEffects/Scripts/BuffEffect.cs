using UnityEngine;

public abstract class BuffEffect : ScriptableObject
{
    [SerializeField] protected string effectName;
    /// <summary>
    /// Used for effects that get applied multiple times
    /// </summary>
    public virtual void ApplyEffect(BuffSystem.Buff buff, float effectValue)
    {
    }
    
    /// <summary>
    /// Used for effects that happen at the end of buff
    /// </summary>
    public virtual void EndEffect(BuffSystem.Buff buff, float effectValue)
    {
    }

    /// <summary>
    /// Used for effects that happen when a buff is first applied
    /// </summary>
    public virtual void StartEffect(BuffSystem.Buff buff, float effectValue)
    {
    }
    /// <summary>
    /// Effects that happen when stacks change
    /// </summary>
    public virtual void StacksChangedEffect(BuffSystem.Buff buff, float effectValue, int amountChanged)
    {
    }
}
