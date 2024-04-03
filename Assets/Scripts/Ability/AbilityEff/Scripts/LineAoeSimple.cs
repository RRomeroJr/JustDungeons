using System.Collections.Generic;
using Mirror;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LineAoeSimple", menuName = "HBCsystem/LineAoeSimple")]
public class LineAoeSimple : Aoe
{
    public float Length;

    public override GameObject startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        try
        {        //Debug.Log("Ring Aoe start effect");
        //Debug.Log("Actor " + _caster.getActorName() + ": casting Missile at " + _target.getActorName());
        //Debug.Log("Caster " + _caster.getActorName() + " currently has target " + _caster.target.getActorName());
        //Debug.Log(_targetWP == null ? "RingAoe: No targetWP" : ("RingAoe: wp = " + _targetWP.Value.ToString()));
        GameObject ability = Instantiate(aoePrefab, getWP(_secondaryTarget, _targetWP), Quaternion.identity);

        AbilityDelivery abilityDelivery = ability.GetComponent<AbilityDelivery>();
        abilityDelivery.Target = _secondaryTarget.transformSafe();
        abilityDelivery.Caster = _caster;
        abilityDelivery.worldPointTarget = getWP(_target.GetComponentSafe<Actor>(), _targetWP);
        abilityDelivery.transform.position = getWP(_caster, targetWP2);
        abilityDelivery.transform.right = abilityDelivery.worldPointTarget - abilityDelivery.transform.position;
        ability.transform.localScale = new Vector3(Length, prefabScale.y, prefabScale.z);
        //Debug.Log(delivery.transform.localScale + "|" + length );
        abilityDelivery.eInstructs = eInstructs;
        NetworkServer.Spawn(ability);
            return ability;
        }
        catch{ return null; }
    }

    public LineAoeSimple(string _effectName, GameObject _aoePrefab, int _id = -1, float _power = 0, int _school = -1)
    {
        effectName = _effectName;
        aoePrefab = _aoePrefab;
        id = _id;
        power = _power;
        school = _school;
    }

    public LineAoeSimple()
    {
        targetIsSecondary = true;
    }

    public override AbilityEff clone()
    {
        LineAoeSimple temp_ref = ScriptableObject.CreateInstance(typeof(LineAoeSimple)) as LineAoeSimple;
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
