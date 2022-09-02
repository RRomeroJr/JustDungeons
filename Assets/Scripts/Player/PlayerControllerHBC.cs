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
    Ability ability9;
    Ability ability10;
    Ability ability11;
    public Ability_V2 ability1a;
    public Ability_V2 ability2a;
    public Ability_V2 ability3a;
    public Ability_V2 ability4a;
    public Ability_V2 ability5a;
    public Ability_V2 ability6a;
    public Ability_V2 ability7a;
    public Ability_V2 ability8a;
    public Ability_V2 ability9a;

    void Start()
    {   
        
        // ability1 = PlayerAbilityData.DoT;
        // ability2 = PlayerAbilityData.CastedDamage;
        // ability3 = PlayerAbilityData.CastedHeal;
        // ability4 = PlayerAbilityData.HoT;
        // ability5 = PlayerAbilityData.AoE;
        // ability6 = PlayerAbilityData.FreeAbilityIfHit;
        // ability7 = PlayerAbilityData.DoubleEffectAbility;
        // ability8 = PlayerAbilityData.DelayedDamage;
        // ability9 = PlayerAbilityData.Teleport;
        // ability10 = PlayerAbilityData.Dash;
        // ability11 = PlayerAbilityData.DmgBuffBolt;
        
        

        // Debug.Log(" 1 = DoT");
        // Debug.Log(" 2 = One off Dmg");
        // Debug.Log(" 3 = One off Heal");
        // Debug.Log(" 4 = HoT");
        // Debug.Log(" 5 = AoE (Doesn't disapear yet)");
        // Debug.Log(" 6 = Skill Shot that fires off a 2nd 1 off if it hits an Actor");
        // Debug.Log(" 7 = Ability with 2 effects (1 off heal then DoT)");
        // Debug.Log(" 8 = 1 off Dmg that hits after a delay (4s)");
        // Debug.Log(" R = Teleport");
        // Debug.Log(" F = Dash");
    }


    void Update()
    {    
        if(Input.GetKeyDown("1")){
            if(ability1a != null)
            player.castAbility2(ability1a);
        }
        if(Input.GetKeyDown("2")){
            if(ability2a != null)
            player.castAbility2(ability2a);
        }
        if(Input.GetKeyDown("3")){
            if(ability3a != null)
            player.castAbility2(ability3a);
        }
        if(Input.GetKeyDown("4")){
            if(ability4a != null)
            player.castAbility2(ability4a);
        }
        if(Input.GetKeyDown("5")){
            if(ability5a != null)
            player.castAbility2(ability5a);
        }
        if(Input.GetKeyDown("6")){
            if(ability6a != null)
            player.castAbility2(ability6a);
        }
        if(Input.GetKeyDown("7")){
            if(ability7a != null)
            player.castAbility2(ability7a);
        }
        if(Input.GetKeyDown("8")){
            if(ability8a != null)
            player.castAbility2(ability8a);
        }
        if(Input.GetKeyDown("9")){
            if(ability9a != null)
            player.castAbility2(ability9a);
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
