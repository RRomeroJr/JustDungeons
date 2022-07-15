using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[System.Serializable]
public class AbilityEffect
{
    /*
        These are private bc they shoud be thought about as "starting points".

        If you want an effect but, for ex., want it to last a little longer 

        you call an Actor's applyAbilityEffect() then manipulate the ActiveAbilityEffect
        that it generates 
        
    */
    [SerializeField]protected string effectName;
    [SerializeField]protected int effectType; // 0=damage, 1=heal, 2=DoT, 3=Hot, 4= something else... tbd
    [SerializeField]protected float power;
    [SerializeField]protected float duration;
    [SerializeField]protected float tickRate; // for now will be rounded
    [SerializeField] protected GameObject particles;
    [SerializeField]protected Action<Actor, Actor> startAction;
    [SerializeField]protected Action<Actor, Actor> hitAction; // Doesn't work yet
    [SerializeField] protected Action<Actor, Actor> finishAction; 
    [SerializeField]protected bool stackable;
    [SerializeField]protected bool refreshable;

    [SerializeField]protected float lastTick = 0.0f; // time since last tick
    [SerializeField]protected float remainingTime = 0.0f;
    [SerializeField]protected bool start = false;
    [SerializeField]protected bool firstFrame = true;
    [SerializeField]protected Actor caster;
    [SerializeField]protected Actor target; // Actor that this effect is attached to
    [SerializeField]protected Vector3 targetWP;
    [SerializeField]protected bool canEdit = true; // Can only be set with constructor
    [SerializeField]protected int id; // Should be a positive unique identifer
    [SerializeField]protected uint stacks; // Should be a positive unique identifer
    
    public AbilityEffect(){
    }
    public AbilityEffect(string _effectName, int _effectType, float _power, float _duration = 0.0f,
                                float _tickRate = 0.0f, GameObject _particles = null, Action<Actor, Actor> _startAction = null,
                                Action<Actor, Actor> _hitAction = null, Action<Actor, Actor> _finishAction = null, bool _canEdit = true,
                                int _id = -1, bool _stackable = false, bool _refreshable = true){


        baseInit(_effectName, _effectType, _power, _duration,_tickRate, _particles, _startAction,
                     _hitAction, _finishAction, _canEdit, _id, _stackable, _refreshable);


    }
    public AbilityEffect(string _effectName, int _effectType, float _power, Vector3 _targetWP, float _duration = 0.0f,
                                float _tickRate = 0.0f, GameObject _particles = null, Action<Actor, Actor> _startAction = null,
                                Action<Actor, Actor> _hitAction = null, Action<Actor, Actor> _finishAction = null, bool _canEdit = true
                                , int _id = -1, bool _stackable = false, bool _refreshable = true){

                //The idea behind this constructor was to make a dash ability effect type
                // but I don't think that a dash would ever be stackable or refreshable?
                // sort this out in the future
      
                
        baseInit(_effectName, _effectType, _power, _duration,_tickRate, _particles, _startAction,
                     _hitAction, _finishAction, _canEdit, _id, _stackable, _refreshable);
        targetWP = _targetWP;
        
    }
    public AbilityEffect(AbilityEffectPreset _aep, bool _canEdit = true){
        baseInit2(_aep);
        canEdit = _canEdit;
    }
    public AbilityEffect(AbilityEffectPreset _aep, Vector3 _targetWP, bool _canEdit = true){
        baseInit2(_aep);
        targetWP = _targetWP;
        canEdit = _canEdit;
    }

