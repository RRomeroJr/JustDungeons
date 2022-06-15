using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {

    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.unit.target != null)
        {
            if (!enemy.unit.targetBehindObstacle())
            {
                enemy.SwitchState(enemy.moveTowardsState);
                return;
            }
            enemy.SwitchState(enemy.pathfindingState);
            return;
        }
    }
}
