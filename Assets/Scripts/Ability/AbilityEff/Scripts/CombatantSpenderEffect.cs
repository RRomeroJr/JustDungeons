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
public class CombatantSpenderEffect : AbilityEff_V2
{   
    public int school = -1;
    [SerializeField]
    public ActorStats scaleStat = ActorStats.MainStat;

    #if UNITY_EDITOR
    [DidReloadScripts]
    static void OnDidReloadScripts()
    {
        var creationPath = "Assets/Scripts/Ability/AbilityEff/Combatant/";
        var SORef = SOAssetTools.MakeOrGetSOAsset(typeof(CombatantSpenderEffect), creationPath) as CombatantSpenderEffect;
        if (!Directory.Exists(creationPath))
        {
            Directory.CreateDirectory(creationPath);
            Debug.Log("Directory created at: " + creationPath);
        }
        SORef.powerScale = 2.25f;

    }
#endif
    public override void OnSendEffect()
    {
        if(HBCTools.checkIfBehind(caster, target))
        {
            powerScale *= 0.03f;
        }
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
    public CombatantSpenderEffect(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public CombatantSpenderEffect(){}
    public override AbilityEff_V2 clone()
    {
        CombatantSpenderEffect temp_ref = ScriptableObject.CreateInstance(typeof (CombatantSpenderEffect)) as CombatantSpenderEffect;
        copyBase(temp_ref);
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