    public string getEffectName(){
        return effectName;
    }
    public void setEffectName(string _effectName){
        if(canEdit){
            effectName = _effectName;        
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
    }
    public int getEffectType(){
        return effectType;
    }
    public void setEffectType(int _effectType){
        if(canEdit){
            effectType = _effectType; 
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
    }
    public float getPower(){
        return power;
    }
    public void setPower(float _power){
        if(canEdit){
            power = _power;
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
    }
    public float getDuration(){
        return duration;
    }
    public void setDuration(float _duration){
        if(canEdit){
            duration = _duration;
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
        
    }
    public float getTickRate(){
        return tickRate;
    }
    public void setTickRate(float _tickRate){
        if(canEdit){
            tickRate = _tickRate;
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
    }
    public GameObject getParticles(){
        return particles;
    }
    public void setParticles(GameObject _particles){
        particles = _particles;
    }
    
    public Action<Actor,Actor> getStartAction(){
        return startAction;
    }
    public void setStartAction(Action<Actor,Actor> _startAction){
        if(canEdit){
            startAction = _startAction;
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
        
    }
    public Action<Actor, Actor> getHitAction(){
        return hitAction;
    }
    public void setHitAction(Action<Actor, Actor> _hitAction){
        if(canEdit){
            hitAction = _hitAction;
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
        
    }
    public Action<Actor, Actor> getFinishAction(){
        return finishAction;
    }
    public void setFinishAction(Action<Actor, Actor> _finishAction){
        if(canEdit){
            finishAction = _finishAction;
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
        
    }
    public float getLastTick(){
        return lastTick;
    }
    public void setLastTick(float _lastTick){
        if(canEdit){
            lastTick = _lastTick;
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
    }
    public float getRemainingTime(){
        return remainingTime;
    }
    public void setRemainingTime(float _remainingTime){
         if(canEdit){
            remainingTime = _remainingTime;
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
        
    }
    public bool getStart(){
        return start;
    }
    public void setStart(bool _start){
        if(canEdit){
            start = _start;
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
        
    }
    public void toggleStart(){
        if(canEdit){
            if(start){
                start = false;
            }
            else{
                start = true;
            }
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
        
    }
    public Actor getCaster(){
        return caster;
    }
    public void setCaster(Actor _caster){
        if(canEdit){
            caster = _caster;
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
        
    }
    public Actor getTarget(){
        return target;
    }
    public void setTarget(Actor _target){
        target = _target;
    }
    public Vector3 getTargetWP(){
        return targetWP;
    }
    public void setTargetWP(Vector3 _targetWP){
        if(canEdit){
            targetWP = _targetWP;
        }else{
            Debug.Log("Can't edit effect" + effectName);
        }
        
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
    public virtual void OnEffectFinish(Actor _caster, Actor _target){
        if(finishAction != null){
            finishAction(_caster, _target);
        }
        //Debug.Log("AE: deafult finish");
    }
    public virtual void OnEffectHit(Actor _caster, Actor _target){          //  Make this work in actor later
        if(hitAction != null){
            hitAction(_caster, _target);
        }
    }
    public virtual void OnEffectStart(Actor _caster, Actor _target){
        if(startAction != null){
            startAction(_caster, _target);
        }
    }

    public AbilityEffect clone(){
        // Creates an editable version of the input Ability Effect
        return new AbilityEffect(String.Copy(effectName), effectType, power, targetWP, duration, tickRate, particles,
                                startAction, hitAction, finishAction, true, id, stackable, refreshable);
    }
        
    public void update(){
        if(canEdit == false){
            Debug.Log("Can't update " + effectName + ". Not editable" );
            return;
        }
        if(start){
                if(firstFrame){
                    if(particles !=  null)
                        GameObject.Instantiate(particles, target.gameObject.transform);
                    firstFrame = false;
                    
                }
                else{
                    remainingTime -= Time.deltaTime;
                    lastTick += Time.deltaTime;
                }
                switch(effectType){
                            case 0: // damage
                                if(remainingTime <= 0.0f){
                                    if(particles !=  null)
                                        GameObject.Instantiate(particles, target.gameObject.transform);
                                    target.damageValue((int) power);
                                }
                                break;
                            case 1: // heal
                                if(remainingTime <= 0.0f){
                                    if(particles !=  null)
                                        GameObject.Instantiate(particles, target.gameObject.transform);
                                    target.restoreValue((int) power);
                                }
                                break;
                            case 2: // DoT
                                if(lastTick >= tickRate){ 
                                    if(particles !=  null)
                                        GameObject.Instantiate(particles, target.gameObject.transform);
                                    target.damageValue((int) DotHotPower());
                                    lastTick -= tickRate;
                                }
                                break;
                            case 3: // HoT
                                if(lastTick >= tickRate){
                                    if(particles !=  null)
                                        GameObject.Instantiate(particles, target.gameObject.transform);
                                    target.restoreValue((int) DotHotPower());
                                    lastTick -= tickRate;
                                }
                                break;
                            case 4: //  Teleport
                                if(remainingTime <= 0.0f){
                                    if(particles !=  null)
                                        GameObject.Instantiate(particles, target.gameObject.transform);
                                    target.transform.position = targetWP;
                                }
                                break;
                            case 5: //  dash
                                if(remainingTime <= 0.0f){
                                    if(particles !=  null)
                                        GameObject.Instantiate(particles, target.gameObject.transform);
                                    if(target.gameObject.tag == "Player"){
                                        target.gameObject.GetComponent<PlayerMovementScript2>().setLastMovementEffect(this);
                                        target.gameObject.GetComponent<PlayerMovementScript2>().setDashing(true);
                                    }
                                    else{
                                        Debug.Log("NPCs can't dash yet");
                                    }

                                }
                                break;
                            default:
                                Debug.Log("Unknown Ability type on " + target.getActorName() + "! Don't know what to do! Setting remaining to 0.0f..");
                                remainingTime = 0.0f;
                                break;
                        
                }
            
        }
    }
    // ------------------------------------------Start/ hit/ finish effects-------------------------------------------------------
    void baseInit(string _effectName, int _effectType, float _power, float _duration = 0.0f,
                                float _tickRate = 0.0f, GameObject _particles = null, Action<Actor, Actor> _startAction = null,
                                Action<Actor, Actor> _hitAction = null, Action<Actor, Actor> _finishAction = null, bool _canEdit = true,
                                int _id = -1, bool _stackable = false, bool _refreshable = true){
        //Debug.Log("Creating AbilityEffect: " + _effectName);
        effectName = _effectName;
        effectType = _effectType;
        power = _power;
        duration = _duration;
        tickRate = RoundToNearestHalf(_tickRate);
        particles = _particles;
        startAction = _startAction;
        hitAction = _hitAction;
        finishAction = _finishAction;
        canEdit = _canEdit;
        id = _id;
        stackable = _stackable;
        refreshable = _refreshable;
        
        
    }
    void baseInit2(AbilityEffectPreset _aep){
        //Debug.Log("Creating AbilityEffect: " + _effectName);
        effectName = _aep.getEffectName();
        effectType = _aep.getEffectType();
        power = _aep.getPower();
        duration = _aep.getDuration();
        tickRate = RoundToNearestHalf(_aep.getTickRate());
        particles = _aep.getParticles();
        startAction = _aep.getStartAction();
        hitAction = _aep.getHitAction();
        finishAction = _aep.getFinishAction();
        id = _aep.getID();
        stackable = _aep.isStackable();
        refreshable = _aep.isRefreshable();
    }
    
    float DotHotPower(){
        return ( ( tickRate / duration ) * power );
    }
    float RoundToNearestHalf(float value) 
    {
        //   rounds to nearest x.5
        return MathF.Round(value * 2) / 2;
    }
}
