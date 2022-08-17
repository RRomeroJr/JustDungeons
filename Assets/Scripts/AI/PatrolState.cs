using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        enemy.unit.RequestPath(enemy.unit.spawnLocation);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        // If enemy has a target
        if (enemy.unit.target != null)
        {
            // Check if target is behind an obstacle, if not, go to moveTowardsState. Else, go to pathfindingState
            if (!enemy.unit.targetBehindObstacle())
            {
                enemy.SwitchState(enemy.moveTowardsState);
                return;
            }
            enemy.SwitchState(enemy.pathfindingState);
            return;
        }

        // Do nothing until pathfinding to spawn location is complete
        else if (enemy.unit.coroutine != null)
        {

        }

        else
        {

        }
    }
}
