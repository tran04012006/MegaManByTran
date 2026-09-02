using UnityEngine;

public class PlayerClimb : IPlayerState
{
    private PlayerController player;
    private Animator _animator;

    //constructure
    public PlayerClimb(PlayerController player)
    {
        this.player = player;
        _animator = player.GetComponent<Animator>();
    }

    public void Enter()
    {
        Debug.Log("dang chay " + this);
        _animator.SetInteger("Status", 4);
    }

    public void Update()
    {
        player.Climb();
        if (player.isClimbEnd == true)
        {
            player.ChangeState(player.climbEndPoint);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }

    public void Exit()
    {

    }
}
