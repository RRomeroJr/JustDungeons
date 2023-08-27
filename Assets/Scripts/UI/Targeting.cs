using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using System;
using System.Net.WebSockets;

public static class Targeting
{
    
    public static Actor LookForNewTarget()
    {
        
        UnitFrame graphicsResult = UIManager.Instance.canvas.GetComponent<UIRaycaster>().GraphicsRaycastToUnitFrame();
        if(graphicsResult)
        {
            return graphicsResult.actor;
        }

        return SearchForActorPhysics2D(UIManager.Instance.clickManager.targetingLayerMask);
    }
    /// <summary>
    /// Return the first actor hit but a Physics2D raycast
    /// </summary>
    public static Actor SearchForActorPhysics2D(LayerMask _targetingMask)
    {
        var hit = Physics2D.Raycast(HBCTools.GetMousePosWP(), Vector2.zero, Mathf.Infinity, _targetingMask);
        try
        {
            return hit.collider.transform.parent.GetComponent<Actor>();
        }
        catch
        {
            return null;
        }
    }
    // public static RaycastHit2D raycastToActors(Vector2 _loc, LayerMask _clickMask)
    // {//Change this to raycastToType to make it more general
    //     return Physics2D.Raycast(_loc, Vector2.zero, Mathf.Infinity, _clickMask);
    // }
}