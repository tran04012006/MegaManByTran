using UnityEngine;

public class PlayerRun : IPlayerState
{
    private PlayerController player;
    private Animator _animator;

    //constructure
    public PlayerRun(PlayerController player)
    {
        this.player = player;
        _animator = player.GetComponent<Animator>();
    }

    public void Enter()
    {
        _animator.SetInteger("Status", 1);
    }

    public void Update()
    {        
        Debug.Log("chay " + this);

        if (player.isMoving == false)
        {
            player.ChangeState(player._playerIdle);
        }
        
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
    }

    public void Exit()
    {
        
    }
}
