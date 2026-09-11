using UnityEngine;
using System;

public class PlayerShoot : IPlayerState
{
    private PlayerController player;
    private Animator _animator;
    private GameObject bulletPrefab;
    private float timer;
    private float duration = 0.25f;

    //constructure
    public PlayerShoot(PlayerController player)
    {
        this.player = player;
        this.bulletPrefab = bulletPrefab;
        _animator = player.GetComponent<Animator>();
    }

    public void Enter()
    {
        _animator.SetInteger("Status", 5);
        player.ShootBullet();
        timer = 0;
    }

    public void Update()
    {
        Debug.Log("chay " + this);
        if (Input.GetKeyDown(KeyCode.W))
        {
            player.ChangeState(player.playerJump);
        }
        
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            //thoat khoi shoot
            if (player.moveX == 0)
            {
                player.ChangeState(player._playerIdle);
            }
            else
            {
                player.ChangeState(player.playerRun);
            }
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                player.ChangeState(player.playerJumpShoot);
            }
        }

        
    }

    public void Exit()
    {

    }
}
