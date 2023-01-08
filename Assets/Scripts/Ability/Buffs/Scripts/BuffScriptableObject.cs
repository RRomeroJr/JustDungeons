using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "HBCsystem/Buff")]
public class BuffScriptableObject : ScriptableObject
{
    [SerializeField] private string buffName;
    [SerializeField] private float duration;
    [SerializeField] private float tickRate;
    [SerializeField] private float speedModifier = 1;
    [SerializeField] private GameObject particles;
    [SerializeField] private List<BuffEffect> buffEffectsList;

    // Buffs default value should not be altered by code. All changes will happen in the inspector
    public string BuffName => buffName;
    public float TickRate => tickRate;
    public float Duration => duration;

    public void StartBuff(IBuff target)
    {
        if (particles != null)
        {
            //GameObject.Instantiate(particles, target.transform as MonoBehaviour);
        }
        foreach (BuffEffect effect in buffEffectsList)
        {
            effect.StartEffect(target, this);
        }
    }

    public void Tick(IBuff target)
    {
        if (particles != null)
        {
            //GameObject.Instantiate(particles, target.transform as MonoBehaviour);
        }
        foreach (BuffEffect effect in buffEffectsList)
        {
            effect.ApplyEffect(target, this);
        }
    }

    public void EndBuff(IBuff target)
    {
        foreach (BuffEffect effect in buffEffectsList)
        {
            effect.EndEffect(target, this);
        }
    }
}
