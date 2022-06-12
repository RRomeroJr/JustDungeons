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

    // Start is called before the first frame update
    void Start()
    {   
        enemyActor = gameObject.GetComponent<Actor>();
        
        queuedAbility = PlayerAbilityData._castedDamage;
        StartCoroutine(castReadyXSecs(4.20f));
       
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
/*   //                                                        doesn't matter yet
    void queueAbility(Ability inAbility){ 
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


