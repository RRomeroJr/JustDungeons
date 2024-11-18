#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Mirror;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEngine.Assertions;
using System.Linq;
public static class AssetFinderTools
{
    /// <summary>
    ///	Returns path of the 1st item of the type and name
    /// </summary>
    public static UnityEngine.Object GetAsset(Type _assetType, string _name)
    {
        // Debug.Log("why unity why");
        string[] guids = AssetDatabase.FindAssets(_name);
        bool found = false;
        string path = null;
        UnityEngine.Object assetRef = null;
        foreach(string s in guids){
            path = AssetDatabase.GUIDToAssetPath(s);
            assetRef = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if(assetRef == null)
            {
                continue;
            }
            if(assetRef.GetType() == _assetType)
            {
                found = true;
                break;
            }
        }
        if(!found)
        {
            // Debug.Log(String.Format("Subclass check. Items to check ({0})", guids.Length));
            foreach(string s in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(s);
                assetRef = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                if(assetRef == null)
                {
                    continue;
                }
                if(assetRef.GetType().IsSubclassOf(_assetType))
                {
                    found = true;
                    break;
                }
            }
        }

        if(found)
        {
            Debug.Log(String.Format("{0} {1} found.", assetRef.GetType().Name, Path.GetFileName(path)));
        }
        else
        {
            Debug.LogError(String.Format("Could not find an asset matching {0} {1} ", _assetType, _name));
        }
        return assetRef;
    }
 
}

#endif