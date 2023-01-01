using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Buff", menuName = "HBCsystem/Buff")]
public class BuffScriptableObject : ScriptableObject
{
    [SerializeField] private string buffName;
    [SerializeField] private float duration;
    [SerializeField] private float tickRate; // for now will be rounded
    [SerializeField] private GameObject particles;
    [SerializeField] private bool stackable;
    [SerializeField] public Actor actor;
    [SerializeField] public int id = -1; // Should be a positive unique identifer. Not sure if needed
    [SerializeField] private bool refreshable;

    [SerializeField] private List<BuffEffect> buffEffectsList;

    // Buffs default value should not be altered by code. All changes will happen in the inspector
    public string BuffName { get => buffName; }
    public float TickRate { get => tickRate; }
    public bool Stackable { get => stackable; }
    public bool Refreshable { get => refreshable; }
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


    /*public void Init(string _effectName, float _duration, List<AbilityEff> _effects, float _tickRate = 3.0f, int _id = -1,
                      bool _stackable = false, bool _refreshable = true, uint _stacks = 1, GameObject _particles = null)
    {
    `
        effectName = _effectName;
        duration = _duration;
        //effects = _effects;

        //remainingTime = duration;

        tickRate = _tickRate;
        id = _id;
        stackable = _stackable;
        refreshable = _refreshable;
        stacks = _stacks;
        particles = _particles;
        foreach (AbilityEff eff in _effects)
        {
            eff.parentBuff = this;
        }
        eInstructs = new List<EffectInstruction>();
        eInstructs.addEffects(_effects);

    }
    public void Init(string _effectName, float _duration, List<EffectInstruction> _eInstructs, float _tickRate = 3.0f, int _id = -1,
                      bool _stackable = false, bool _refreshable = true, uint _stacks = 1, GameObject _particles = null)
    {

        effectName = _effectName;
        duration = _duration;

        eInstructs = _eInstructs;
        //remainingTime = duration;

        tickRate = _tickRate;
        id = _id;
        stackable = _stackable;
        refreshable = _refreshable;
        stacks = _stacks;
        particles = _particles;
        foreach (EffectInstruction eI in eInstructs)
        {
            eI.effect.parentBuff = this;
        }

    }

    public Buff clone()
    {
        // Creates an editable version of the input Ability Effect

        Buff temp_ref = ScriptableObject.CreateInstance(typeof(Buff)) as Buff;

        temp_ref.Init(String.Copy(effectName), duration, eInstructs.cloneInstructs(),
         tickRate, id, stackable, refreshable, stacks, particles);
        temp_ref.onCastHooks = onCastHooks;
        temp_ref.onHitHooks = onHitHooks;
        //  temp_ref.caster = caster;
        //  temp_ref.target = target;
        return temp_ref;
    }*/
}
