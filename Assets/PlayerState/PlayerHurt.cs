using UnityEngine;

public class PlayerHurt : IPlayerState
{
    private PlayerController player;
    private Animator _animator;
    private float timer;
    private float duration = 0.4f;

    //constructure
    public PlayerHurt(PlayerController player)
    {
        this.player = player;
        _animator = player.GetComponent<Animator>();
     
    }

    public void Enter()
    {
        Debug.Log("chay " + this);
        timer = 0;
        _animator.SetInteger("Status", 3);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.ChangeState(player.playerShoot);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            player.ChangeState(player.playerJump);
        }
        
        if (player.moveX == 0)
        {
            player.ChangeState(player.playerRun);
        } 
        
        timer += Time.deltaTime;
        if (timer >= duration)
        {
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
