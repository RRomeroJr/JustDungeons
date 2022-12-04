using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName="Buff", menuName = "HBCsystem/Buff")]
public class Buff: ScriptableObject
{
    
    [SerializeField]public string effectName;
    [SerializeField]public float duration;
    [SerializeField]public float tickRate; // for now will be rounded
    [SerializeField]public GameObject particles;
    
    [SerializeField]public bool stackable;
    [SerializeField]public bool refreshable;
    [SerializeField]public float lastTick = 0.0f; // time since last tick
    [SerializeField]public float remainingTime = 0.0f;
    [SerializeField]public bool start = false;
    [SerializeField]public bool firstFrame = true;
    [SerializeField]public Actor caster;
    [SerializeField]public Actor actor; // Actor that this effect is attached to
    [SerializeField]public Actor target;
    [SerializeField]public int id = -1; // Should be a positive unique identifer
    [SerializeField]public uint stacks = 1;
    
    public List<EffectInstruction> eInstructs;
    [SerializeField]public List<UnityEvent<Buff, EffectInstruction>> onCastHooks;
    [SerializeField]public UnityEvent<Buff, EffectInstruction> onHitHooks;
    
    
    
    
    public virtual void update(){
        if(firstFrame){
            if(particles !=  null)
                GameObject.Instantiate(particles, actor.gameObject.transform);
            
            firstFrame = false;
            //Debug.Log(effectName + "onStart()");
            onStart();
            
        }
        else{
            if(remainingTime > 0.0f){
                remainingTime -= Time.deltaTime;
                lastTick += Time.deltaTime;
            }
            
        }
        if(lastTick >= tickRate){ 
            if(particles !=  null){
                GameObject.Instantiate(particles, actor.gameObject.transform);
            }
            OnTick();
            lastTick -= tickRate;
        }
        if(remainingTime <= 0 ){
            if(actor.isServer){
                onFinish();
            }
        }
    }
    public virtual void OnTick(){
        foreach(EffectInstruction eI in eInstructs){
            eI.startEffect(actor, null, caster);
        }
    }
    public virtual void onStart(){

        foreach(EffectInstruction eI in eInstructs){
            eI.effect.target = actor;
            eI.effect.caster = caster;
            
            eI.effect.buffStartEffect();
        }

    }
    public virtual void onFinish(){
        foreach(EffectInstruction eI in eInstructs){
            eI.effect.buffEndEffect();
        }
        Debug.Log(effectName + ": onFinish complete");
        //List<Buff> list_ref = actor.getBuffs();

        //Find this buff in actor's List<> and remove it
        // Debug.Log(effectName+ ": list rm");
        //list_ref.Remove(list_ref.Find(x => x ==  this)); //This needs to be tested
        if(actor.isServer){
            actor.removeBuff(this);
        }
        else{
            actor.ClientRemoveBuff(this);
        }
        
    }
    public virtual void OnRemove(){
        
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
    
    public void Init(string _effectName, float _duration, List<AbilityEff> _effects, float _tickRate = 3.0f, int _id = -1,
                      bool _stackable = false, bool _refreshable = true, uint _stacks = 1, GameObject _particles = null){

        effectName = _effectName;
        duration = _duration;
        //effects = _effects;

        //remainingTime = duration;

        tickRate = _tickRate;
        id = _id;
        stackable = _stackable;
        refreshable = _refreshable;
        stacks = _stacks;
        particles = _particles;
        foreach (AbilityEff eff in _effects){
            eff.parentBuff = this;
        }
        eInstructs = new List<EffectInstruction>();
        eInstructs.addEffects(_effects);

    }public void Init(string _effectName, float _duration, List<EffectInstruction> _eInstructs, float _tickRate = 3.0f, int _id = -1,
                      bool _stackable = false, bool _refreshable = true, uint _stacks = 1, GameObject _particles = null){

        effectName = _effectName;
        duration = _duration;

        eInstructs = _eInstructs;
        //remainingTime = duration;

        tickRate = _tickRate;
        id = _id;
        stackable = _stackable;
        refreshable = _refreshable;
        stacks = _stacks;
        particles = _particles;
        foreach(EffectInstruction eI in eInstructs){
            eI.effect.parentBuff = this;
        }

    }
    
    public Buff clone(){
        // Creates an editable version of the input Ability Effect

        Buff temp_ref = ScriptableObject.CreateInstance(typeof (Buff)) as Buff;
        
        temp_ref.Init(String.Copy(effectName), duration, eInstructs.cloneInstructs(),  
         tickRate, id, stackable, refreshable, stacks, particles);
         temp_ref.onCastHooks = onCastHooks;
         temp_ref.onHitHooks = onHitHooks;
        //  temp_ref.caster = caster;
        //  temp_ref.target = target;
        return temp_ref;
    }
}
