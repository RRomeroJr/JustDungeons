using System;
using System.Collections.Generic;
using BuffSystem;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// General purpose buff container which implements every buff in the game
/// </summary>
public class BuffHandler : MonoBehaviour, IStun, IInterrupt, IFear
{
    [SerializeField] private int feared;
    [SerializeField] private int silenced;
    [SerializeField] private int stunned;
    [SerializeField] private List<Buff> buffs;
    private NavMeshAgent agent;

    #region Events

    /// <summary>
    /// Status effect value has changed
    /// </summary>
    public event EventHandler StatusEffectChanged;
    /// <summary>
    /// Buff has interrupted spellcasting
    /// </summary>
    public event EventHandler Interrupted;

    #endregion

    #region EventRaised

    protected virtual void OnStatusEffectChanged()
    {
        EventHandler raiseEvent = StatusEffectChanged;
        if (raiseEvent != null)
        {
            raiseEvent(this, EventArgs.Empty);
        }
    }

    protected virtual void OnInterrupt()
    {
        EventHandler raiseEvent = StatusEffectChanged;
        if (raiseEvent != null)
        {
            raiseEvent(this, EventArgs.Empty);
        }
    }

    #endregion

    #region Status Effect Properties

    public int Stunned
    {
        get => stunned;
        set
        {
            stunned = value;
            OnStatusEffectChanged();
        }
    }

    public int Feared
    {
        get => feared;
        set
        {
            feared = value;
            OnStatusEffectChanged();
        }
    }
    public int Silenced
    {
        get => silenced;
        set
        {
            silenced = value;
            OnStatusEffectChanged();
        }
    }

    #endregion

    private void Start()
    {
        stunned = 0;
        silenced = 0;
        feared = 0;
        buffs = new List<Buff>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            buffs[i].Update();
        }
    }

    public void AddBuff(BuffScriptableObject buffSO)
    {
        var newBuff = new Buff(buffSO, this);
        newBuff.Finished += HandleBuffFinished;
        buffs.Add(newBuff);
    }

    public void RemoveBuff(Buff buff)
    {
        buff.Finished -= HandleBuffFinished;
        buffs.Remove(buff);
    }

    private void HandleBuffFinished(object sender, EventArgs e)
    {
        RemoveBuff(sender as Buff);
    }

    public void InterruptCast()
    {
        OnInterrupt();
    }

    public void ApplyFear()
    {
        if (agent == null)
        {
            return;
        }
        if (HBCTools.NT_AuthoritativeClient(GetComponent<NetworkTransform>()))
        {
            agent.speed = 1.0f;
            Vector3 randomPointOnCircle = UnityEngine.Random.insideUnitCircle.normalized * 10;
            agent.SetDestination(transform.position + randomPointOnCircle);
        }
    }

    public void RemoveFear()
    {
        if (agent == null)
        {
            return;
        }
        if (HBCTools.NT_AuthoritativeClient(GetComponent<NetworkTransform>()))
        {
            agent.ResetPath();
        }
    }
}

