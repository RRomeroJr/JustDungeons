using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
     Container many for any RPG related elements
*/

public class Actor : MonoBehaviour
{
    public bool showDebug = false;
    public string actorName;
    public int health;
    public int maxHealth;
    public float mana;
    public float maxMana;
    public Actor target;
    public Vector3? queuedTargetWP;
    public Color unitColor;
    public List<AbilityEffect> abilityEffects;
    
    // When readyToFire is true queuedAbility will fire
    public bool readyToFire = false; // Will True by CastBar for abilities w/ casts. Will only be true for a freme
    public bool isCasting = false; // Will only be set False by CastBar 
    public Ability queuedAbility; // Used when Ability has a cast time
    public Actor queuedTarget; // Used when Ability has a cast time
    public List<AbilityCooldown> abilityCooldowns = new List<AbilityCooldown>();
    public UIManager uiManager;
    public GameObject abilityDeliveryPrefab;
    //public GameObject testParticlesPrefab;



    void Start(){
        abilityEffects = new List<AbilityEffect>();
    }
    void Update(){
        updateCooldowns();
        handleAbilityEffects();
        handleCastQueue();
    }
    //------------------------------------------------------------handling Active Ability Effects-------------------------------------------------------------------------
    
    void handleAbilityEffects(){

        if(abilityEffects.Count > 0){

            for(int i = 0; i < abilityEffects.Count; i++){

                switch(abilityEffects[i].getEffectType()){
                    case 0: // damage
                        handleDamage(abilityEffects[i]);

                        break;

                    case 1: // heal
                        handleHeal(abilityEffects[i]);
                        break;
                    case 2: // DoT
                        if(abilityEffects[i].start){ 
                        // if this isn't the first frame

                            //Iterate duration
                            abilityEffects[i].remainingTime -= Time.deltaTime;
                            //Iterate lastTick
                            abilityEffects[i].lastTick += Time.deltaTime;

                            if(abilityEffects[i].lastTick >= abilityEffects[i].getTickRate()){
                                // if rdy to tick
                                //Spawn particles
                                if(abilityEffects[i].particles != null)
                                    Instantiate(abilityEffects[i].particles, gameObject.transform);
                                handleDoT(abilityEffects[i]);
                                //Debug.Log("Actor: ticking" + abilityEffects[i].getEffectName() + " on " + actorName);
                            }
                        }
                        else{
                            handleDoT(abilityEffects[i]);
                            abilityEffects[i].start = true;
                        }
                        break;
                    case 3: // HoT
                        if(abilityEffects[i].start){
                        // if this isn't the first frame
                            //Iterate duration
                            abilityEffects[i].remainingTime -= Time.deltaTime;
                            //Iterate lastTick
                            abilityEffects[i].lastTick += Time.deltaTime;

                            if(abilityEffects[i].lastTick >= abilityEffects[i].getTickRate()){
                                // if rdy to tick

                                handleHoT(abilityEffects[i]);
                               // Debug.Log("Actor: ticking" + abilityEffects[i].getEffectName() + " on " + actorName);
                            }
                            
                        }
                        else{
                            handleHoT(abilityEffects[i]);
                            abilityEffects[i].start = true;
                        }
                        break;
                    case 4: //dash to point
                        if(abilityEffects[i].start){
                            //Iterate duration
                            abilityEffects[i].remainingTime -= Time.deltaTime;
                            //Iterate lastTick
                            abilityEffects[i].lastTick += Time.deltaTime;
                                                                                                                                //power == speed of dash
                            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, abilityEffects[i].dashTarget,  abilityEffects[i].power);
                        }
                        else{
                            //handleDashToPoint()
                            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, abilityEffects[i].dashTarget,  abilityEffects[i].power);
                            abilityEffects[i].start = true;
                        }
                        break;
                    default:
                        Debug.Log("Unknown Ability type on " + actorName + "! Don't know what to do! Trying to remove..");
                        abilityEffects[i].duration = 0.0f;
                        break;
                
                }
            checkabilityEffectToRemoveAtPos(abilityEffects[i], i);
            }
        //Debug.Log(actorName + " cleared all Ability effects!");
        }
    }
    void damageValue(int amount){
        // Right now this only damages health, but, maybe in the future,
        // This could take an extra param to indicate a different value to "damage"
        // For ex. a Ability that reduces maxHealth or destroys mana

        //Debug.Log("damageValue: " + amount.ToString()+ " on " + actorName);
        health -= amount;

    }

    void restoreValue(int amount){
        // This would be the opposite of damageValue(). Look at that for more details
        
        //Debug.Log("restoreValue: " + amount.ToString()+ " on " + actorName);
        health += amount;

    }

    void checkabilityEffectToRemoveAtPos(AbilityEffect _abilityEffect, int listPos){
        // Remove AbilityEffect is it's duration is <= 0.0f

        if(_abilityEffect.remainingTime <= 0.0f){
            if(showDebug)
            Debug.Log(actorName + ": Removing.. "+ _abilityEffect.getEffectName());
            abilityEffects[listPos].OnEffectFinish(abilityEffects[listPos].caster, this);
            abilityEffects.RemoveAt(listPos);
        }
    }
    public void applyAbilityEffect(AbilityEffect _abilityEffect, Actor inCaster){

        //Adding AbilityEffect it to this actor's list<AbilityEffect>
        _abilityEffect.caster = inCaster;
        _abilityEffect.remainingTime = _abilityEffect.getDuration();
        abilityEffects.Add(_abilityEffect);

        _abilityEffect.OnEffectStart(inCaster, this);
        //Debug.Log("Actor: Applying.." + _abilityEffect.getEffectName() + " to " + actorName);  

    }
    public void applyAbilityEffects(List<AbilityEffect> _abilityEffects, Actor inCaster){
        if(_abilityEffects.Count > 0){
            for(int i = 0; i < _abilityEffects.Count; i++ ){
                applyAbilityEffect(_abilityEffects[i], inCaster);
            }
        } 

    }
    void handleDamage(AbilityEffect _abilityEffect){// Type 0

        /* 
            In here you could add interesting interactions
            Maybe something like 
            if(ReverseDamageAndHealing)
                then call restoreValue() instead
        */
        
        damageValue( (int) _abilityEffect.getPower() );// likly will change once we have stats

        // For saftey to make sure that the effect is removed from list
        // right aft the effect finishes
        _abilityEffect.remainingTime = 0.0f; 
            
    }
    void handleDoT(AbilityEffect _abilityEffect){// Type 2

        // Do any extra stuff

        damageValue( (int) ( ( _abilityEffect.getTickRate() / (_abilityEffect.getDuration() + _abilityEffect.getTickRate()) ) * _abilityEffect.getPower() ) );// likly will change once we have stats
        //damageValue( (int) _abilityEffect.getPower()  );
        if(_abilityEffect.lastTick >= _abilityEffect.tickRate)
            _abilityEffect.lastTick -= _abilityEffect.tickRate;
            
    }

    void handleHeal(AbilityEffect _abilityEffect){// Type 1

        /* 
            In here you could add interesting interactions
            Maybe something like 
            if(ReverseDamageAndHealing)
                then call damageValue() instead
        */
        
        restoreValue( (int) _abilityEffect.getPower() ); // likly will change once we have stats

        // For saftey to make sure that the effect is removed from list
        // right aft the effect finishes
        _abilityEffect.remainingTime = 0.0f; 
            
    }

    void handleHoT(AbilityEffect _abilityEffect){// Type 3

        // Do any extra stuff

        restoreValue( (int) ( ( _abilityEffect.getTickRate() / _abilityEffect.getDuration() ) * _abilityEffect.getPower() ) );// likly will change once we have stats
        if(_abilityEffect.lastTick >= _abilityEffect.tickRate)
            _abilityEffect.lastTick -= _abilityEffect.tickRate;
            
    }

    //-------------------------------------------------------------------handling casts--------------------------------------------------------------------------

    public void castAbility(Ability _ability, Actor _target = null, Vector3? _targetWP = null){
        //Main way to make Acotr cast an ability

        if(checkOnCooldown(_ability) == false){
            if(!readyToFire){
                if(checkAbilityReqs(_ability, _target, _targetWP)){
                    if(_ability.getCastTime() > 0.0f){
                        if(!isCasting){
                            queueAbility(_ability, _target, _targetWP);
                            prepCast();
                        }
                    }
                    else{
                        fireCast(_ability, _target, _targetWP);
                    }
                }
                else{
                    if(showDebug)
                    Debug.Log("Ability doesn't have necessary requirments");
                }                   
            }
            else{
                if(showDebug)
                Debug.Log("Something else is ready to fire and blocking this cast");
            }
        }
    }
    public void castAbility(Ability _ability, Vector3 _queuedTargetWP){
        //Main way to make Acotr cast an ability

        if(checkOnCooldown(_ability) == false){
            if(!readyToFire){
                Vector3? tempNullibleVect = _queuedTargetWP;
                if(checkAbilityReqs(_ability, null, tempNullibleVect)){
                    if(_ability.getCastTime() > 0.0f){
                        if(!isCasting){
                            queueAbility(_ability, null, tempNullibleVect);
                            prepCast();
                        }
                    }
                    else{
                        fireCast(_ability, null, tempNullibleVect);
                    }
                }
                else{
                    if(showDebug)
                    Debug.Log("Ability doesn't have necessary requirments: A, WP");
                }                       
            }
            else{
                if(showDebug)
                Debug.Log("Something else is ready to fire and blocking this cast: A, WP");
            }
        }
    }
    public void freeCast(Ability _ability, Actor _target = null, Vector3? _targetWP = null){
        //  Make Acotor cast an ability without starting a cooldown or (in the future) cost resources
        // Maybe make this into an overload of castAbility later

        if(checkAbilityReqs(_ability, _target, _targetWP)){
            if(handleDelivery(_ability, _target)){
                // No cd gfenerated
                // Make not cost resources
                readyToFire = false;
            }
            
        }
        else{
            if(showDebug)
            Debug.Log(actorName + " Free cast failed reqs");
        }

    }
    public void freeCast(Ability _ability, Vector3 _targetWP){
        //  Make Acotor cast an ability without starting a cooldown or (in the future) cost resources
        // Maybe make this into an overload of castAbility later

        if(checkAbilityReqs(_ability, null, _targetWP)){
            if(handleDelivery(_ability, null, _targetWP)){
                // No cd gfenerated
                // Make not cost resources
                readyToFire = false;
            }
            
        }
        else{
            if(showDebug)
            Debug.Log(actorName + " Free cast failed reqs: A, WP");
        }

    }
    void forceCastAbility(Ability _ability, Actor _target){
        
        /*
                                                                  ***IGNORE*** Unused for now
        */
        
        if(_target != null){
            //Debug.Log("A: " + actorName + " casting " + _ability.getName() + " on " + target.actorName);
            _target.applyAbilityEffects(_ability.getEffects(), this);
            addToCooldowns(queuedAbility);
        }
        else{
            if(showDebug)
            Debug.Log("Actor: " + actorName + " has no target!");
        }

    }

    public void fireCast(Ability _ability, Actor _target = null, Vector3? _targetWP = null){
        // Main way for "Fireing" a cast by creating a delivery if needed then creating an AbilityCooldown
        if(handleDelivery(_ability, _target, _targetWP)){
            addToCooldowns(queuedAbility);
            readyToFire = false;
        }

    }  
    void queueAbility(Ability _ability, Actor _queuedTarget = null, Vector3? _queuedTargetWP = null){
        //Preparing variables for a cast
        queuedAbility = _ability;
        queuedTarget = _queuedTarget;
        queuedTargetWP = _queuedTargetWP;
    }
    void prepCast(){
        //Creates castbar for abilities with cast times

        //Debug.Log("Trying to create a castBar for " + _ability.getName())
            
        isCasting = true;   

        // Creating CastBar or CastBarNPC with apropriate variables   
        InitCastBarFromQueue();  
    }
    void InitCastBarFromQueue(){
        //Makes a castbar 

        if( (queuedAbility.NeedsTargetActor()) && (queuedAbility.NeedsTargetWP()) ){
            Debug.Log("Spell that needs an Actor and WP are not yet suported");
        }
        else if(queuedAbility.NeedsTargetActor()){
            initCastBarWithActor();
        }
        else if(queuedAbility.NeedsTargetWP()){
            initCastBarWithWP();
        }
        else{
            initCastBarWithActor();
        }
    }
    void initCastBarWithActor(){
        // Creates a CastBar with target being an Actor
        if(gameObject.tag == "Player"){ // For player
                //Creating cast bar and setting it's parent to canvas to display it properly

                GameObject newAbilityCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);
                //                                   v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
                newAbilityCast.GetComponent<CastBar>().Init(queuedAbility.getName(), this, queuedTarget, queuedAbility.getCastTime());
        }
        else{// For NPCs
            if(showDebug)
            Debug.Log(actorName + " starting cast: " + queuedAbility.getName());
            gameObject.AddComponent<CastBarNPC>().Init(queuedAbility.getName(), this, queuedTarget, queuedAbility.getCastTime());
        }
    }
    void initCastBarWithWP(){
        //   Creates Castbar with target being a world point Vector3

        if(gameObject.tag == "Player"){ // For player
                //Creating cast bar and setting it's parent to canvas to display it properly

                GameObject newAbilityCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);

                Vector3 tempVect = queuedTargetWP ?? Vector3.zero; // Make this ERROR instead in the future

                //                                   v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
                newAbilityCast.GetComponent<CastBar>().Init(queuedAbility.getName(), this, tempVect, queuedAbility.getCastTime());
        }
        else{// For NPCs
            if(showDebug)
            Debug.Log(actorName + " starting cast: " + queuedAbility.getName());

            Vector3 tempVect = queuedTargetWP ?? Vector3.zero; // Make this ERROR instead in the future

            gameObject.AddComponent<CastBarNPC>().Init(queuedAbility.getName(), this, tempVect, queuedAbility.getCastTime());
        }
    }
    bool handleDelivery(Ability _ability, Actor _target = null, Vector3? _targetWP = null){
        // Creates delivery if needed. Applies effects to target if not
        // ***** WILL RETURN FALSE if DeliveryType is -1 (auto apply to target) and there is no target *****


        List<AbilityEffect> tempListAE_Ref = cloneListAE(_ability.getEffects());

        if(_ability.DeliveryType == -1){
            if(_target != null){
                _target.applyAbilityEffects(tempListAE_Ref, this);
                return true;
            }
            else{
                Debug.Log(actorName + ": Direct Actor to Actor Delivery with no target");
                return false;
            }
        }
        else{
            GameObject delivery = CreateAndInitDelivery(tempListAE_Ref, _ability.DeliveryType, _target, _targetWP);
            return true;
        }
    }
    List<AbilityEffect> cloneListAE(List<AbilityEffect> _abilityEffects){
        List<AbilityEffect> tempListAE_Ref = new List<AbilityEffect>();
        if(_abilityEffects.Count > 0){
            for(int i = 0; i < _abilityEffects.Count; i++ ){
                AbilityEffect tempAE_Ref = _abilityEffects[i].clone();
                 /*          
                     vV__Pretend below power is being modified by Actor's stats__Vv
                 */
                tempAE_Ref.setEffectName(tempAE_Ref.getEffectName() + " ("+ actorName + ")");
                tempListAE_Ref.Add(tempAE_Ref);
            }
        }
        return tempListAE_Ref;
    }
    GameObject CreateAndInitDelivery(List<AbilityEffect> _abilityEffects, int _deliveryType, Actor _target = null, Vector3? _targetWP = null){
        // Creates and returns delivery

        GameObject delivery;

        if( (queuedAbility.NeedsTargetActor()) && (queuedAbility.NeedsTargetWP()) ){
            Debug.Log("Spell With Actor and WP reqs not yet suported");
            delivery = null;
        }
        else if(queuedAbility.NeedsTargetActor()){
            delivery = Instantiate(abilityDeliveryPrefab, gameObject.transform.position, gameObject.transform.rotation);
            delivery.GetComponent<AbilityDelivery>().init( _abilityEffects, _deliveryType, this, _target, 0.1f);
            return delivery;
        }
        else if(queuedAbility.NeedsTargetWP()){

            Vector3 tempVect = _targetWP ?? Vector3.zero; // Make this ERROR instead in the future

            delivery = Instantiate(abilityDeliveryPrefab, gameObject.transform.position, gameObject.transform.rotation);
            delivery.GetComponent<AbilityDelivery>().init( _abilityEffects, _deliveryType, this, tempVect, 0.1f);
        }
        else{
            delivery = Instantiate(abilityDeliveryPrefab, gameObject.transform.position, gameObject.transform.rotation);
            delivery.GetComponent<AbilityDelivery>().init( _abilityEffects, _deliveryType, this, _target, 0.1f);
        }
        return delivery;
    }
    void handleCastQueue(){
        // Called every Update() to see if queued spell is ready to fire

        if(readyToFire){
            //Debug.Log("castCompleted: " + queuedAbility.getName());
            if(queuedAbility.NeedsTargetWP()){
                fireCast(queuedAbility, null, queuedTargetWP);
            }
            else{
                fireCast(queuedAbility, queuedTarget);
            }
        }
    }
    bool checkAbilityReqs(Ability _ability, Actor _target = null, Vector3? _targetWP = null){
        // Checks if the requirments of _ability are satisfied

        //Debug.Log(_ability.NeedsTargetActor().ToString() + " " + _ability.NeedsTargetWP().ToString());
        if( (_ability.NeedsTargetActor()) && (_ability.NeedsTargetWP()) ){
            return ( (_target != null) && (_targetWP != null) );
        }
        else if(_ability.NeedsTargetActor()){
            return _target != null;
        }
        else if(_ability.NeedsTargetWP()){
            return _targetWP != null;
        }
        else{
            return true;
        }
    }
    void updateCooldowns(){
        if(abilityCooldowns.Count > 0){
            for(int i = 0; i < abilityCooldowns.Count; i++){
                if(abilityCooldowns[i].remainingTime > 0)
                    abilityCooldowns[i].remainingTime -= Time.deltaTime;
                else
                    abilityCooldowns.RemoveAt(i);
            }
        }
    }
    void addToCooldowns(Ability _ability){
        abilityCooldowns.Add(new AbilityCooldown(queuedAbility));
    }
    public bool checkOnCooldown(Ability _ability){
        if(abilityCooldowns.Count > 0){
            for(int i = 0; i < abilityCooldowns.Count; i++){
                if(abilityCooldowns[i].getName() == _ability.getName()){
                    if(showDebug)
                        Debug.Log(queuedAbility.getName() + " is on cooldown!");
                    return true;
                }
            }
            return false;
        }
        else{
            return false;
        }
    }
    //-------------------------------------------------------------------other---------------------------------------------------------------------------------------------------------
    float RoundToNearestHalf(float value) 
    {
        //   rounds to nearest x.5
        return MathF.Round(value * 2) / 2;
    }
}

