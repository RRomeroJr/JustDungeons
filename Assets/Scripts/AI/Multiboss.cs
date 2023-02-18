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
    public abstract void OnFindPartners();
    
    public virtual void SetComboReady(){
        
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
}
