using UnityEngine;

public class PathfindingState : EnemyBaseState
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

        // If target is not behind an obstacle, go to moveTowardsState and cancel pathfinding
        if (!enemy.unit.targetBehindObstacle())
        {
            enemy.SwitchState(enemy.moveTowardsState);
            return;
        }

        // If target is in a new position, request new path
        if (enemy.unit.target.position != enemy.unit.targetPrev)
        {
            enemy.unit.RequestPath();
        }
    }
}
