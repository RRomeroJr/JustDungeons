using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
#endif
public class VanguardDeathStrikeBuffEffectUpdateData : BuffUpdateData
{
    public int recentDamage;
    public override void OnWrite(NetworkWriter _nw)
    {
        _nw.WriteInt(recentDamage);
    }
    public override void OnRead(NetworkReader _nr)
    {
        recentDamage = _nr.ReadInt();
    }

}
public class VanguardDeathStrikeBuffEffect : AbilityEff_V2, IBuffEff
{
    public ActorStats scaleStat = ActorStats.MainStat;
    [Header("Buff Variables")]
    public int recentDamage = 77;
    public float? RemainingTimeOverride{get;set;}
    public float? TickRateOverride{get;set;}
    public uint? StacksOverride{get;set;}

    #if UNITY_EDITOR
    [DidReloadScripts]
    static void OnDidReloadScripts()
    {
        var creationPath = "Assets/Scripts/Ability/AbilityEff/Vanguard/ScriptableObjects/";
        var SORef = SOAssetTools.MakeOrGetSOAsset(typeof(VanguardDeathStrikeBuffEffect), creationPath) as VanguardDeathStrikeBuffEffect;
        if (!Directory.Exists(creationPath))
        {
            Directory.CreateDirectory(creationPath);
            Debug.Log("Directory created at: " + creationPath);
        }
        SORef.effectName = SORef.name;
        // SORef.recentDamage = 0;

    }
    #endif
    public override void OnUpdateData(BuffUpdateData _hostBud)
    {
        if(!_hostBud.IsA<VanguardDeathStrikeBuffEffectUpdateData>())
        {
            return;
        }
        var castedMsg = _hostBud as VanguardDeathStrikeBuffEffectUpdateData;
        recentDamage = castedMsg.recentDamage;
        recentDamage = 1000;
        Debug.Log($"Client is updating buffs[{parentBuff.actor.buffHandler.buffs.FindIndex(x => x.abilityEff.IsA<VanguardDeathStrikeBuffEffect>())}] to {castedMsg.recentDamage}");
    }
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {

    }

    public override void buffStartEffect()
    {
        // parentBuff.actor.OnDamageTaken.AddListener(UpdateRecentDamage)
    }
    public override void buffEndEffect()
    {
        // parentBuff.actor.OnDamageTaken.RemoveListener(UpdateRecentDamage)
    }
    void UpdateClientsData()
    {
        // var ubd = new VanguardDeathStrikeBuffEffectUpdateData()
        // {
        //     recentDamage = recentDamage
        // };
        var newVal = (caster.buffHandler.buffs.Find(x => x.abilityEff.IsA<VanguardDeathStrikeBuffEffect>()).abilityEff as VanguardDeathStrikeBuffEffect).recentDamage;
        var ubd = new VanguardDeathStrikeBuffEffectUpdateData()
        {
            recentDamage = newVal
        };
        var ubm = ubd.WrapData();
        ubm.buffIndex = caster.buffHandler.buffs.FindIndex(x => x == this);
        NetworkServer.SendToAll(ubd.WrapData());
    }
    void UpdateRecentDamage(int _val)
    {
        recentDamage += _val;
        UpdateClientsData();
    }
    public override void OnBuffTick()
    {
        // every time the buff ticks we reset the recentDamage
        recentDamage = 0;
        UpdateClientsData();
    }
    public override AbilityEff_V2 clone()
    {
        VanguardDeathStrikeBuffEffect temp_ref = ScriptableObject.CreateInstance(typeof (VanguardDeathStrikeBuffEffect)) as VanguardDeathStrikeBuffEffect;
        copyBase(temp_ref);
        temp_ref.scaleStat = scaleStat;

        return temp_ref;
    }

}