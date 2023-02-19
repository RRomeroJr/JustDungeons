using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
[System.Serializable]
[CreateAssetMenu(fileName="LineAoe", menuName = "HBCsystem/LineAoe")]
public class LineAoe : Aoe
{   
    public float length;
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        //Debug.Log("Ring Aoe start effect");
        //Debug.Log("Actor " + _caster.getActorName() + ": casting Missile at " + _target.getActorName());
        //Debug.Log("Caster " + _caster.getActorName() + " currently has target " + _caster.target.getActorName());
        //Debug.Log(_targetWP == null ? "RingAoe: No targetWP" : ("RingAoe: wp = " + _targetWP.Value.ToString()));
        GameObject delivery = Instantiate(aoePrefab, getWP(_secondaryTarget, _targetWP), Quaternion.identity);
        delivery.GetComponent<AbilityDelivery>().setTarget(_secondaryTarget);
        delivery.GetComponent<AbilityDelivery>().setCaster(_caster);
        delivery.GetComponent<AbilityDelivery>().worldPointTarget = getWP(_target, _targetWP);
        delivery.GetComponent<AbilityDelivery>().transform.position  = getWP(_caster, targetWP2);
        delivery.transform.localScale = new Vector3(length, prefabScale.y, prefabScale.z);
        //Debug.Log(delivery.transform.localScale + "|" + length );
        delivery.GetComponent<AbilityDelivery>().eInstructs = eInstructs;
        NetworkServer.Spawn(delivery);
        
       
    }
    public LineAoe(string _effectName, GameObject _aoePrefab, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;

        aoePrefab = _aoePrefab;

        id = _id;
        power = _power;
        school = _school;
    }
    public LineAoe(){
        targetIsSecondary = true;
    }
    public override AbilityEff clone()
    {
        LineAoe temp_ref = ScriptableObject.CreateInstance(typeof (LineAoe)) as LineAoe;
        copyBase(temp_ref);
        temp_ref.prefabScale = prefabScale;
        temp_ref.length = length;
        temp_ref.aoePrefab = aoePrefab;
        if(targetWP2 != null){
            temp_ref.targetWP2 = new NullibleVector3(targetWP2.Value);
        }
        temp_ref.eInstructs = new List<EffectInstruction>();
        foreach (EffectInstruction eI in eInstructs){
            temp_ref.eInstructs.Add(eI.clone());
        }

        return temp_ref;
    }
}
