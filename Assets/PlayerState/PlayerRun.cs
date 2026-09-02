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
    }

    public void Exit()
    {
        
    }
}
