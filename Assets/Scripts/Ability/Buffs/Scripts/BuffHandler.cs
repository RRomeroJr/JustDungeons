using System;
using System.Collections.Generic;
using BuffSystem;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// General purpose buff container which implements every buff in the game
/// </summary>
public class BuffHandler : MonoBehaviour, IStun, IInterrupt, IFear, ISpeedModifier, IDamageOverTime, IHealOverTime
{
    [SerializeField] private int feared;
    [SerializeField] private int silenced;
    [SerializeField] private int stunned;
    [SerializeField] private int dizzy;
    [SerializeField] private float speedModifier;
    [SerializeField] private List<Buff> buffs;
    private NavMeshAgent agent;
    private Controller controller;

    #region Events

    /// <summary>
    /// Status effect value has changed
    /// </summary>
    public event EventHandler StatusEffectChanged;
    /// <summary>
    /// Buff has interrupted spellcasting
    /// </summary>
    public event EventHandler Interrupted;
    /// <summary>
    /// Receive damage from buff
    /// </summary>
    public event EventHandler<DamageEventArgs> DamageTaken;
    /// <summary>
    /// Receive heal from buff
    /// </summary>
    public event EventHandler<HealEventArgs> HealTaken;

    #endregion

    #region EventRaised

    protected virtual void OnStatusEffectChanged()
    {
        StatusEffectChanged?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnInterrupt()
    {
        EventHandler raiseEvent = Interrupted;
        if (raiseEvent != null)
        {
            raiseEvent(this, EventArgs.Empty);
        }
    }


    protected virtual void OnDamageTaken(DamageEventArgs e)
    {
        DamageTaken?.Invoke(this, e);
    }

    protected virtual void OnHealTaken(HealEventArgs e)
    {
        HealTaken?.Invoke(this, e);
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

    public float SpeedModifier
    {
        get => speedModifier;
        set
        {
            speedModifier *= value;
        }
    }

    public int Dizzy
    {
        get => dizzy;
        set
        {
            dizzy = value;
            OnStatusEffectChanged();
        }
    }

    #endregion

    private void Start()
    {
        stunned = 0;
        silenced = 0;
        feared = 0;
        speedModifier = 1;
        buffs = new List<Buff>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<Controller>();
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
        var newBuff = new Buff
        {
            target = this.gameObject,
            buff = buffSO,
            remainingTime = buffSO.Duration
        };
        newBuff.Start();
        buffs.Add(newBuff);
        buffs.Last().Finished += HandleBuffFinished;
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
            agent.speed = controller.moveSpeed * speedModifier;
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

    public void ApplyDamage(float damage)
    {
        var damageEventArgs = new DamageEventArgs
        {
            Damage = damage
        };
        OnDamageTaken(damageEventArgs);
    }

    public void ApplyHeal(float heal)
    {
        var healEventArgs = new HealEventArgs
        {
            Heal = heal
        };
        OnHealTaken(healEventArgs);
    }
}
