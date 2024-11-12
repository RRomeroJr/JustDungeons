using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using System.IO;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

namespace OldBuff
{
    public class DizzyBuff : OldBuff.Buff
    {
        public GameObject indicatorPrefab;
        public override void OnTick()
        {
            Debug.Log("Hello this is DizzyBuff OnTick. Aint that something.");
        }
    }
}
