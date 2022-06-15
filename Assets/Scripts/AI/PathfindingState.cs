using UnityEngine;

public class PathfindingState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {

    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.unit.target == null)
        {
            enemy.SwitchState(enemy.patrolState);
            enemy.unit.StopPathfinding();
            return;
        }

        if (!enemy.unit.targetBehindObstacle())
        {
            enemy.SwitchState(enemy.moveTowardsState);
            enemy.unit.StopPathfinding();
            return;
        }

        if (enemy.unit.target.position != enemy.unit.targetPrev)
        {
            enemy.unit.RequestPath();
        }
    }
}
