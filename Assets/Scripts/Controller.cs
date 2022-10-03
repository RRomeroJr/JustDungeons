using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class Controller : NetworkBehaviour
{   float tempspeed = 0.02f;
    public Actor actor;
    public NavMeshAgent agent;
    public void MoveTowards(Vector3 pos){
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, pos, tempspeed);
    }
}
