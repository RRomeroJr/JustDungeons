using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
[System.Serializable]
[CreateAssetMenu(fileName="Aoe", menuName = "HBCsystem/Aoe")]
public class Aoe : AbilityEff
{   
    public int school;
    public GameObject aoePrefab;
    public override void effectStart(Actor _target = null, Vector3? _targetWP = null, Actor _caster = null){
        //Debug.Log("Actor " + _caster.getActorName() + ": casting Missle at " + _target.getActorName());
        //Debug.Log("Caster " + _caster.getActorName() + " currently has target " + _caster.target.getActorName());
        GameObject delivery = Instantiate(aoePrefab, _caster.gameObject.transform.position, _caster.gameObject.transform.rotation);
        delivery.GetComponent<AbilityDelivery>().setTarget(_target);
        delivery.GetComponent<AbilityDelivery>().setCaster(_caster);
        NetworkServer.Spawn(delivery);
        
        /*
            RR: this works bc the prefab already has variable set to what I want.
            Not sure if this is the best way but it seems to work fine
        */
       
    }
    public Aoe(string _effectName, GameObject _aoePrefab, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;

        aoePrefab = _aoePrefab;

        id = _id;
        power = _power;
        school = _school;
    }
    public Aoe(){}
    public override AbilityEff clone()
    {
        Aoe temp_ref = ScriptableObject.CreateInstance(typeof (Aoe)) as Aoe;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.aoePrefab = aoePrefab;
        

        return temp_ref;
    }
}
