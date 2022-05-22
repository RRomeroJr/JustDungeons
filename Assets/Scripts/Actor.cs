using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
     Container many for any RPG related elements
*/

public class Actor : MonoBehaviour
{
    public string name;
    public FloatReference health;
    public float maxHealth;
    public float mana;
    public float maxMana;

    public Color unitColor;

    void takeDamage(float amount){
        
    }

}
