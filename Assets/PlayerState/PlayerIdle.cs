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
        if (player.moveX != 0)
        {
            player.ChangeState(player.playerRun);
        }
    }

    public void Exit()
    {

    }
}
