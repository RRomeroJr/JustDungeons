using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerHBC : MonoBehaviour
{
    
    
    
    public Actor player;
    public UIManager uiManager;
    public Ability finishbolt;
    public Ability secondarybolt;
    public Ability doubleTesterbolt;
    public AbilityEffect doubleTesterboltEffect;
    
 
    void Start()
    {

                //    This doesn't work without making a clone function
        doubleTesterboltEffect = AbilityEffectData._oneOffDamageEffect.clone();
        doubleTesterboltEffect.finishAction = AbilityEffectData._oneOffDamageEffect.secondaryTestbolt;

        doubleTesterbolt = PlayerAbilityData._castedDamage.clone();
        doubleTesterbolt.setName("Double tester!");
        doubleTesterbolt.setEffect(doubleTesterboltEffect);
        //AbilityEffectData._oneOffHealEffect.finishAction = AbilityEffectData._oneOffHealEffect.delegateboltFinish;
    }


    void Update()
    {    
        if(Input.GetKeyDown("1")){
            player.checkAndQueue(PlayerAbilityData._instantAbility, player.target);
        }
        if(Input.GetKeyDown("2")){       
            player.checkAndQueue(PlayerAbilityData._castedDamage, player.target);
        }
        if(Input.GetKeyDown("3")){
            player.checkAndQueue(PlayerAbilityData._castedHeal, player.target);
        }
        if(Input.GetKeyDown("4")){
            player.checkAndQueue(PlayerAbilityData._instantAbility2, player.target);
        }
        if(Input.GetKeyDown("6")){
            player.checkAndQueue(doubleTesterbolt, player.target);
        }
        
    }
    void sayAbilityEffectName(AbilityEffect _abilityEffect){
        Debug.Log("This effect " + _abilityEffect.getEffectName() + "  finished!");
    }
}
