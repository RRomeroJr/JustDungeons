using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerHBC : MonoBehaviour
{
    // When castCompleted is true queuedAbility will fire
    public bool readyToFire = false; // Will only be set TRUE by CastBar
    public bool isCasting = false; // Will only be set FALSE by CastBar 
    public Ability queuedAbility;
    public Actor actor;
    public EnemySO enemyStats;

    
    //public UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {   
        actor = gameObject.GetComponent<Actor>();
        
        queuedAbility = PlayerAbilityData.CastedDamage;
        //StartCoroutine(tryCastEveryXSecs(queuedAbility, 9.5f));
       
    }
    void Update()
    {   
        //actor.castAbility(queuedAbility, actor.target);
    }

    IEnumerator tryCastEveryXSecs(Ability _ability, float x){
        while(x>0){
            yield return new WaitForSeconds(x);
            actor.castAbility(queuedAbility, actor.target);
        }
        
    }
    IEnumerator tryCastOnCooldown(Ability _ability){
        while(_ability.getCooldown()>0){
            yield return new WaitForSeconds(_ability.getCooldown()+_ability.getCastTime() + 0.02f);
             actor.castAbility(queuedAbility, actor.target);
        }
        
    }
}


