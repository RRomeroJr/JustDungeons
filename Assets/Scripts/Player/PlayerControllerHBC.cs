using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerHBC : MonoBehaviour
{
    
    
    
    public Actor player;
    public UIManager uiManager;
    Ability ability1;
    Ability ability2;
    Ability ability3;
    Ability ability4;
    Ability ability5;
    Ability ability6;
    Ability ability7;
    Ability ability8;
    void Start()
    {
        ability1 = PlayerAbilityData.DoT;
        ability2 = PlayerAbilityData.CastedDamage;
        ability3 = PlayerAbilityData.CastedHeal;
        ability4 = PlayerAbilityData.HoT;
        ability5 = PlayerAbilityData.AoE;
        ability6 = PlayerAbilityData.FreeAbilityIfHit;
        ability7 = PlayerAbilityData.DoubleEffectAbility;
        ability8 = PlayerAbilityData.DelayedDamage;
        Debug.Log(" 1 = DoT");
        Debug.Log(" 2 = One off Dmg");
        Debug.Log(" 3 = One off Heal");
        Debug.Log(" 4 = HoT");
        Debug.Log(" 5 = AoE (Doesn't disapear yet)");
        Debug.Log(" 6 = Skill Shot that fires off a 2nd 1 off if it hits an Actor");
        Debug.Log(" 7 = Ability with 2 effects (1 off heal then DoT)");
        Debug.Log(" 8 = 1 off Dmg that hits after a delay (4s)");
    }


    void Update()
    {    
        if(Input.GetKeyDown("1")){
            player.castAbility(ability1, player.target);
        }
        if(Input.GetKeyDown("2")){       
            player.castAbility(ability2, player.target);
        }
        if(Input.GetKeyDown("3")){
            player.castAbility(ability3, player.target);
        }
        if(Input.GetKeyDown("4")){
            player.castAbility(ability4, player.target);
        }
        if(Input.GetKeyDown("5")){
            player.castAbility(ability5, getWorldPointTarget());
        }
        if(Input.GetKeyDown("6")){
            player.castAbility(ability6, player.target);
        }
        if(Input.GetKeyDown("7")){
            player.castAbility(ability7, player.target);
        }
        if(Input.GetKeyDown("8")){
            player.castAbility(ability8, player.target);
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
