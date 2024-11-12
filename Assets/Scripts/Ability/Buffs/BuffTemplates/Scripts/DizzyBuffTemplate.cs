using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using System.IO;
using System.Security.Cryptography;



#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

namespace OldBuff
{
    [Serializable]
    // [CreateAssetMenu(fileName = "BuffTemplate", menuName = "HBCsystem/BuffTemplate")]
    public class DizzyBuffTemplate : BuffTemplate
    {
        public GameObject indicatorPrefab;
        #if UNITY_EDITOR
        [DidReloadScripts]
        static void OnDidReloadScripts()
        {
            var assetPath = SOAssetTools.GetSOAssetPath(typeof(DizzyBuffTemplate));
            if(assetPath == null){
                assetPath = SOAssetTools.CreateSOAsset(typeof(DizzyBuffTemplate), "Assets/Scripts/Ability/Buffs/BuffTemplates/Templates/");
            }
            if(assetPath == null)

            {
                throw new Exception("Could not find or create a " + typeof(DizzyBuffTemplate).ToString() + " ScirptableObject");
            }

        }
        #endif
        DizzyBuffTemplate()
        {
            effectName = "Dizzy";
        }

        public override OldBuff.Buff CreateBuff()
        {
            OldBuff.Buff fromBuffBase = CreateBuffBase(typeof(DizzyBuff));
            DizzyBuff temp = fromBuffBase as DizzyBuff;
            // temp.indicatorPrefab = indicatorPrefab;
            return temp;
        }
    }

}
