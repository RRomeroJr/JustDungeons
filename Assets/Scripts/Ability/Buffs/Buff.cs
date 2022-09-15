using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName="Buff", menuName = "HBCsystem/Buff")]
public class Buff: ScriptableObject
{
    
    [SerializeField]protected string effectName;
    [SerializeField]protected float duration;
    [SerializeField]protected float tickRate; // for now will be rounded
    [SerializeField]protected GameObject particles;
    protected Func<AbilityEffect, float> powerCalc;
    [SerializeField]protected bool stackable;
    [SerializeField]protected bool refreshable;
    [SerializeField]protected float lastTick = 0.0f; // time since last tick
    [SerializeField]protected float remainingTime = 0.0f;
    [SerializeField]protected bool start = false;
    [SerializeField]protected bool firstFrame = true;
    [SerializeField]protected Actor caster;
    [SerializeField]protected Actor actor; // Actor that this effect is attached to
   
    [SerializeField]protected int id; // Should be a positive unique identifer
    [SerializeField]public uint stacks;
    public List<AbilityEff> effects;
    
    
    public virtual void update(){
        if(firstFrame){
            if(particles !=  null)
                GameObject.Instantiate(particles, actor.gameObject.transform);
            
            firstFrame = false;
            
        }
        else{
            remainingTime -= Time.deltaTime;
            lastTick += Time.deltaTime;
        }
        if(lastTick >= tickRate){ 
            if(particles !=  null){
                GameObject.Instantiate(particles, actor.gameObject.transform);
            }
            OnTick();
            lastTick -= tickRate;
        }
        if(remainingTime <= 0 ){
            List<Buff> list_ref = actor.getBuffs();

            //Find this buff in actor's List<> and remove it
            list_ref.Remove(list_ref.Find(x => this)); //This needs to be tested
        }
    }
    public virtual void OnTick(){
        foreach(AbilityEff eff in effects){
            eff.effectStart(_target: actor, _caster: caster);
        }
    }

    public string getEffectName(){
        return effectName;
    }
    public void setEffectName(string _effectName){
        effectName = _effectName;        
    }
    
    // public float getPower(){
    //     // acessingEvent.Invoke()
    //     if(powerCalc != null){
    //         powerCalc(this);
    //     }
    //     return power;
    // }
    // public void setPower(float _power){
    //     if(canEdit){
    //         power = _power;
    //     }
    // }
    public float getDuration(){
        return duration;
    }
    public void setDuration(float _duration){
        duration = _duration;
    }
    public float getTickRate(){
        return tickRate;
    }
    public void setTickRate(float _tickRate){
        tickRate = _tickRate;
    }
    public GameObject getParticles(){
        return particles;
    }
    public void setParticles(GameObject _particles){
        particles = _particles;
    }
    public float getRemainingTime(){
        return remainingTime;
    }
    public void setRemainingTime(float _remainingTime){
        remainingTime = _remainingTime;  
    }
    public bool getStart(){
        return start;
    }
    public void setStart(bool _start){
        
        start = _start;
      
    }
    public void toggleStart(){
        if(start){
            start = false;
        }
        else{
            start = true;
        }
    }
    
    public Actor getActor(){
        return actor;
    }
    public void setActor(Actor _actor){
        actor = _actor;
    }
    public Actor getCaster(){
        return caster;
    }
    public void setCaster(Actor _caster){
        caster = _caster;
    }
    public int getID(){
        return id;
    }
    public bool isStackable(){
        return stackable;
    }
    public bool isRefreshable(){
        return refreshable;
    }
    public uint getStacks(){
        return stacks;
    }
    public void setStacks(uint _stacks){
        stacks = _stacks;
    }
    public void addStacks(uint amount){
        stacks += amount;
    }
    public void removeStacks(uint amount){
        stacks -= amount;
    }
    public Buff(){
    }
    public Buff(string _effectName, float _duration, float _tickRate = 3.0f, int _id = -1,
                      bool _stackable = false, bool _refreshable = true, uint _stacks = 1, GameObject _particles = null){


        effectName = _effectName;
        duration = _duration;

        remainingTime = duration;

        tickRate = _tickRate;
        id = _id;
        stackable = _stackable;
        refreshable = _refreshable;
        stacks = _stacks;
        particles = _particles;

    }
    public void Init(string _effectName, float _duration, List<AbilityEff> _effects, float _tickRate = 3.0f, int _id = -1,
                      bool _stackable = false, bool _refreshable = true, uint _stacks = 1, GameObject _particles = null){


        effectName = _effectName;
        duration = _duration;
        effects = _effects;

        //remainingTime = duration;

        tickRate = _tickRate;
        id = _id;
        stackable = _stackable;
        refreshable = _refreshable;
        stacks = _stacks;
        particles = _particles;
        foreach (AbilityEff eff in effects){
            eff.parentBuff = this;
        }

    }
    
    public Buff clone(){
        // Creates an editable version of the input Ability Effect

        Buff temp_ref = ScriptableObject.CreateInstance(typeof (Buff)) as Buff;
        
        temp_ref.Init(String.Copy(effectName), duration, effects.cloneEffects(), //<- fix this garbage
         tickRate, id, stackable,refreshable, stacks, particles);
        return temp_ref;
    }
}
