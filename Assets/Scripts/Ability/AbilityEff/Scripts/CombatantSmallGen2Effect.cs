using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using OldBuff;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
#endif
[System.Serializable]
// [CreateAssetMenu(fileName="CombatantSmallGen2Effect", menuName = "HBCsystem/CombatantSmallGen2Effect")]
public class CombatantSmallGen2Effect : AbilityEff_V2
{   
    public int school = -1;
    [SerializeField]
    public ActorStats scaleStat = ActorStats.MainStat;
    public ClassResourceType crt;

    #if UNITY_EDITOR
    [DidReloadScripts]
    static void OnDidReloadScripts()
    {
        // var crtAsset = AssetFinderTools.GetAsset(typeof(ClassResourceType), "Simple5points") as ClassResourceType;
        // var creationPath = "Assets/Scripts/Ability/AbilityEff/Combatant/";
        // var SORef = SOAssetTools.MakeOrGetSOAsset(typeof(CombatantSmallGen2Effect), creationPath) as CombatantSmallGen2Effect;
        // if (!Directory.Exists(creationPath))
        // {
        //     Directory.CreateDirectory(creationPath);
        //     Debug.Log("Directory created at: " + creationPath);
        // }
        // SORef.powerScale = 1;
        // if(crtAsset != null)
        // {
        //     SORef.crt = crtAsset;
        // }

    }
    #endif
        public override void OnSendEffect()
    {
        if(HBCTools.checkIfFlank(caster, target))
        {
            powerScale *= 0.03f;
        }
    }
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        try
        {
            if(caster != null){
                target.damageValue((int)power + (int)(caster.GetStat(scaleStat) * powerScale), fromActor: caster);
                caster.restoreResource(crt, 1);
            }
            else{
                target.damageValue((int)power);
            }
        }
        catch
        {
            // DebugMsgs(_target, _caster: _caster);
        }
    //    Debug.Log(power.ToString() + " + " + caster.mainStat.ToString() + " * " + powerScale.ToString());
    }
    public CombatantSmallGen2Effect(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public CombatantSmallGen2Effect(){}
    public override AbilityEff_V2 clone()
    {
        CombatantSmallGen2Effect temp_ref = ScriptableObject.CreateInstance(typeof (CombatantSmallGen2Effect)) as CombatantSmallGen2Effect;
        copyBase(temp_ref);
        temp_ref.crt = crt;
        // temp_ref.effectName = effectName;
        // temp_ref.id = id;
        // temp_ref.power = power;
        // temp_ref.powerScale = powerScale;
        temp_ref.school = school;
        temp_ref.scaleStat = scaleStat;
        // temp_ref.targetIsSecondary = targetIsSecondary;

        return temp_ref;
    }

}
