using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
[System.Serializable]
[CreateAssetMenu(fileName="Missile", menuName = "HBCsystem/Missile")]
public class Missile : DeliveryEff
{   
    public int school = -1;
    public GameObject misslePrefab;
    

    
    public override void startEffect(Actor _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null){
        //Debug.Log("Actor " + _caster.getActorName() + ": casting Missile at " + _target.getActorName());
        //Debug.Log("Caster " + _caster.getActorName() + " currently has target " + _caster.target.getActorName());
        GameObject delivery = Instantiate(misslePrefab, _caster.gameObject.transform.position, _caster.gameObject.transform.rotation);
        delivery.GetComponent<AbilityDelivery>().setTarget(_secondaryTarget);
        delivery.GetComponent<AbilityDelivery>().setCaster(_caster);
        delivery.GetComponent<AbilityDelivery>().eInstructs = eInstructs;
        NetworkServer.Spawn(delivery);
        
        /*
            RR: this works bc the prefab already has variable set to what I want.
            Not sure if this is the best way but it seems to work fine
        */
       
    }
    public Missile(string _effectName, GameObject _misslePrefab, int _id = -1, float _power = 0, int _school = -1){
        effectName = _effectName;

        misslePrefab = _misslePrefab;

        id = _id;
        power = _power;
        school = _school;
    }
    public Missile(){
        targetIsSecondary = true;
    }
    public override AbilityEff clone()
    {
        //Debug.Log("Missile clone called");
        Missile temp_ref = ScriptableObject.CreateInstance(typeof (Missile)) as Missile;
        temp_ref.effectName = effectName;
        temp_ref.id = id;
        temp_ref.power = power;
        temp_ref.misslePrefab = misslePrefab;
        temp_ref.eInstructs = new List<EffectInstruction>();
        temp_ref.targetIsSecondary = targetIsSecondary;
        foreach (EffectInstruction eI in eInstructs){
            temp_ref.eInstructs.Add(eI.clone());
        }
    
        return temp_ref;
    }
}
