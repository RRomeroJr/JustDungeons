using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ProjectPaths.buffs + "NewBuff", menuName = "HBCsystem/NewBuff")]
public class BuffScriptableObject : ScriptableObject
{
    [SerializeField] private string buffName;
    [SerializeField] private float duration;
    [SerializeField] private float tickRate;
    [SerializeField] private float speedModifier = 1;
    [SerializeField] private float damagePerTick;
    [SerializeField] private GameObject particles;
    [SerializeField] private List<BuffEffect> buffEffectsList;

    // Buffs default value should not be altered by code. All changes will happen in the inspector
    public string BuffName => buffName;
    public float TickRate => tickRate;
    public float Duration => duration;
    public float SpeedModifier => speedModifier;
    public float DamagePerTick => damagePerTick;

    public void StartBuff(IBuff target)
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

    public void Tick(IBuff target)
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

    public void EndBuff(IBuff target)
    {
        foreach (var effect in buffEffectsList)
        {
            effect.Key.EndEffect(target, effect.Value);
        }
    }
}
