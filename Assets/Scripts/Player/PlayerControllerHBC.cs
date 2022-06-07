using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerHBC : MonoBehaviour
{
    // When castCompleted is true queueAbility will fire
    public bool castCompleted = false; // Will only be set TRUE by CastBar
    public bool isCasting = false; // Will only be set FALSE by CastBar 

    public Ability queuedAbility;
    
    public Actor player;
    public Actor target; // Set by clickManager
    public UIManager uiManager;


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

    // Start is called before the first frame update
    void Start()
    {


        Debug.Log("Press \"1-4\" | DoT, Dmg, Heal, HoT! Careful bc you can do many at once if you spam");
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~For testing casting and Ability effect system~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // RR: In the future these should be in a sperate file somewhere. Don't know how to do that yet
        
        //     (Ability Name, Ability Type, Power, Duration, Tick Rate) || 0=dmg, 1=heal, tick rate not working atm
        oneOffDamageEffect = new AbilityEffect("Testerbolt Effect", 0, 8.0f, 0.0f, 0.0f);
        dotEffect = new AbilityEffect("Debugger\'s Futility Effect", 2, 30.0f, 9.0f, 3.0f);// damage ^^
        oneOffHealEffect = new AbilityEffect("Quality Assured Effect", 1, 13.0f, 0.0f, 0.0f);
        hotEffect = new AbilityEffect("Sisyphean Resolve Effect", 3, 25.0f, 4.0f, 1.0f);// heals ^^

        castedAbility = new Ability("Testerbolt", oneOffDamageEffect, 1.5f);
        instantAbility = new Ability("Debugger\'s Futility", dotEffect, 0.0f);
        castedHeal = new Ability("Quality Assured", oneOffHealEffect, 1.5f);
        instantHeal = new Ability("Sisyphean Resolve Effect", hotEffect, 0.0f);
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetKeyDown("1")){
            queueAbility(instantAbility);
        }
        if(Input.GetKeyDown("2")){       
            queueAbility(castedAbility);
        }
        if(Input.GetKeyDown("3")){
            queueAbility(castedHeal);
        }
        if(Input.GetKeyDown("4")){
            queueAbility(instantHeal);
        }
        
        if(castCompleted){
            Debug.Log("castCompleted: " + queuedAbility.getName());
            castAbility(queuedAbility);
            castCompleted = false;

        }
    }

    void castAbility(Ability inAbility){

        uiManager.targetFrame.actor.applyAbilityEffect(inAbility.getEffect(), player.GetComponent<Actor>());

    }

    void queueAbility(Ability inAbility){
        if(isCasting){
            Debug.Log("You are casting!");
        }
        else{
            if(target != null){ // Change to PlayerControllerHBC?

                
                if(inAbility.getCastTime() > 0.0f){ // Casted Ability

                    //Debug.Log("Trying to create a castBar for " + inAbility.getName());

                    //Preparing variables for cast
                    queuedAbility = inAbility;
                    castCompleted = false; // for saftey
                    isCasting = true;

                    //Creating cast bar and setting it's parent to canvas to display it properly
                    GameObject newAbilityCast = Instantiate(uiManager.castBarPrefab, uiManager.canvas.transform);
                    // v (string cast_name, Actor from_caster, Actor to_target, float cast_time) v
                    newAbilityCast.GetComponent<CastBar>().Init(inAbility.getName(), player,
                                                                    uiManager.targetFrame.actor, inAbility.getCastTime());// change so that it is target in here later
                    
                }
                else{
                    Debug.Log("GM| Instant cast: " + inAbility.getName());
                    queuedAbility = inAbility;
                    castCompleted = true;
                }
            }
            else{
                Debug.Log("You don't have a target!");
            }
        }
        
    }
}
