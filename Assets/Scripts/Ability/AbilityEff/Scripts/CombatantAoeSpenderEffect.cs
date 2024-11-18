using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using OldBuff;
using Mirror;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
#endif
[System.Serializable]
// [CreateAssetMenu(fileName="CombatantAoeSpenderEffect", menuName = "HBCsystem/CombatantAoeSpenderEffect")]
public class CombatantAoeSpenderEffect : AbilityEff_V2
{   
    public int school = -1;
    [SerializeField]
    public ActorStats scaleStat = ActorStats.MainStat;
    public GameObject aoePrefab;

    #if UNITY_EDITOR
    [DidReloadScripts]
    static void OnDidReloadScripts()
    {
        // var aoePrefabRef = AssetFinderTools.GetAsset(typeof(GameObject), "CircleAoe") as GameObject;
        var creationPath = "Assets/Scripts/Ability/AbilityEff/Combatant/";
        // if (!Directory.Exists(creationPath))
        // {
        //     Directory.CreateDirectory(creationPath);
        //     Debug.Log("Directory created at: " + creationPath);
        // }
        var SORef = SOAssetTools.MakeOrGetSOAsset(typeof(CombatantAoeSpenderEffect), creationPath) as CombatantAoeSpenderEffect;
        SORef.effectName = SORef.name;
        // SORef.powerScale = 2.25f;
        // if (aoePrefabRef != null)
        // {
        //     SORef.aoePrefab = aoePrefabRef;
        // }
    }
#endif
    public override void OnSendEffect()
    {
        GameObject delivery = Instantiate(aoePrefab);
        // Set the sprite here
        // delivery.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/FrostFireBoltSprite");

        //Where is this thing spawning/ starting from?
        if(targetWP2 != null){
            delivery.transform.position += targetWP2.Value;
            //Debug.Log("targetWP.Value: " + targetWP.Value + "Spawning Missle there");
        }
        else if(caster != null){
            delivery.transform.position += caster.gameObject.transform.position;
        }
        // else: then it's not my problem
        // ----------------------------------------------------------------------

        delivery.GetComponent<AbilityDelivery>().Caster = caster;

        delivery.GetComponent<AbilityDelivery>().abilityEff = this;

        delivery.GetComponent<AbilityDelivery>().speed = 0.0f;

        delivery.GetComponent<AbilityDelivery>().type = AbilityType.Aoe;

        delivery.GetComponent<AbilityDelivery>().ignoreDuration = false;
        delivery.GetComponent<AbilityDelivery>().duration = 0.167f;
        NetworkServer.Spawn(delivery);
    }
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        try
        {
            // Not sure whether to put the resouce cost here or leave it handled by the hanlder
            var amt = (int)power + (int)(caster.GetStat(scaleStat) * powerScale);
            target.damageValue(amt, fromActor: caster);
        }
        catch
        {
            // DebugMsgs(_target, _caster: _caster);
        }
    //    Debug.Log(power.ToString() + " + " + caster.mainStat.ToString() + " * " + powerScale.ToString());
    }
    public CombatantAoeSpenderEffect(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public CombatantAoeSpenderEffect(){}
    public override AbilityEff_V2 clone()
    {
        CombatantAoeSpenderEffect temp_ref = ScriptableObject.CreateInstance(typeof (CombatantAoeSpenderEffect)) as CombatantAoeSpenderEffect;
        copyBase(temp_ref);
        temp_ref.aoePrefab = aoePrefab;
        temp_ref.school = school;
        temp_ref.scaleStat = scaleStat;

        return temp_ref;
    }

}
