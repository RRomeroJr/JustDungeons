using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
[System.Serializable]
[CreateAssetMenu(fileName="Aoe", menuName = "HBCsystem/Aoe")]
public class Aoe : DeliveryEff
{   
    public int school =-1;
    public Vector3 prefabScale = Vector3.one;
    public GameObject aoePrefab;
    
    
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        
            //Debug.Log("Actor " + _caster.getActorName() + ": casting Missile at " + _target.getActorName());
            //Debug.Log("Caster " + _caster.getActorName() + " currently has target " + _caster.target.getActorName());
            //Debug.Log(_targetWP == null ? "Aoe: No targetWP" : ("Aoe: wp = " + _targetWP.Value.ToString()));
            GameObject delivery = Instantiate(aoePrefab, getWP(_secondaryTarget, _targetWP), Quaternion.identity);
            delivery.GetComponent<AbilityDelivery>().Target = _secondaryTarget.transformSafe();
            delivery.GetComponent<AbilityDelivery>().Caster = _caster;
            delivery.GetComponent<AbilityDelivery>().worldPointTarget = getWP(_secondaryTarget, _targetWP);
            delivery.GetComponent<AbilityDelivery>().eInstructs = eInstructs;
            delivery.transform.localScale = Vector3.Scale(delivery.transform.localScale, prefabScale);
            
            NetworkServer.Spawn(delivery);

            /*
                RR: this works bc the prefab already has variables in AbilityDelivery set to what I want.
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
    public Aoe(){
        targetIsSecondary = true;
        
    }
    public override AbilityEff clone()
    {
        Aoe temp_ref = ScriptableObject.CreateInstance(typeof (Aoe)) as Aoe;
        copyBase(temp_ref);
        
        
        
        temp_ref.prefabScale = prefabScale;
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
    public Vector3 getWP(Actor _target = null, NullibleVector3 _targetWP = null){
        if(_targetWP != null){
            return _targetWP.Value;
            
        }
        else if(_target != null){
            return _target.transform.position;
        }
        else{
            throw new NullReferenceException();
        }
    }
    
    public Vector3 getWP(Transform _target = null, NullibleVector3 _targetWP = null)
    {
        if (_targetWP != null)
        {
            return _targetWP.Value;
            
        }
        else if (_target != null)
        {
            return _target.position;
        }
        else
        {
            throw new NullReferenceException();
        }
    }
}
