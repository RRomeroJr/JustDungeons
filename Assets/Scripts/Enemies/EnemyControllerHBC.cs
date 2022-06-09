using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerHBC : MonoBehaviour
{
    // When castCompleted is true queueAbility will fire
    public bool castReady = false; // Will only be set TRUE by CastBar
    public bool isCasting = false; // Will only be set FALSE by CastBar 
    public Ability queuedAbility;
    
    public Actor enemyActor;
    //public UIManager uiManager;


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
        enemyActor = gameObject.GetComponent<Actor>();
        
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

        queuedAbility = castedAbility;
        StartCoroutine(castReadyXSecs(4.20f));
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }

    // Update is called once per frame
    void Update()
    {   
        
        if(castReady){
            //Debug.Log("castCompleted: " + queuedAbility.getName());
            enemyActor.castAbility(queuedAbility, enemyActor.target);
            castReady = false;

        }
    }
/*
    void queueAbility(Ability inAbility){ //doesn't matter yet
        if(isCasting){
            //Debug.Log("Enemy is casting!");
        }
        else{
            if(enemyActor.target != null){ // Change to PlayerControllerHBC?

                
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
    }*/

    IEnumerator castReadyXSecs(float x){
        while(x>0){
            yield return new WaitForSeconds(x);
            castReady = true;
        }
        
    }
}


