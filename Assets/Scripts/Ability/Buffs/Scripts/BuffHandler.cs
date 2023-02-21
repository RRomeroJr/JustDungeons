using System;
using System.Linq;
using BuffSystem;
using Mirror;
using UnityEngine;

/// <summary>
/// General purpose buff container which implements every buff in the game
/// </summary>
public class BuffHandler : NetworkBehaviour, IAllBuffs
{
    // If immune, value will always return 0 even if debug value has value other than 0
    [Header("Set Status Effect Immunities")]
    [SerializeField] private bool fearImmune;
    [SerializeField] private bool silenceImmune;
    [SerializeField] private bool stunImmune;
    [SerializeField] private bool dizzyImmune;
    [SerializeField] private bool slowImmune;
    [SerializeField] private bool hasteImmune;

    [Header("Debug Values")]
    [SerializeField] private int feared;
    [SerializeField] private int silenced;
    [SerializeField] private int stunned;
    [SerializeField] private int dizzy;
    [SerializeField] private float slow;
    [SerializeField] private float haste;
    [SerializeField] private readonly SyncList<Buff> buffs = new SyncList<Buff>();

    #region Events

    /// <summary>
    /// Status effect value has changed
    /// </summary>
    public event EventHandler<StatusEffectChangedEventArgs> StatusEffectChanged;
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
    /// <summary>
    /// New slow or haste buff effect applied
    /// </summary>
    public event EventHandler<SpeedChangedEventArgs> SpeedChanged;

    #endregion

    #region EventRaised

    protected virtual void OnStatusEffectChanged(StatusEffectChangedEventArgs e)
    {
        StatusEffectChanged?.Invoke(this, e);
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

    protected virtual void OnSpeedChanged(SpeedChangedEventArgs e)
    {
        SpeedChanged?.Invoke(this, e);
    }

    #endregion

    #region Status Effect Properties

    public int Stunned
    {
        get
        {
            if (stunImmune)
            {
                return 0;
            }
            return stunned;
        }
        set
        {
            var newEffect = value > stunned ? StatusEffectState.Stunned : StatusEffectState.None;
            stunned = value;
            ChangeStatusEffect(newEffect);
        }
    }

    public int Feared
    {
        get
        {
            if (fearImmune)
            {
                return 0;
            }
            return feared;
        }
        set
        {
            var newEffect = value > feared ? StatusEffectState.Feared : StatusEffectState.None;
            feared = value;
            ChangeStatusEffect(newEffect);
        }
    }

    public int Silenced
    {
        get
        {
            if (silenceImmune)
            {
                return 0;
            }
            return silenced;
        }
        set
        {
            var newEffect = value > silenced ? StatusEffectState.Silenced : StatusEffectState.None;
            silenced = value;
            ChangeStatusEffect(newEffect);
        }
    }

    public int Dizzy
    {
        get
        {
            if (dizzyImmune)
            {
                return 0;
            }
            return dizzy;
        }
        set
        {
            var newEffect = value > dizzy ? StatusEffectState.Dizzy : StatusEffectState.None;
            dizzy = value;
            ChangeStatusEffect(newEffect);
        }
    }

    public float Haste
    {
        get => haste;
        set
        {
            haste *= value;
            ChangeSpeed();
        }
    }

    public float Slow
    {
        get => slow;
        set
        {
            slow *= value;
            ChangeSpeed();
        }
    }

    #endregion

    private void Start()
    {
        stunned = 0;
        silenced = 0;
        feared = 0;
        dizzy = 0;
        slow = 1;
        haste = 1;
        if (!isServer)
        {
            buffs.Callback += OnBuffsUpdated;
        }
    }

    private void Update()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            buffs[i].Update();
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

    [Server]
    public void AddBuff(BuffScriptableObject buffSO)
    {
        if (RefreshOrStackBuff(buffSO))
        {
            return;
        }

        // Create buff if it is not already present
        var newBuff = new Buff
        {
            target = this.gameObject,
            buffSO = buffSO,
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

    private void ChangeStatusEffect(StatusEffectState newEffect)
    {
        var statusEffectChangedEventArgs = new StatusEffectChangedEventArgs
        {
            Feared = Feared,
            Silenced = Silenced,
            Stunned = Stunned,
            Dizzy = Dizzy,
            NewEffect = newEffect
        };
        OnStatusEffectChanged(statusEffectChangedEventArgs);
    }

    private void ChangeSpeed()
    {
        var speedChangedEventArgs = new SpeedChangedEventArgs
        {
            Slow = Slow,
            Haste = Haste
        };
        OnSpeedChanged(speedChangedEventArgs);
    }

    /// <summary>
    /// Handles refreshing or stacking a buff if it is already present on the target
    /// </summary>
    /// <returns>True if the buff was refreshed or stacked, false if it was not present</returns>
    private bool RefreshOrStackBuff(BuffScriptableObject buffSO)
    {
        var buff = buffs.FirstOrDefault(b => b.buffSO == buffSO);
        if (buff == null)
        {
            return false;
        }
        if (buff.buffSO.Stackable)
        {
            buff.Stacks++;
        }
        buff.remainingTime = buffSO.Duration;
        return true;
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
