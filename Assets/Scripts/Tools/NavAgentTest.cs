using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentTest: MonoBehaviour
{
    NavMeshAgent navAgent;
    void Awake(){
        navAgent = GetComponent<NavMeshAgent>();
    }
    void Update(){
        if(Input.GetKeyDown("u")){
            navAgent.SetDestination(HBCTools.GetMousePosWP());
        }
    }
	
}