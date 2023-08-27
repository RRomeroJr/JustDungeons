using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;
using System;


public class ClickManager : NetworkBehaviour
{
    public float clickWindow = 0.66f;
    public float clickTravelWindow = 66.0f;
    public ClickData clickData0 = new ClickData();
    public ClickData clickData1 = new ClickData();
    public LayerMask targetingLayerMask;

    public void Awake()
    {
        targetingLayerMask = LayerMask.GetMask("UI");
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickData0.ClickStart();
        }
        if (Input.GetMouseButtonDown(1))
        {
            clickData1.ClickStart();
        }
    }
    /// <summary>
    ///	If the mouse button is held and mouse posistion moved atleast the clickTravelWindow distance
    /// </summary>
    public bool MouseButtonDrag(int _buttonId)
    {
        switch (_buttonId)
        {
            case (0):
                return (Input.GetMouseButton(0) && clickData0.CalcTravel() >= clickTravelWindow);
                break;
            case (1):
                return (Input.GetMouseButton(1) && clickData1.CalcTravel() >= clickTravelWindow);
                break;
            default:
                Debug.LogError("Unknown mouse button for click");
                break;
        }
        Debug.Log("CM drag: " + false);
        return false;
    }
    /// <summary>
    ///	If the mouse button was held for less than or equal to the clickWindow
    /// </summary>
    public bool MouseButtonShort(int _buttonId)
    {
        switch (_buttonId)
        {
            case (0):
                return clickData0.CalcHoldTime() <= clickWindow;
                break;
            case (1):
                return clickData1.CalcHoldTime() <= clickWindow;
                break;
            default:
                Debug.LogError("Unknown mouse button for click");
                break;
        }
        Debug.Log("CM short: " + false);
        return false;
    }
    public Vector2 MouseButtonDragVector(int _buttonId)
    {
        switch (_buttonId)
        {
            case (0):
                return Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(clickData0.startPos);
                break;
            case (1):
                return Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(clickData1.startPos);
                break;
            default:
                Debug.LogError("Unknown mouse button for Drag vector");
                break;
        }
        return Vector2.zero;
    }
    public bool MouseButtonHold(int _buttonId)
    {
        switch (_buttonId)
        {
            case (0):
                return (Input.GetMouseButton(0) && clickData0.CalcHoldTime() <= clickWindow);
                break;
            case (1):
                return (Input.GetMouseButton(1) && clickData1.CalcHoldTime() <= clickWindow);
                break;
            default:
                Debug.LogError("Unknown mouse button for click");
                break;
        }
        return false;
    }

}
