using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerHBC : MonoBehaviour
{
    
    
    
    public Actor player;
    public UIManager uiManager;

    
 
    void Start()
    {     
    }

    // Update is called once per frame
    void Update()
    {    
        if(Input.GetKeyDown("1")){
            player.checkAndQueue(PlayerAbilityData._instantAbility);
        }
        if(Input.GetKeyDown("2")){       
            player.checkAndQueue(PlayerAbilityData._castedDamage);
        }
        if(Input.GetKeyDown("3")){
            player.checkAndQueue(PlayerAbilityData._castedHeal);
        }
        if(Input.GetKeyDown("4")){
            player.checkAndQueue(PlayerAbilityData._instantAbility2);
        }
        
        
    }

    
}
