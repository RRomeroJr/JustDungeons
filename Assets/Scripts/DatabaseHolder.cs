using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseHolder : MonoBehaviour
{
    /*
        RR: I could not find a way to insure that AbilityData and AbilityEffectData scriptable objects remain loaded
        when the game starts for both host and clients. As a work around I going to keep references here to them both to 
        Make sure that unity does not destroy them and always has them loaded.
    */
    public AbilityData adSingleton;
    public AbilityEffectData aedSingleton;
}
