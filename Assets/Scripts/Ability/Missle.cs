using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
[System.Serializable]
[CreateAssetMenu(fileName="Missle", menuName = "HBCsystem/Missle")]
public class Missle : AbilityEff
{   
    public int school;
    public GameObject misslePrefab;
    public override void effectStart(Actor _target = null, Vector3? _targetWP = null, Actor _caster = null){
        //Debug.Log("Actor " + _caster.getActorName() + ": casting Missle at " + _target.getActorName());
        //Debug.Log("Caster " + _caster.getActorName() + " currently has target " + _caster.target.getActorName());
        GameObject delivery = Instantiate(misslePrefab, _caster.gameObject.transform.position, _caster.gameObject.transform.rotation);
        delivery.GetComponent<AbilityDelivery>().setTarget(_target);
        delivery.GetComponent<AbilityDelivery>().setCaster(_caster);
        NetworkServer.Spawn(delivery);
        
        /*
            RR: this works bc the prefab already has variable set to what I want.
            Not sure if this is the best way but it seems to work fine
        */
       
    }
    public Missle(string _effectName, GameObject _misslePrefab, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;

        misslePrefab = _misslePrefab;

        id = _id;
        power = _power;
        school = _school;
    }
    public Missle(){}
    public override AbilityEff clone()
    {
        Missle temp_ref = ScriptableObject.CreateInstance(typeof (Missle)) as Missle;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.misslePrefab = misslePrefab;
        

        return temp_ref;
    }
}
