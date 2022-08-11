using UnityEngine;

public class MoveTowardsState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {

    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.unit.target == null)
        {
            enemy.SwitchState(enemy.patrolState);
            return;
        }

        if (enemy.unit.targetBehindObstacle())
        {
            enemy.SwitchState(enemy.pathfindingState);
            return;
        }
        Debug.Log("here");
        enemy.unit.MoveTowards();
    }
}
