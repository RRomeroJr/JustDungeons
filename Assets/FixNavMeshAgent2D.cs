using UnityEngine;
using UnityEngine.AI;

public class FixNavMeshAgent2D : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
}
