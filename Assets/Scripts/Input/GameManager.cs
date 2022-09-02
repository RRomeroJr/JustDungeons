using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UIManager uiManager;
    public GameObject player;
    //Assume that actor get you a COPY of the abililty that they want to cast
    // or.. I could copy it for them... and replace the ref with ref keyword
    public ActorCastedAbilityEvent actorCastedAbility_Event;

    void Awake(){
        instance = this;
    }

    void Start()
    {
        actorCastedAbility_Event = new ActorCastedAbilityEvent();
        actorCastedAbility_Event.AddListener(applyEffects);
    }
    
    
    void Update()
    {
    }

    public void applyEffects(List<AbilityEffect> _effects){
        Debug.Log("List of size: " + _effects.Count.ToString() + " Trying to apply to Actor: " + _effects[0].getTarget().getActorName());
    }
    
}
