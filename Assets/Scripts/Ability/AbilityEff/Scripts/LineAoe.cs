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
        {        //Debug.Log("Ring Aoe start effect");
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
        abilityDelivery.worldPointTarget = _targetWP.Value;

        //  A LineAoe with no caster doesn't make much sense to me
        //  Feel like you would just use a normal aoe
        //  vv so I'll leave this unsafe vv
        abilityDelivery.transform.position = _caster.transform.position;

        abilityDelivery.eInstructs = eInstructs;
        abilityDelivery.type = AbilityType.LineAoe;
        if (_target != null)
        {
            abilityDelivery.transform.right = Vector3.Normalize(_target.position - abilityDelivery.transform.position);
        }
        else
        {
            abilityDelivery.transform.right = Vector3.Normalize(_targetWP.Value - abilityDelivery.transform.position);
        }
        NetworkServer.Spawn(ability);
        }
        catch{}
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
