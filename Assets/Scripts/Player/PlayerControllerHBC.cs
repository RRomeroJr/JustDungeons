using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerHBC : MonoBehaviour
{
    // When castCompleted is true queueAbility will fire
    public bool castReady = false; // Will only be set TRUE by CastBar
    public bool isCasting = false; // Will only be set FALSE by CastBar 
    public Ability queuedAbility;
    
    public Actor player;
    public UIManager uiManager;

    public List<AbilityCooldown> abilityCooldowns = new List<AbilityCooldown>();
 
    void Start()
    {     
    }

    // Update is called once per frame
    void Update()
    {   
        updateCooldowns();
        
        if(Input.GetKeyDown("1")){
            checkAndQueue(PlayerAbilityData._instantAbility);
        }
        if(Input.GetKeyDown("2")){       
            checkAndQueue(PlayerAbilityData._castedDamage);
        }
        if(Input.GetKeyDown("3")){
            checkAndQueue(PlayerAbilityData._castedHeal);
        }
        if(Input.GetKeyDown("4")){
            checkAndQueue(PlayerAbilityData._instantAbility2);
        }
        
        if(castReady){
            //Debug.Log("castCompleted: " + queuedAbility.getName()); 
            player.castAbility(queuedAbility, player.target);
            castReady = false;
            addToCooldowns(queuedAbility);

        }
    }

    void queueAbility(Ability inAbility){
        if(isCasting){
            Debug.Log("You are casting!");
        }
        else{
            if(player.target != null){ 

                if(inAbility.getCastTime() > 0.0f){ // Casted Ability

                    //Debug.Log("Trying to create a castBar for " + inAbility.getName());

                    //Preparing variables for cast
                    queuedAbility = inAbility;
                    castReady = false; // for saftey. Should've been set by castBar or initialized that way already
                    isCasting = true;

                    //Creating cast bar and setting it's parent to canvas to display it properly
                    GameObject newAbilityCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);
                    // v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
                    newAbilityCast.GetComponent<CastBar>().Init(inAbility.getName(), player,
                                                                    player.target, inAbility.getCastTime());

                }
                else{
                    Debug.Log("GM| Instant cast: " + inAbility.getName());
                    queuedAbility = inAbility;
                    castReady = true;
                }
            }
            else{
                Debug.Log("You don't have a target!");
            }
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
    bool checkOnCooldown(Ability _ability){
        if(abilityCooldowns.Count > 0){
            for(int i = 0; i < abilityCooldowns.Count; i++){
                if(abilityCooldowns[i].getName() == _ability.getName()){
                    return true;
                }
            }
            return false;
        }
        else{
            return false;
        }
    }
    void checkAndQueue(Ability _ability){
        if(checkOnCooldown(_ability) == false){
            queueAbility(_ability);
        }
    }
}
