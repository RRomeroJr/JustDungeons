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
// [CreateAssetMenu(fileName="CombatantSmallGen1Effect", menuName = "HBCsystem/CombatantSmallGen1Effect")]
public class CombatantSmallGen1Effect : AbilityEff_V2
{   
    public int school = -1;
    [SerializeField]
    public ActorStats scaleStat = ActorStats.MainStat;
    public ClassResourceType crt;

    #if UNITY_EDITOR
    [DidReloadScripts]
    static void OnDidReloadScripts()
    {
        // var crtAsset = AssetFinderTools.GetAsset(typeof(ClassResourceType), "Simple5points");
        // var creationPath = "Assets/Scripts/Ability/AbilityEff/Combatant/";
        // var SORef = SOAssetTools.MakeOrGetSOAsset(typeof(CombatantSmallGen1Effect), creationPath) as CombatantSmallGen1Effect;
        // if (!Directory.Exists(creationPath))
        // {
        //     Directory.CreateDirectory(creationPath);
        //     Debug.Log("Directory created at: " + creationPath);
        // }
        

    }
    #endif
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
    public CombatantSmallGen1Effect(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public CombatantSmallGen1Effect(){}
    public override AbilityEff_V2 clone()
    {
        CombatantSmallGen1Effect temp_ref = ScriptableObject.CreateInstance(typeof (CombatantSmallGen1Effect)) as CombatantSmallGen1Effect;
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
