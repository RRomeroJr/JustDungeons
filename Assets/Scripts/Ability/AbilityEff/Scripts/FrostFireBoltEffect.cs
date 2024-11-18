using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;
using System.IO;
using BuffSystem;
using Mirror;



#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

[System.Serializable]
[CreateAssetMenu(fileName="FrostFireBoltEffect", menuName = "HBCsystem/FrostFireBoltEffect")]
public class FrostFireBoltEffect : AbilityEff_V2
{   
    #if UNITY_EDITOR
    [DidReloadScripts]
    static void OnDidReloadScripts()
    {
        FrostFireBoltEffect aeRef = AssetDatabase.LoadAssetAtPath<FrostFireBoltEffect>(CheckForAsset());
        //Adding buff SO to ability
        AddBuffRefToAE(aeRef);

    }
    
    static void AddBuffRefToAE(FrostFireBoltEffect _aeRef){
        string[] buffGuids = AssetDatabase.FindAssets("Press the Attack");
        bool found = false;
        string path = "";
        foreach(string s in buffGuids){
            found = Path.GetFileName(AssetDatabase.GUIDToAssetPath(s)) == "Press the Attack.asset";
            if(found)
            {   
                // Debug.Log("Buff SO found");
                path = AssetDatabase.GUIDToAssetPath(s);
                break;
            }
        }
        var res = AssetDatabase.LoadAssetAtPath<BuffScriptableObject>(path);
        // if (res != null){
        //     Debug.Log("res wasn't null");
        // }
        // else
        // {
        //     Debug.Log("No buff SO at path: " + path);
        // }
        _aeRef.frostfireboltBuffSO = res;
    }
    static string CheckForAsset()
    {
        // Debug.Log("why unity why");
        string[] guids = AssetDatabase.FindAssets("FrostFireBoltEffect");
        bool found = false;
        string path = "";
        foreach(string s in guids){
            found = Path.GetFileName(AssetDatabase.GUIDToAssetPath(s)) == "FrostFireBoltEffect.asset";
            if(found)
            {
                path = AssetDatabase.GUIDToAssetPath(s);
                Debug.Log("Asset found at: " + path);
                break;
            }
        }
        if (!found)
        {
            Debug.LogError("FrostFireBoltEffect scriptable object not found in project. Creating new one..");
            FrostFireBoltEffect aeRef = CreateInstance(typeof(FrostFireBoltEffect)) as FrostFireBoltEffect;
            string aePath = "Assets/Scripts/Ability/AbilityEff/FindMeTest/FrostFireBoltEffect.asset";
            AssetDatabase.CreateAsset(aeRef, aePath);
            guids = AssetDatabase.FindAssets("FrostFireBoltEffect");
            found = false;
            foreach(string s in guids)
            {
                found = Path.GetFileName(AssetDatabase.GUIDToAssetPath(s)) == "FrostFireBoltEffect.asset";
                if(found)
                {
                    path = AssetDatabase.GUIDToAssetPath(s);
                    Debug.Log("Created asset found at: " + path);
                    break;
                }
            }
            if (!found)
            {
                Debug.LogError("FrostFireBoltEffect still not found. Check " + aePath +"\nIs it there?");

            }
        }
        return path;
    
    }
    #endif
    public int school = -1;
    public ActorStats scaleStat = ActorStats.MainStat;
    public BuffScriptableObject frostfireboltBuffSO;
    public override void OnSendEffect()
    {
        GameObject delivery = Instantiate(Resources.Load<GameObject>("Networked/Missile"));
        // Set the sprite here
        delivery.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/FrostFireBoltSprite");

        //Don't entirely remember what this does but i'll put it here anyway----
        if(targetWP2 != null){
            delivery.transform.position += targetWP2.Value;
            //Debug.Log("targetWP.Value: " + targetWP.Value + "Spawning Missle there");
        }
        else if(caster != null){
            delivery.transform.position += caster.gameObject.transform.position;
        }
        // ----------------------------------------------------------------------

        delivery.GetComponent<AbilityDelivery>().Caster = caster;

        delivery.GetComponent<AbilityDelivery>().abilityEff= this;
        // delivery.GetComponent<AbilityDelivery>().eInstructs = eInstructs;
        
        delivery.GetComponent<AbilityDelivery>().speed = 0.1f; 

        // Normal targeted pattern. Skillshot would be a little different
        delivery.GetComponent<AbilityDelivery>().type = AbilityType.Normal;
        delivery.GetComponent<AbilityDelivery>().Target = target.transformSafe();

        delivery.GetComponent<AbilityDelivery>().ignoreDuration = true;

        NetworkServer.Spawn(delivery);
    }
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        try
        {
            Debug.Log("Boom wow look at he this amazing custom effect!");
            if(caster != null){
                target.damageValue((int)power + (int)(caster.GetStat(scaleStat) * powerScale), fromActor: caster);
            }
            else{
                target.damageValue((int)power);
            }
            _caster.GetComponent<IBuff>().AddBuff(frostfireboltBuffSO);
        }
        catch
        {
            // DebugMsgs(_target, _caster: _caster);
        }
    //    Debug.Log(power.ToString() + " + " + caster.mainStat.ToString() + " * " + powerScale.ToString());
    }
    public FrostFireBoltEffect(string _effectName, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;
        id = _id;
        power = _power;
        school = _school;
    }
    public FrostFireBoltEffect(){}
    public override AbilityEff_V2 clone()
    {
        FrostFireBoltEffect temp_ref = ScriptableObject.CreateInstance(typeof (FrostFireBoltEffect)) as FrostFireBoltEffect;
        copyBase(temp_ref);
        temp_ref.frostfireboltBuffSO = frostfireboltBuffSO;
        // temp_ref.effectName = effectName;
        // temp_ref.id = id;
        // temp_ref.power = power;
        // temp_ref.powerScale = powerScale;
        temp_ref.school = school;
        temp_ref.scaleStat = scaleStat;
        // temp_ref.targetIsSecondary = targetIsSecondary;

        return temp_ref;
    }

    public void DebugMsgs(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        if(!_target)
        {
            Debug.LogError(effectName + ": no _target");
        }
        if(!target)
        {
            Debug.LogError(effectName + ": no effect.target");
        }
        if(!_caster)
        {
            Debug.LogError(effectName + ": no _caster");
        }
        if(!caster)
        {
            Debug.LogError(effectName + ": no effect.caster");
        }
    }
}
