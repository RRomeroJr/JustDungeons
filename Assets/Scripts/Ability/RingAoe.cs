using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
[System.Serializable]
[CreateAssetMenu(fileName="RingAoe", menuName = "HBCsystem/RingAoe")]
public class RingAoe : Aoe
{   
    public float innerCircleRadius;
    public Vector3 innerCircleScale;
    public Vector3 innerCirclePosistion; //relative to parent
    public bool usePosition = false;
    
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null){
        //Debug.Log("Actor " + _caster.getActorName() + ": casting Missile at " + _target.getActorName());
        //Debug.Log("Caster " + _caster.getActorName() + " currently has target " + _caster.target.getActorName());
        Debug.Log(_targetWP == null ? "RingAoe: No targetWP" : ("RingAoe: wp = " + _targetWP.Value.ToString()));
        GameObject delivery = Instantiate(aoePrefab, getWP(_target, _targetWP), Quaternion.identity);
        delivery.GetComponent<AbilityDelivery>().setTarget(_target);
        delivery.GetComponent<AbilityDelivery>().setCaster(_caster);
        delivery.GetComponent<AbilityDelivery>().worldPointTarget = getWP(_target, _targetWP);
        delivery.transform.localScale = prefabScale;
        delivery.transform.GetChild(0).transform.localScale = innerCircleScale;
        if(usePosition){
            delivery.transform.GetChild(0).transform.position = innerCirclePosistion;
        }
        NetworkServer.Spawn(delivery);
        
        /*
            RR: this works bc the prefab already has variables in AbilityDelivery set to what I want.
            Not sure if this is the best way but it seems to work fine
        */
       
    }
    public RingAoe(string _effectName, GameObject _aoePrefab, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;

        aoePrefab = _aoePrefab;

        id = _id;
        power = _power;
        school = _school;
    }
    public RingAoe(){}
    public override AbilityEff clone()
    {
        RingAoe temp_ref = ScriptableObject.CreateInstance(typeof (RingAoe)) as RingAoe;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.prefabScale = prefabScale;
        temp_ref.innerCircleRadius = innerCircleRadius;
        temp_ref.innerCircleScale = innerCircleScale;
        temp_ref.innerCirclePosistion = innerCirclePosistion;
        temp_ref.usePosition = usePosition;
        temp_ref.aoePrefab = aoePrefab;
        

        return temp_ref;
    }
}
