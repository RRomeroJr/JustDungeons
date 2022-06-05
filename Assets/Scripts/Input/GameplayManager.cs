using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    
    public UIManager uiManager;
    public GameObject player;

    // ~~~~~~~~~~~~~~~~~~~~~~For testing casting and Ability effect system~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // RR: In the future these should be in a sperate file somewhere. Don't know how to do that yet
    public AbilityEffect oneOffDamageEffect;
    public AbilityEffect dotEffect;
    public AbilityEffect oneOffHealEffect;
    public AbilityEffect hotEffect;
    public Ability castedAbility;
    public Ability instantAbility;
    public Ability castedHeal;
    public Ability instantHeal;
    public Ability testerBolt;
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    void Start()
    {
        Debug.Log("Press \"1-4\" | DoT, Dmg, Heal, HoT! Careful bc you can do many at once if you spam");
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~For testing casting and Ability effect system~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // RR: In the future these should be in a sperate file somewhere. Don't know how to do that yet
        
        //            (Ability Name, Ability Type, Power, Duration, Tick Rate) || 0=dmg, 1=heal, tick rate not working atm
        oneOffDamageEffect = new AbilityEffect("Testerbolt Effect", 0, 7.0f, 0.0f, 0.0f);
        dotEffect = new AbilityEffect("Debugger\'s Futility Effect", 2, 30.0f, 9.0f, 3.0f);// damage ^^
        oneOffHealEffect = new AbilityEffect("Quality Assured Effect", 1, 13.0f, 0.0f, 0.0f);
        hotEffect = new AbilityEffect("Sisyphean Resolve Effect", 3, 25.0f, 4.0f, 1.0f);// heals ^^

        castedAbility = new Ability("Testerbolt", oneOffDamageEffect, 1.5f);
        instantAbility = new Ability("Debugger\'s Futility", dotEffect, 0.0f);
        castedHeal = new Ability("Quality Assured", oneOffHealEffect, 1.5f);
        instantHeal = new Ability("Sisyphean Resolve Effect", hotEffect, 0.0f);
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }
    
    // bool isCasting
    // float castEnd 
    void Update()
    {
        //if(isCasting)
            //if(Time.time >= castEnd) if it go to the end
                //castAbility(instantAbility);
        if(Input.GetKeyDown("1")){
            castAbility(instantAbility);
        }
        if(Input.GetKeyDown("2")){       
            castAbility(castedAbility);
        }
        if(Input.GetKeyDown("3")){
            castAbility(castedHeal);
        }
        if(Input.GetKeyDown("4")){
                
            castAbility(instantHeal);
        }
    }
    void castAbility(Ability inAbility){
        if(uiManager.hasTarget){
            //Debug.Log("GM: Casting Ability.. " + inAbility.getName());
            if(inAbility.getCastTime() > 0.0f){
                //Creating cast bar and setting it's parent to canvas to display it properly
                GameObject newAbilityCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);
                // v (AbilityEffect ability_effect, string ability_name, Actor from_caster, Actor to_target, float cast_time) v
                newAbilityCast.GetComponent<CastBar>().Init(inAbility.getEffect(), inAbility.getName(), player.GetComponent<Actor>(),
                                                                uiManager.targetFrame.actor, inAbility.getCastTime());
            }
            else{
                Debug.Log("GM| Instant cast: " + inAbility.getName());
                uiManager.targetFrame.actor.applyAbilityEffect(inAbility.getEffect(), player.GetComponent<Actor>());
            }
        }
        else{
            Debug.Log("You don't have a target!");
        }
        
    }
}
