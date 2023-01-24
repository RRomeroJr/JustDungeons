using System;
using System.Linq;
using BuffSystem;
using Mirror;
using UnityEngine;

/// <summary>
/// General purpose buff container which implements every buff in the game
/// </summary>
public class BuffHandler : NetworkBehaviour, IStun, IInterrupt, IFear, ISpeedModifier, IDamageOverTime, IHealOverTime, IDizzy
{
    [SerializeField] private int feared;
    [SerializeField] private int silenced;
    [SerializeField] private int stunned;
    [SerializeField] private int dizzy;
    [SerializeField] private float speedModifier;
    [SerializeField] private readonly SyncList<Buff> buffs = new SyncList<Buff>();

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

    [Server]
    protected virtual void OnDamageTaken(DamageEventArgs e)
    {
        DamageTaken?.Invoke(this, e);
    }

    [Server]
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
        if (!isServer)
        {
            buffs.Callback += OnBuffsUpdated;
        }
    }

    private void OnBuffsUpdated(SyncList<Buff>.Operation op, int index, Buff oldBuff, Buff newBuff)
    {
        if (op == SyncList<Buff>.Operation.OP_ADD)
        {
            newBuff.Start();
        }
        else if (op == SyncList<Buff>.Operation.OP_REMOVEAT)
        {
            oldBuff.End();
        }
    }

    private void Update()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            buffs[i].Update();
        }
    }

    [Server]
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

    [Server]
    public void RemoveBuff(Buff buff)
    {
        buff.Finished -= HandleBuffFinished;
        buff.End();
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

    public void ApplyDamage(float damage)
    {
        if (!isServer)
        {
            return;
        }
        var damageEventArgs = new DamageEventArgs
        {
            Damage = damage
        };
        OnDamageTaken(damageEventArgs);
    }

    public void ApplyHeal(float heal)
    {
        if (!isServer)
        {
            return;
        }
        var healEventArgs = new HealEventArgs
        {
            Heal = heal
        };
        OnHealTaken(healEventArgs);
    }
}
