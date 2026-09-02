using UnityEngine;

public class PlayerHurt : IPlayerState
{
    private PlayerController player;
    private Animator _animator;
    private float timer;
    private float duration = 0.5f;

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
