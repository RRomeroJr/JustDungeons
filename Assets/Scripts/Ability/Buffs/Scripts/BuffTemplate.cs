using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using Mirror;



#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

namespace OldBuff
{
    [Serializable]
    // [CreateAssetMenu(fileName = "BuffTemplate", menuName = "HBCsystem/BuffTemplate")]
    public abstract class BuffTemplate : ScriptableObject
    {
        [SerializeField] public string effectName;
        [SerializeField] public float duration;
        [SerializeField] public float tickRate; // for now will be rounded
        [SerializeField] public GameObject particles;

        [SerializeField] public bool stackable = false;
    [SerializeField] public bool refreshable = true;
        [SerializeField] public uint stacks = 1;

        public List<EffectInstruction> eInstructs;
        [SerializeField] public List<UnityEvent<Buff, EffectInstruction>> onCastHooks;
        [SerializeField] public UnityEvent<Buff, EffectInstruction> onHitHooks;
        [SerializeField] public List<Ability_V2> MakeGlow;
        
        [SerializeField] public List<GlowCheck> GlowChecks;
        public abstract OldBuff.Buff CreateBuff();
        public OldBuff.Buff CreateBuffBase(Type _newBuffsType)
        {
            // Creates an editable version of the input Ability Effect

            var temp_ref = CreateInstance(_newBuffsType) as OldBuff.Buff;
            if(temp_ref == null)
            {
                throw new Exception("Something went wrong in the creation of buff for template: " + name);
            }
            temp_ref.name = name + " (clone)";
            temp_ref.duration = duration;
            temp_ref.eInstructs = eInstructs.cloneInstructs();
            temp_ref.tickRate = tickRate;
            temp_ref.stackable = stackable;
            temp_ref.refreshable = refreshable;
            temp_ref.stacks = stacks;
            temp_ref.particles = particles;
            temp_ref.onCastHooks = onCastHooks;
            temp_ref.onHitHooks = onHitHooks;
            temp_ref.MakeGlow = MakeGlow;
            temp_ref.GlowChecks = GlowChecks;
            // Debug.Log(temp_ref == null? "base temp_ref IS NULL" : "got something from base: " + temp_ref.name);
            return temp_ref;
        }
    }
}
