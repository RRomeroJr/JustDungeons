using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
[CreateAssetMenu(fileName="Summon", menuName = "HBCsystem/Summon")]
public class Summon : AbilityEff
{
    public int school;
    public GameObject mobPrefab;
	
    public override GameObject startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        try
        {        
            if(mobPrefab == null)
            {
                Debug.LogError(name + ": mobPrefab was null");
            }

            GameObject inst = Instantiate(mobPrefab, targetWP.Value, Quaternion.identity);

            SummonedCreature _sc;
            if(inst.TryGetComponent(out SummonedCreature _comp))
            {
                _sc = _comp;
            }
            else
            {
                _sc = inst.AddComponent<SummonedCreature>();
            }
            _sc.summoner = caster;
            NetworkServer.Spawn(inst);
            return inst;
        }
        catch{ return null; }
    }
    // public override void clientEffect()
    // {
        
    // }
    // public override void buffStartEffect()
    // {
        
    // }
    // public override void buffEndEffect()
    // {
       
    // }
    
    public Summon()
    {
        targetIsSecondary = true;
    }
    public override AbilityEff clone()
    {
        Summon temp_ref = ScriptableObject.CreateInstance(typeof (Summon)) as Summon;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.powerScale = powerScale;
        temp_ref.school = school;
        temp_ref.targetIsSecondary = targetIsSecondary;
        temp_ref.mobPrefab = mobPrefab;
        return temp_ref;
    }
    
}
