using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public Unit unit;
    public EnemyBaseState currentState;
    public string myState;
    public PatrolState patrolState = new PatrolState();
    public PathfindingState pathfindingState = new PathfindingState();
    public MoveTowardsState moveTowardsState = new MoveTowardsState();

    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
        currentState = patrolState;
        currentState.EnterState(this);
        myState = currentState.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
        myState = currentState.ToString();
    }
}
