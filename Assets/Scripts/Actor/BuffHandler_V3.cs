using System;
using System.Collections.Generic;
using BuffSystem;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// General purpose buff container which implements every buff in the game
/// </summary>
public class BuffHandler_V3 : NetworkBehaviour
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
    [SerializeField] private float damageTakenMod;
    [SerializeField] private float damageOutMod;
    [SerializeField] private float healingTakenMod;
    [SerializeField] private float healingOutMod;

    // [SerializeField] private readonly SyncList<Buff> buffs = new();
    [SerializeField] public List<OldBuff.Buff> buffs = new();

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
    public event EventHandler<DamageTakenModChangedEventArgs> DamageTakenModChanged;
    public event EventHandler<CombatModChangedEventArgs> CombatModChanged;
    public UnityEvent OnBuffUpdate = new UnityEvent();

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
    public float DamageTakenMod
    {
        get => damageTakenMod;
        set
        {
            damageTakenMod = value;
            ChangeDamageTakenMod();
        }
    }
    public float DamageOutMod
    {
        get => damageOutMod;
        set
        {
            damageOutMod = value;
            ChangeCombatMod(CombatModIDs.DamageOut);
        }
    }
    public float HealingTakenMod
    {
        get => healingTakenMod;
        set
        {
            healingTakenMod = value;
            ChangeCombatMod(CombatModIDs.HealingTaken);
        }
    }
    public float HealingOutMod
    {
        get => healingOutMod;
        set
        {
            healingOutMod = value;
            ChangeCombatMod(CombatModIDs.HealingOut);
        }
    }

    // public SyncList<Buff> Buffs => buffs;

    #endregion

    private void Start()
    {
        stunned = 0;
        silenced = 0;
        feared = 0;
        dizzy = 0;
        slow = 1.0f;
        haste = 1.0f;
        damageTakenMod = 1.0f;
        damageOutMod = 1.0f;
        healingTakenMod = 1.0f;
        healingOutMod = 1.0f;
        // if (!isServer)
        // {
        //     buffs.Callback += OnBuffsUpdated;
        // }
    }

    private void Update()
    {
        OnBuffUpdate.Invoke();
    }

    // private void OnBuffsUpdated(SyncList<Buff>.Operation op, int index, Buff oldBuff, Buff newBuff)
    // {
    //     if (op == SyncList<Buff>.Operation.OP_ADD)
    //     {
    //         newBuff.Start();
    //     }
    //     else if (op == SyncList<Buff>.Operation.OP_REMOVEAT)
    //     {
    //         oldBuff.End();
    //     }
    // }

    public void AddBuff(OldBuff.Buff _buffClone)
    {
        Debug.Log("AddBuff");
        var bef = buffs.Count;
        OnBuffUpdate.AddListener(_buffClone.update);
        _buffClone.actor = GetComponent<Actor>();
        buffs.Add(_buffClone);
        Debug.Log(_buffClone.name + "added");
        RpcAddBuff(_buffClone);

    }
    [ClientRpc]
    public void RpcAddBuff(OldBuff.Buff _buffFromServer){
        if(isServer){
            return;
        }
        OnBuffUpdate.AddListener(_buffFromServer.update);
        buffs.Add(_buffFromServer);
        Debug.Log("RpcAddBuff" + _buffFromServer.name + "added");
    }
    [Server]
    public void RemoveBuff(OldBuff.Buff _buff)
    {
        int buffIndex = buffs.FindIndex(x => x == _buff);
        RemoveBuffLogic(buffIndex);
        Debug.Log("Rpc-ing to remove index: " + buffIndex);
        RpcRemoveBuffIndex(buffIndex);
    }
        
    [ClientRpc]
    void RpcRemoveBuffIndex(int hostIndex)
    {
        Debug.Log("Host saying to remove buff index: " + hostIndex);
        if(isServer)
        {
            return; //We did it already
        }
        RemoveBuffLogic(hostIndex);

    }
    public void RemoveBuffLogic(int _buffIndex) // Actual removal logic
    {
        buffs[_buffIndex].OnRemoveFromList();

        var buffRef = buffs[_buffIndex];
        OnBuffUpdate.RemoveListener(buffRef.update);
        buffs.RemoveAt(_buffIndex);
        Destroy(buffRef);
    }
    // [Server]
    // public void RemoveBuff(Buff buff)
    // {
    //     buff.Finished -= HandleBuffFinished;
    //     buff.End();
    //     buffs.Remove(buff);
    // }
    [Server]
    public void RemoveAll()
    {
        foreach(OldBuff.Buff buff in buffs)
        {
            RemoveBuff(buff);
        }
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

    private void ChangeDamageTakenMod()
    {
        var damageTakenChangedEventArgs = new DamageTakenModChangedEventArgs
        {
            eDamageTakenMod = DamageTakenMod
        };
        DamageTakenModChanged?.Invoke(this, damageTakenChangedEventArgs);
    }
    private void ChangeCombatMod(CombatModIDs _combatModID)
    {
        CombatModChangedEventArgs combatModChangedEventArgs = null;
        switch(_combatModID)
        {
            case(CombatModIDs.DamageTaken):
                combatModChangedEventArgs = new CombatModChangedEventArgs
                {
                    eFloat = DamageTakenMod,
                    eCombatModID = _combatModID
                    
                };
                break;
            case(CombatModIDs.DamageOut):
                combatModChangedEventArgs = new CombatModChangedEventArgs
                {
                    eFloat = DamageOutMod,
                    eCombatModID = _combatModID
                    
                };
                break;
            case(CombatModIDs.HealingTaken):
                combatModChangedEventArgs = new CombatModChangedEventArgs
                {
                    eFloat = HealingTakenMod,
                    eCombatModID = _combatModID
                    
                };
                break;
            case(CombatModIDs.HealingOut):
                combatModChangedEventArgs = new CombatModChangedEventArgs
                {
                    eFloat = HealingOutMod,
                    eCombatModID = _combatModID
                    
                };
                break;
            default:
                break;
        }

        CombatModChanged?.Invoke(this, combatModChangedEventArgs);
    }

    /// <summary>
    /// Handles refreshing or stacking a buff if it is already present on the target
    /// </summary>
    /// <returns>True if the buff was refreshed or stacked, false if it was not present</returns>
    // private bool RefreshOrStackBuff(OldBuff.Buff _buff)
    // {
    //     // var buff = buffs.FirstOrDefault(b => b.BuffSO == buffSO);
    //     var index = buffs.FindIndex(b => b == _buff);
    //     if ((index < 0) || (buffs.Count <= index))
    //     {
    //         return false;
    //     }
    //     if (buffs[index].BuffSO.Stackable)
    //     {
    //         buffs[index].stacks += 1;
    //         RpcRefreshOrStackBuff(index);
    //     }
    //     else // Refresh
    //     {
    //         buffs[index].Refresh();
    //         RpcRefreshOrStackBuff(index);
    //     }
    //     return true;
    // }

    // [ClientRpc]
    // private void RpcRefreshOrStackBuff(int _indexFromServer)
    // {
    //     if (isServer)
    //     {
    //         return;
    //     }
    //     if ((_indexFromServer < 0) || (buffs.Count <= _indexFromServer))
    //     {
    //         return;
    //     }

    //     if (buffs[_indexFromServer].BuffSO.Stackable)
    //     {
    //         buffs[_indexFromServer].AddStack();
    //     }
    //     else // Refresh
    //     {
    //         buffs[_indexFromServer].Refresh();
    //     }
    //     return;
    // }

    public void InterruptCast()
    {
        OnInterrupt();
    }
    
    public bool RemoveRandomBuff(Predicate<OldBuff.Buff> _matchExpression)
    {
        List<int> indices = buffs.IndicesThatMatch(_matchExpression);

        if(indices == null){
            return false;
        }

        RemoveBuff(buffs[indices[UnityEngine.Random.Range(0, indices.Count)]]);
        return true;
    }

}
