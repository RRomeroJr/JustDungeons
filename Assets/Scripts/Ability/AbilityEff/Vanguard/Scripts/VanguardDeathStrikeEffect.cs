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
public class VanguardDeathStrikeEffect : AbilityEff_V2
{
    public GameObject deliveryPrefab0;
    public ActorStats scaleStat = ActorStats.MainStat;

    #if UNITY_EDITOR
    [DidReloadScripts]
    static void OnDidReloadScripts()
    {
        var creationPath = "Assets/Scripts/Ability/AbilityEff/Vanguard/ScriptableObjects/";
        var SORef = SOAssetTools.MakeOrGetSOAsset(typeof(VanguardDeathStrikeEffect), creationPath) as VanguardDeathStrikeEffect;
        if (!Directory.Exists(creationPath))
        {
            Directory.CreateDirectory(creationPath);
            Debug.Log("Directory created at: " + creationPath);
        }
        SORef.effectName = SORef.name;
        // SORef.recentDamage = 0;

    }
    #endif
    public override void OnSendEffect()
    {

        GameObject deliveryPrefab0 = Instantiate(Resources.Load<GameObject>("Networked/Missile"));

        //Where does this thing start?
        if(targetWP2 != null){
            deliveryPrefab0.transform.position += targetWP2.Value;
            //Debug.Log("targetWP.Value: " + targetWP.Value + "Spawning Missle there");
        }
        else if(caster != null)
        {
            deliveryPrefab0.transform.position += caster.gameObject.transform.position;
        }
        // ----------------------------------------------------------------------

        deliveryPrefab0.GetComponent<AbilityDelivery>().Caster = caster;

        deliveryPrefab0.GetComponent<AbilityDelivery>().abilityEff = this;

        deliveryPrefab0.GetComponent<AbilityDelivery>().speed = 0.069f;

        deliveryPrefab0.GetComponent<AbilityDelivery>().type = AbilityType.Normal;
        deliveryPrefab0.GetComponent<AbilityDelivery>().Target = target.transformSafe();

        deliveryPrefab0.GetComponent<AbilityDelivery>().ignoreDuration = true;
        deliveryPrefab0.GetComponent<AbilityDelivery>().duration = 5;
        deliveryPrefab0.GetComponent<AbilityDelivery>().onlyHitTarget = true;



        NetworkServer.Spawn(deliveryPrefab0);
    }
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        try
        {
            if(caster != null)
            {
                target.damageValue((int)(power + (caster.GetStat(scaleStat) * powerScale)), fromActor: caster);
                var passiveBuff = caster.GetComponent<BuffHandler_V3>().buffs.Find(x => x.abilityEff.IsA<VanguardDeathStrikeBuffEffect>()).abilityEff as VanguardDeathStrikeBuffEffect;
                caster.restoreValue((int)(passiveBuff.recentDamage * 0.25f));
            }
            else
            {
                target.damageValue((int)power);
            }
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }

    }

    public override void buffStartEffect()
    {
        // parentBuff.actor.OnDamageTaken.AddListener(UpdateRecentDamage)
    }
    public override void buffEndEffect()
    {
        // parentBuff.actor.OnDamageTaken.RemoveListener(UpdateRecentDamage)
    }
    public override AbilityEff_V2 clone()
    {
        VanguardDeathStrikeEffect temp_ref = ScriptableObject.CreateInstance(typeof(VanguardDeathStrikeEffect)) as VanguardDeathStrikeEffect;
        copyBase(temp_ref);
        temp_ref.deliveryPrefab0 = deliveryPrefab0;
        temp_ref.scaleStat = scaleStat;

        return temp_ref;
    }

}