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

        // Color lineColor;
        // if(hasPath){
        //     if(isStopped){
        //         lineColor = Color.red;
        //     }
        //     else{
        //         lineColor = Color.white;
        //     }
        // }
        // else{
        //     if(isStopped){
        //         lineColor = Color.blue;
        //     }
        //     else{
        //         lineColor = Color.magenta;
        //     }
        // }
        

        // Debug.DrawLine(transform.position, destination, lineColor);

    }
}