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
    public bool isSkillshot;
    public float speed;
    public bool ignoreDuration = true;
    public float duration;

    
    public override void startEffect(Transform _target = null, NullibleVector3 _targetWP = null, Actor _caster = null, Actor _secondaryTarget = null)
    {
        try
        {        //Debug.Log("Actor " + _caster.getActorName() + ": casting Missile at " + _target.getActorName());
        //Debug.Log("Caster " + _caster.getActorName() + " currently has target " + _caster.target.getActorName());
        // Debug.Log("startEffect Missile");
        GameObject delivery;
        //_targetWP is going to be the spawn and targetWP2 the moveVect
        
        
        delivery = Instantiate(misslePrefab);
        if(targetWP2 != null){
            delivery.transform.position += targetWP2.Value;
//            Debug.Log("targetWP.Value: " + targetWP.Value + "Spawning Missle there");
        }
        else if(_caster != null){
            delivery.transform.position  += _caster.gameObject.transform.position;
        }
        
        
        
        delivery.GetComponent<AbilityDelivery>().Caster = _caster;
        delivery.GetComponent<AbilityDelivery>().eInstructs = eInstructs;
        delivery.GetComponent<AbilityDelivery>().speed = speed; 
        if(isSkillshot){
            
            delivery.GetComponent<AbilityDelivery>().worldPointTarget = getWP(_secondaryTarget, targetWP);
            
            //Debug.Log("setting delivery to 1");
            delivery.GetComponent<AbilityDelivery>().type = 1;
        }
        else{
            delivery.GetComponent<AbilityDelivery>().type = 0;
            delivery.GetComponent<AbilityDelivery>().Target = _secondaryTarget.transformSafe();
        }
        if(!ignoreDuration){
            delivery.GetComponent<AbilityDelivery>().ignoreDuration = ignoreDuration;
            delivery.GetComponent<AbilityDelivery>().duration = duration;
        }
        
        
        
        NetworkServer.Spawn(delivery);
        
        /*
            RR: this works bc the prefab already has variable set to what I want.
            Not sure if this is the best way but it seems to work fine
        */
        }
        catch{}
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
        copyBase(temp_ref);
        temp_ref.misslePrefab = misslePrefab;
        temp_ref.eInstructs = new List<EffectInstruction>();

        temp_ref.isSkillshot = isSkillshot;
        temp_ref.speed = speed;
        temp_ref.ignoreDuration = ignoreDuration;
        temp_ref.duration = duration;
        if(targetWP2 != null){
            temp_ref.targetWP2 = new NullibleVector3(targetWP2.Value);
        }
        
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
            Debug.LogError("Missile.getWP() count find a wp. passing (0,0,0)");
            return Vector3.zero;
        }
    }
}
