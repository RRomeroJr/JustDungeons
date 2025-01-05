using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Mirror;
using UnityEditor;
using System.IO;

public class DevCaster : MonoBehaviour
{
    public Ability_V2 a1;
    void Update()
    {
        if(Input.GetKeyDown(";"))
        {
            Debug.Log($"{name} is casting {a1.name}");
            GetComponent<Actor>().castAbility3(a1, UIManager.playerActor.transform);
            GetComponent<EnemyController>().autoAttacking = false;

        }
    }
}