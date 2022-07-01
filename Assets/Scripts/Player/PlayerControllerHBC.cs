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
    public Ability aoeTBolt;
    public AbilityEffect doubleTesterboltEffect;
    public AbilityEffect dashToPointEffect;

    
 
    void Start()
    {

        dashToPointEffect = new AbilityEffect("DashToPoint Effect", 4, 0.01f, 0.25f, 0.0f);
        
                //    This doesn't work without making a clone function
        doubleTesterboltEffect = AbilityEffectData._oneOffDamageEffect.clone();
        doubleTesterboltEffect.finishAction = AbilityEffectData._oneOffDamageEffect.secondaryTestbolt;

        doubleTesterbolt = PlayerAbilityData._castedDamage.clone();
        doubleTesterbolt.DeliveryType = 1;
        doubleTesterbolt.setName("Double tester!");
        doubleTesterbolt.setEffect(doubleTesterboltEffect);
        //AbilityEffectData._oneOffHealEffect.finishAction = AbilityEffectData._oneOffHealEffect.delegateboltFinish;
        aoeTBolt = PlayerAbilityData._castedDamage.clone();
        aoeTBolt.DeliveryType = 2;
        aoeTBolt.setName("AoE TB");
    }


    void Update()
    {    
        if(Input.GetKeyDown("1")){
            player.castAbility(PlayerAbilityData._instantAbility, player.target);
        }
        if(Input.GetKeyDown("2")){       
            player.castAbility(PlayerAbilityData._castedDamage, player.target);
        }
        if(Input.GetKeyDown("3")){
            player.castAbility(PlayerAbilityData._castedHeal, player.target);
        }
        if(Input.GetKeyDown("4")){
            player.castAbility(PlayerAbilityData._instantAbility2, player.target);
        }
        if(Input.GetKeyDown("5")){
            player.castAbility(aoeTBolt, getWorldPointTarget());
        }
        if(Input.GetKeyDown("6")){
            player.castAbility(doubleTesterbolt, player.target);
        }
        
    }
    void sayAbilityEffectName(AbilityEffect _abilityEffect){
        Debug.Log("This effect " + _abilityEffect.getEffectName() + "  finished!");
    }
    Vector3 getWorldPointTarget(){
        Vector3 scrnPos = Input.mousePosition;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(scrnPos);
        worldPoint.z = 0.0f;
        return worldPoint;
    }
}
