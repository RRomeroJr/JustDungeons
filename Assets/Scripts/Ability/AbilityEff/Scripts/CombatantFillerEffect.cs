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
// [CreateAssetMenu(fileName="CombatantFillerEffect", menuName = "HBCsystem/CombatantFillerEffect")]
public class CombatantFillerEffect : AbilityEff_V2
{   
    public int school = -1;
    [SerializeField]
    public ActorStats scaleStat = ActorStats.MainStat;
    public GameObject aoePrefab;

    #if UNITY_EDITOR
    [DidReloadScripts]
    static void OnDidReloadScripts()
    {
        var creationPath = "Assets/Scripts/Ability/AbilityEff/Combatant/";
        // if (!Directory.Exists(creationPath))
        // {
        //     Directory.CreateDirectory(creationPath);
        //     Debug.Log("Directory created at: " + creationPath);
        // }
        var SORef = SOAssetTools.MakeOrGetSOAsset(typeof(CombatantFillerEffect), creationPath) as CombatantFillerEffect;
        SORef.effectName = SORef.name;

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
        
        }
        catch
        {
        }
    }
    public override AbilityEff_V2 clone()
    {
        CombatantFillerEffect temp_ref = ScriptableObject.CreateInstance(typeof (CombatantFillerEffect)) as CombatantFillerEffect;
        copyBase(temp_ref);
        temp_ref.aoePrefab = aoePrefab;
        temp_ref.school = school;
        temp_ref.scaleStat = scaleStat;

        return temp_ref;
    }

}
