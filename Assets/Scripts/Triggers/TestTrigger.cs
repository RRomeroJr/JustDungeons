using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TestTrigger : Trigger
{
    public bool SetActive = false;
    public override bool TriggerCheck()
    {
        return SetActive;
    }
}