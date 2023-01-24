using UnityEngine;
using System.Collections;

public class MoveTowardsState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {

    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        // If no target is set, go to patrolState
        if (enemy.unit.target == null)
        {
            enemy.SwitchState(enemy.patrolState);
            return;
        }

        // If target is behind an obstacle, move to pathfindingState
        if (enemy.unit.targetBehindObstacle())
        {
            enemy.SwitchState(enemy.pathfindingState);
            return;
        }

        enemy.unit.MoveTowards();
    }
}
