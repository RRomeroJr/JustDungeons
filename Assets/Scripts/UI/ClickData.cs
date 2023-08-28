using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using System;
[System.Serializable]
public class ClickData
{
    public float timeStart;
    public Vector2 startPos;
    public void ClickStart()
    {
        // Debug.Log("ClickStart");
        timeStart = Time.time;
        startPos = Input.mousePosition;
    }
    public float CalcTravel()
    {
        return (startPos - (Vector2)Input.mousePosition).magnitude;
    }
    public float CalcHoldTime()
    {
        return Time.time - timeStart;
    }
    public void Reset()
    {
        timeStart = 0.0f;
        startPos = Vector2.zero;
    }
}