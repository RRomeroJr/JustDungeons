using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentTest: MonoBehaviour
{
    NavMeshAgent navAgent;
    public float autoVertDisty = 20.0f;
    public float autoVertDistx = 0.0f;
    void Awake(){
        navAgent = GetComponent<NavMeshAgent>();
        if(navAgent.enabled = false){
            navAgent.enabled = true;
        }
    }
    void Update(){
        if(Input.GetKeyDown("u")){
            navAgent.SetDestination(HBCTools.GetMousePosWP());
        }
        if(Input.GetKeyDown("0")){
            navAgent.SetDestination(new Vector3(transform.position.x + autoVertDistx, transform.position.y + autoVertDisty, 0.0f));
        }
        if(Input.GetKeyDown("9")){
            navAgent.SetDestination(new Vector3(transform.position.x - autoVertDistx, transform.position.y - autoVertDisty, 0.0f));
        }
    }
	
}