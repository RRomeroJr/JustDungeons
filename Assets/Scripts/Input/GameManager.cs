using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public UIManager uiManager;
    public GameObject player;
    //Assume that actor get you a COPY of the abililty that they want to cast
    // or.. I could copy it for them... and replace the ref with ref keyword
    public ActorCastedAbilityEvent actorCastedAbility_Event;

    

    void Start()
    {
        actorCastedAbility_Event = new ActorCastedAbilityEvent();
    }
    
    
    void Update()
    {
    }

    
    
}
