using UnityEngine;

public class MoveStrategy : IEnemyMoveStrategy
{
    private EnemyControllerNew enemy;
    private Vector2 direction;

    public  MoveStrategy(EnemyControllerNew enemy, Vector2 direction)
    {
        this.enemy = enemy;
        this.direction = direction;
    }
    
    public void Move()
    {
        enemy.rb.linearVelocity = direction * enemy.moveSpeed;
    }
}
