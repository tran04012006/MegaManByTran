using UnityEngine;

public class PlayerClimbShoot : IPlayerState
{
    private PlayerController player;
    private Animator _animator;
    private float timer;
    private float duration = 0.25f;

    //constructure
    public PlayerClimbShoot(PlayerController player)
    {
        this.player = player;
        _animator = player.GetComponent<Animator>();
    }

    public void Enter()
    {
        Debug.Log("dang chay " + this);
        _animator.SetInteger("Status", 11);
        player.ShootBullet();
        timer = 0;
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            player.ChangeState(player.playerClimb);
        }
    }

    public void Exit()
    {

    }
}
