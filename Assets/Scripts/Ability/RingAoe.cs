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
    public Vector2 innerCirclePosistion; //relative to parent
    public bool usePosition = false;
    
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        //Debug.Log("Actor " + _caster.getActorName() + ": casting Missile at " + _target.getActorName());
        //Debug.Log("Caster " + _caster.getActorName() + " currently has target " + _caster.target.getActorName());
        Debug.Log(_targetWP == null ? "RingAoe: No targetWP" : ("RingAoe: wp = " + _targetWP.Value.ToString()));
        GameObject delivery = Instantiate(aoePrefab, getWP(_secondaryTarget, _targetWP), Quaternion.identity);
        delivery.GetComponent<AbilityDelivery>().setTarget(_secondaryTarget);
        delivery.GetComponent<AbilityDelivery>().setCaster(_caster);
        delivery.GetComponent<AbilityDelivery>().worldPointTarget = getWP(_secondaryTarget, _targetWP);
        delivery.GetComponent<AbilityDelivery>().innerCircleRadius = innerCircleRadius;
        delivery.transform.localScale = prefabScale;
        delivery.transform.GetChild(0).transform.localScale = Vector3.one * ( ( 2* innerCircleRadius) / prefabScale.x);
        if(usePosition){
            delivery.transform.GetChild(0).transform.position = (Vector2)delivery.transform.position + innerCirclePosistion;
        }
            //delivery.GetComponent<AbilityDelivery>().safeZoneCenter = innerCirclePosistion;
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
    public RingAoe(){
        targetIsSecondary = true;
    }
    public override AbilityEff clone()
    {
        RingAoe temp_ref = ScriptableObject.CreateInstance(typeof (RingAoe)) as RingAoe;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.prefabScale = prefabScale;
        temp_ref.innerCircleRadius = innerCircleRadius;
        
        temp_ref.innerCirclePosistion = innerCirclePosistion;
        temp_ref.usePosition = usePosition;
        temp_ref.aoePrefab = aoePrefab;
        temp_ref.targetIsSecondary = targetIsSecondary;
        

        return temp_ref;
    }
}
