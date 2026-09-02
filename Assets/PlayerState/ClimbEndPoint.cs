using System.Collections;
using UnityEngine;

public class ClimbEndPoint : IPlayerState
{
    private PlayerController player;
    private Animator _animator;

    //constructure
    public ClimbEndPoint(PlayerController player)
    {
        this.player = player;
        _animator = player.GetComponent<Animator>();
    }

    public void Enter()
    {
        Debug.Log("dang chay " + this);
        _animator.SetInteger("Status", 10);

        player.StartCoroutine(EndClimb());
    }

    public void Update()
    {
        
    }

    public void Exit()
    {

    }

    IEnumerator EndClimb()
    {
        _animator.SetInteger("Status", 10);
        player.rb.linearVelocity = new Vector2(0, 4);
        yield return new WaitForSeconds(1f);
        
        player.ChangeState(player._playerIdle);
        player.rb.linearVelocity = Vector2.zero;
        player.rb.bodyType = RigidbodyType2D.Dynamic;
        player.isClimbEnd = false;
    }
}
