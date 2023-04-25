using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;

    public Vector3 offset = new Vector3(0.0f, 0.0f, -10.0f);
    public Vector2 dragOffset = new Vector2(0.0f,0.0f);
    public float dragSens = 0.1f;
    public float dragMax = 1.5f;
    void Start(){
        //offset = new Vector3(target.transform.position.x, target.transform.position.y + 2.75f, -10f);
        CustomNetworkManager.singleton.GamePlayers.CollectionChanged += HandlePlayerAdded;
        
    }
    void Update(){
        if(Input.GetMouseButton(1)){
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
        if(Input.GetMouseButton(2)){
            dragOffset = Vector2.zero;
        }
    }
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position + (Vector3)dragOffset + offset;
            Vector3 newTargetPos;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
            Vector2 mousePos = HBCTools.GetMousePosWP();
            if((mousePos - (Vector2)targetPos).magnitude < 4.0f){
                newTargetPos = mousePos;
            }
            else{
                newTargetPos = (Vector3)Vector2.ClampMagnitude((mousePos - (Vector2)targetPos), 4.0f) + targetPos;
            }
            //Debug.DrawLine(target.position, newTargetPos, Color.red);
            transform.position = smoothedPos;
        }
    }

    void HandlePlayerAdded(object sender, EventArgs e)
    {
        if (CustomNetworkManager.singleton.GamePlayers.Last().isLocalPlayer)
        {
            target = CustomNetworkManager.singleton.GamePlayers.Last().transform;
            offset.y = -1 * target.GetComponent<Renderer>().bounds.extents.y;
        }
    }
}
