using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "HBCsystem/Buff")]
public class BuffScriptableObject : ScriptableObject
{
    [SerializeField] private string buffName;
    [SerializeField] private float duration;
    [SerializeField] private float tickRate;
    [SerializeField] private GameObject particles;
    [SerializeField] private List<BuffEffect> buffEffectsList;

    // Buffs default value should not be altered by code. All changes will happen in the inspector
    public string BuffName { get => buffName; }
    public float TickRate { get => tickRate; }
    public float Duration { get => duration; }

    public void StartBuff(IBuff target)
    {
        if (particles != null)
        {
            //GameObject.Instantiate(particles, target.transform as MonoBehaviour);
        }
        foreach (BuffEffect effect in buffEffectsList)
        {
            effect.StartEffect(target);
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
            effect.ApplyEffect(target);
        }
    }

    public void EndBuff(IBuff target)
    {
        foreach (BuffEffect effect in buffEffectsList)
        {
            effect.EndEffect(target);
        }
    }
}
