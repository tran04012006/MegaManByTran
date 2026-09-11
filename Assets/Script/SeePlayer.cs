using UnityEngine;

public class SeePlayer : IEnemyTrigger
{
    private EnemyControllerNew enemy;
    private GameObject player;
    private float detectDistance;

    public SeePlayer(EnemyControllerNew enemy, GameObject player,
        float detectDistance)
    {
        this.enemy = enemy;
        this.player = player;
        this.detectDistance = detectDistance;
    }
    
    public bool Trigger()
    {
        if (Vector2.Distance(player.transform.position,
                enemy.transform.position) <= detectDistance)
        {
            return true;
        }

        return false;
    }
}
