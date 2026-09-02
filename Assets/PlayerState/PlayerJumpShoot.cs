using UnityEngine;

public class PlayerJumpShoot : IPlayerState
{
    private PlayerController player;
    private Animator _animator;
    private float timer;
    private float duration = 2f;

    //constructure
    public PlayerJumpShoot(PlayerController player)
    {
        this.player = player;
        _animator = player.GetComponent<Animator>();
    }

    public void Enter()
    {
        Debug.Log("status la nhay, luc nay isGround = " + player.isGround);
        _animator.SetInteger("Status", 12);
        player.ShootBullet();
    }

    public void Update()
    {
        if (player.isGround == true)
        {
            Debug.Log("status la ko nhay nua");
            if (player.moveX == 0)
            {
                player.ChangeState(player._playerIdle);
            }
            else
            {
                player.ChangeState(player.playerRun);
            }
        }
        
    }

    public void Exit()
    {

    }
}
