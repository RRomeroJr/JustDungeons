using System.Collections.Generic;
using UnityEngine;
using BuffSystem;

[CreateAssetMenu(fileName = ProjectPaths.buffs + "NewBuff", menuName = "HBCsystem/NewBuff")]
public class BuffScriptableObject : ScriptableObject
{
    [SerializeField] private string buffName;
    [SerializeField] private float duration;
    [SerializeField] private float tickRate;
    [SerializeField] private bool stackable;
    [SerializeField] private GameObject particles;

    // Store BuffEffects in a list of custom serializable KeyValuePairs so it can edited in the inspector
    [SerializeField] private List<CustomKeyValuePair<BuffEffect, float>> buffEffectsList;

    // Buffs default value should not be altered by code or at runtime. All changes will happen in the inspector
    public string BuffName => buffName;
    public float TickRate => tickRate;
    public float Duration => duration;
    public bool Stackable => stackable;

    public void StartBuff(GameObject target)
    {
        if (particles != null)
        {
            //GameObject.Instantiate(particles, target.transform as MonoBehaviour);
        }
        foreach (var effect in buffEffectsList)
        {
            effect.Key.StartEffect(target, effect.Value);
        }
    }

    public void Tick(GameObject target)
    {
        if (particles != null)
        {
            //GameObject.Instantiate(particles, target.transform as MonoBehaviour);
        }
        foreach (var effect in buffEffectsList)
        {
            effect.Key.ApplyEffect(target, effect.Value);
        }
    }

    public void EndBuff(GameObject target)
    {
        foreach (var effect in buffEffectsList)
        {
            effect.Key.EndEffect(target, effect.Value);
        }
    }
}
