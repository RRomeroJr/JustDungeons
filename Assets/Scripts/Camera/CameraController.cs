using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    const float OFFSET_ADJUST = 0.01f;
    public Transform target;
    public float smoothSpeed = 0.125f;

    public Vector3 offset = new Vector3(0.0f, 0.0f, -10.0f);
    public Vector2 dragOffset = new Vector2(0.0f,0.0f);
    public float dragSens = 0.1f;
    public float dragMax = 1.5f;
    public bool dragMode = false;
    void Start(){
        //offset = new Vector3(target.transform.position.x, target.transform.position.y + 2.75f, -10f);
        CustomNetworkManager.singleton.GamePlayers.CollectionChanged += HandlePlayerAdded;
        
    }
    void OnDestroy()
    {
        CustomNetworkManager.singleton.GamePlayers.CollectionChanged -= HandlePlayerAdded;
    }
    void LateUpdate(){

        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            if( Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                // dragOffset = Vector2.ClampMagnitude(HBCTools.GetMousePosWP() - target.position, dragMax);
            }
            
        }
        if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)){
            if( !(UIManager.Instance.MouseButtonDrag(0) || UIManager.Instance.MouseButtonDrag(1)) )
            {
                dragMode = false;
                UnlockCursor();
            }
            
        }
        if(UIManager.Instance.draggingObject == false)
        {
            if(dragMode == false)
            {
                if((UIManager.Instance.MouseButtonDrag(0) || UIManager.Instance.MouseButtonDrag(1)))
                {
                    // dragOffset = Vector2.ClampMagnitude(HBCTools.GetMousePosWP() - target.position, dragMax);
                    dragMode = true;
                    LockCursor();
                }
                if(UIManager.Instance.MouseButtonHold(0) || UIManager.Instance.MouseButtonHold(1))
                {
                    // dragOffset = Vector2.ClampMagnitude(HBCTools.GetMousePosWP() - target.position, dragMax);
                    dragMode = true;
                    LockCursor();
                }

            }
            if (dragMode)
            {
                Vector2 mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
                // if(mouseMove.magnitude > 0){
                //     mouseMove.Normalize();
                //     mouseMove = dragMax * mouseMove;
                //     dragOffset = mouseMove;
                // }
                
                mouseMove = dragSens * mouseMove;
                dragOffset = dragOffset + mouseMove;
                dragOffset = Vector2.ClampMagnitude(dragOffset, dragMax);

            }
        }
        if(Input.GetMouseButton(2) || Input.GetKeyDown(KeyCode.Space)){
            dragOffset = Vector2.zero;
        }
    }
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position + (Vector3)dragOffset +
                offset;
            Vector3 newTargetPos;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
            // Vector2 mousePos = HBCTools.GetMousePosWP();
            // if((mousePos - (Vector2)targetPos).magnitude < 4.0f){
            //     newTargetPos = mousePos;
            // }
            // else{
            //     newTargetPos = (Vector3)Vector2.ClampMagnitude((mousePos - (Vector2)targetPos), 4.0f) + targetPos;
            // }
            //Debug.DrawLine(target.position, newTargetPos, Color.red);
            transform.position = smoothedPos;
        }
    }

    void HandlePlayerAdded(object sender, EventArgs e)
    {
        foreach(var ele in CustomNetworkManager.singleton.GamePlayers)
        {
            if (ele.isLocalPlayer)
            {
                target = ele.transform;
                // offset.y = -1 * target.GetComponent<Renderer>().bounds.extents.y + OFFSET_ADJUST;
                break;
            }

        }
    }
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    void ToggleCursorLock(){
        if(Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if(Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
