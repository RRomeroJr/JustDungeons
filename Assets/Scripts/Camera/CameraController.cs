using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;

    public Vector3 offset = new Vector3(0.0f, 2.75f, -10.0f);

    void Start(){
        //offset = new Vector3(target.transform.position.x, target.transform.position.y + 2.75f, -10f);
        CustomNetworkManager.singleton.GamePlayers.CollectionChanged += HandlePlayerAdded;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed);

            transform.position = smoothedPos;
        }
    }

    void HandlePlayerAdded(object sender, EventArgs e)
    {
        if (CustomNetworkManager.singleton.GamePlayers.Last().isLocalPlayer)
        {
            target = CustomNetworkManager.singleton.GamePlayers.Last().transform;
        }
    }
}
