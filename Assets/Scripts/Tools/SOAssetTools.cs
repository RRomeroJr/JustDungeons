#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Mirror;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
public static class SOAssetTools
{
    /// <summary>
    ///	Gets the path to a SO asset. Will return null if not found.
    /// </summary>
    public static string GetSOAssetPath(Type _soType)
    {
        if(!_soType.IsSubclassOf(typeof(ScriptableObject)))
        {
            throw new Exception("trying to make an asset from a non-ScriptableObject type");
        }
        // Debug.Log("why unity why");
        string[] guids = AssetDatabase.FindAssets(_soType.Name);
        bool found = false;
        string path = null;
        foreach(string s in guids){
            found = Path.GetFileName(AssetDatabase.GUIDToAssetPath(s)) == _soType.Name + ".asset";
            if(found)
            {
                path = AssetDatabase.GUIDToAssetPath(s);
                Debug.Log("Asset found at: " + path);
                break;
            }
        }

        return path;
    }
    /// <summary>
    ///	Will create the SO asset then return the ref to created SO.
    /// </summary>
    public static string CreateSOAsset(Type _soType, string _creationPath)
    {

        // Debug.LogError(_so.GetType().ToString() + " scriptable object not found in project. Creating new one..");
        var soRef = ScriptableObject.CreateInstance(_soType);
        string soPath = _creationPath + _soType.Name + ".asset";
        AssetDatabase.CreateAsset(soRef, soPath);
        var guids = AssetDatabase.FindAssets(_soType.Name);
        var found = false;
        string path = null;
        foreach(string s in guids)
        {
            found = Path.GetFileName(AssetDatabase.GUIDToAssetPath(s)) == _soType.Name + ".asset";
            if(found)
            {
                path = AssetDatabase.GUIDToAssetPath(s);
                Debug.Log("Created asset found at: " + path);
                break;
            }
        }
        if (!found)
        {
            Debug.LogError(_soType + " still not found. Check " + soPath +"\nIs it there?");

        }

        return path;
    
    }
    public static ScriptableObject MakeOrGetSOAsset(Type _soType, string _creationPath){
        var assetPath = GetSOAssetPath(_soType);
        if(assetPath == null){
            assetPath = CreateSOAsset(_soType, _creationPath);
        }
        
        if(assetPath == null)

        {
            throw new Exception("Could not find or create a " + _soType.Name + " ScirptableObject");
        }
        return AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
    }
    public static ScriptableObject GetSOAsset(Type _soType){
        var assetPath = GetSOAssetPath(_soType);
        
        if(assetPath == null)

        {
            throw new Exception("Could not find a " + _soType.Name + " ScirptableObject");
        }
        return AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
    }
}

#endif