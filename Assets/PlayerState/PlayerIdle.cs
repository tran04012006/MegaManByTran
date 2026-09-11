using UnityEngine;

public class PlayerIdle : IPlayerState
{
    private PlayerController player;
    private Animator _animator;

    //constructure
    public PlayerIdle(PlayerController player)
    {
        this.player = player;
        _animator = player.GetComponent<Animator>();
    }

    public void Enter()
    {
        _animator.SetInteger("Status", 0);
    }

    public void Update()
    {
        Debug.Log("chay " + this);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player.isGround == true)
            {
                player.ChangeState(player.playerShoot);
            }
            else
            {
                if (player.isClimb == false)
                    player.ChangeState(player.playerJumpShoot);
                else
                {
                    player.ChangeState(player.playerClimbShoot);
                }
            }
        }
        
        if (player.moveX != 0)
        {
            player.ChangeState(player.playerRun);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (player.isClimb == true)
            {
                player.ChangeState(player.playerClimb);
            }
            
            if (player.isClimb == false && player.isGround == true)
            {
                player.isGround = false;
                player.Jump();
                player.ChangeState(player.playerJump);
            }
        }
        
        if (player.dead == true)
        {
            player.ChangeState(player.playerDie);
        }
    }

    public void Exit()
    {

    }
}
