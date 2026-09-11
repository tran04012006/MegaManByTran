using UnityEngine;

public class PlayerDie : IPlayerState
{
    private PlayerController player;
    private Animator _animator;
    private float timer;
    private float duration = 0.5f;

    //constructure
    public PlayerDie(PlayerController player)
    {
        this.player = player;
        _animator = player.GetComponent<Animator>();
     
    }

    public void Enter()
    {
        Debug.Log("chay " + this);
        _animator.SetInteger("Status", 6);
    }

    public void Update()
    {
        
    }

    public void Exit()
    {

    }
}
