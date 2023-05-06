using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Multiboss: MonoBehaviour
{
    /*
        This will be put on an actor. It will search for other actors with
        this component and store thier refereces in a list. 
    */
    public bool searchingForPartners;
    public List<Actor> partners;
    public MultibossCoordinator multibossCoordinator;
    public bool comboReady = false;
    public bool comboFinished = false;
    public int lastCombo;
    [SerializeField]protected int comboMax;
    public abstract IEnumerator SearchForPartners();
    public virtual void OnFindPartners(){
        Debug.Log("Multiboss: Partners found!");
        
        //Gaurd clause: if you are not the first to reach this, then get the coordinator ref
        bool spawnCoordinator = true;
        foreach(Actor a in partners)
        {   
            if(a.GetComponent<Multiboss>().multibossCoordinator != null)
            {
                multibossCoordinator = a.GetComponent<Multiboss>().multibossCoordinator;
                spawnCoordinator = false;
                break;
            }
        }
        
        // if you made it here and spawnCoordinator then you are are the 1st and need to spawn it
        if(spawnCoordinator)
        {
            GameObject go = new GameObject(GetType()+ "_MutibossCoordinator");
            go.AddComponent<MultibossCoordinator>();
            multibossCoordinator = go.GetComponent<MultibossCoordinator>();
        }
        
        multibossCoordinator.comboEvent.AddListener(readyCombo);
        
    }
    
    public virtual void SetComboReady(){
        
        comboReady = true;

    }
    public void readyCombo(int _eventInput){
        Debug.Log(gameObject.name + " combo readied(" + _eventInput +")");
        lastCombo = _eventInput;
        comboReady = true;
    }
    public virtual bool AllPartnersComboReady(){
        
        if(partners.Count < 0){
            return false;
        }
        if(!comboReady){
            return false;
        }
        foreach(Actor partner in partners){
            if(partner.Health > 0.0f){
                if(partner.GetComponent<Multiboss>().comboReady == false){
                    return false;
                }
            }
            
        }
        return true;
    }
    public virtual bool AllPartnersComboFinished(){
        
        if(partners.Count < 0){
            return false;
        }
        if(!comboFinished){
            return false;
        }
        foreach(Actor partner in partners){
            if(partner.Health > 0.0f){
                if(partner.GetComponent<Multiboss>().comboFinished == false){
                    return false;
                }
            }
            
        }
        return true;
    }
    public virtual void SetAllPartnersComboFinished(bool _input){
        if(partners.Count < 0){
            return;
        }

        foreach(Actor partner in partners){
            partner.GetComponent<Multiboss>().comboFinished = _input;
        }
    }
    public void incrementCombo(){
        if(lastCombo + 1 <= comboMax){
            lastCombo++;
        }
        else{
            lastCombo = 0;
        }
    }
    public bool setComboNumber(int _in){
        if((-1 <= _in)&&(_in <= comboMax)){
            lastCombo = _in;
            return true;
        }
        return false;
    }
    public int GetComboNumber(){
        return lastCombo;
    }
    public bool isPartner(Actor _actorToCheck)
    {
        foreach(Actor partner in partners)
        {
            if(partner == _actorToCheck)
                return true;
        }

        return false;
    }
    public void ChainAggro(Actor _actorStartedAggro){
        if(isPartner(_actorStartedAggro) == false)
            return;

        GetComponent<Actor>().CheckStartCombatWith(_actorStartedAggro.target);
    }
}
