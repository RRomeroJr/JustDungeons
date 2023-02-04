using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshStatusViewer: MonoBehaviour
{
	public NavMeshAgent navAgent;
    public bool hasPath;
    public bool isStopped;
    public UnityEngine.AI.NavMeshPathStatus pathSatus;
    public Vector3 destination;
    void Awake(){
        navAgent = GetComponent<NavMeshAgent>();
    }
    void Update(){
        hasPath = navAgent.hasPath;
        isStopped = navAgent.isStopped;
        pathSatus = navAgent.pathStatus;
        destination = navAgent.destination;

    }
}