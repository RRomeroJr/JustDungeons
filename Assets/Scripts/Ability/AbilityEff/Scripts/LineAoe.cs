using System.Collections.Generic;
using Mirror;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LineAoe", menuName = "HBCsystem/LineAoe")]
public class LineAoe : Aoe
{
    public float Length;

    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        try
        {   //Debug.Log("Ring Aoe start effect");
            //Debug.Log("Actor " + _caster.getActorName() + ": casting Missile at " + _target.getActorName());
            //Debug.Log("Caster " + _caster.getActorName() + " currently has target " + _caster.target.getActorName());
            //Debug.Log(_targetWP == null ? "RingAoe: No targetWP" : ("RingAoe: wp = " + _targetWP.Value.ToString()));
            GameObject ability = Instantiate(aoePrefab, _caster.transform.position, Quaternion.identity);

            if (ability.TryGetComponent(out BeamBuilder beam))
            {
                beam.Length = Length;
            }

            AbilityDelivery abilityDelivery = ability.GetComponent<AbilityDelivery>();
            abilityDelivery.Target = _target;
            abilityDelivery.Caster = _caster;
            if (_targetWP != null)
            {
                abilityDelivery.worldPointTarget = _targetWP.Value;
            }
            abilityDelivery.eInstructs = eInstructs;
            abilityDelivery.type = AbilityType.LineAoe;
            NetworkServer.Spawn(ability);
        }
        catch { }
    }

    public LineAoe(string _effectName, GameObject _aoePrefab, int _id = -1, float _power = 0, int _school = -1)
    {
        effectName = _effectName;
        aoePrefab = _aoePrefab;
        id = _id;
        power = _power;
        school = _school;
    }

    public LineAoe()
    {
        targetIsSecondary = true;
    }

    public override AbilityEff clone()
    {
        LineAoe temp_ref = ScriptableObject.CreateInstance(typeof(LineAoe)) as LineAoe;
        copyBase(temp_ref);
        temp_ref.prefabScale = prefabScale;
        temp_ref.Length = Length;
        temp_ref.aoePrefab = aoePrefab;
        if (targetWP2 != null)
        {
            temp_ref.targetWP2 = new NullibleVector3(targetWP2.Value);
        }
        temp_ref.eInstructs = new List<EffectInstruction>();
        foreach (EffectInstruction eI in eInstructs)
        {
            temp_ref.eInstructs.Add(eI.clone());
        }

        return temp_ref;
    }
}
