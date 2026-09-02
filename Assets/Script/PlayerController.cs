using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 private enum PlayerState
    {
        Idle = 0, +/////
        Jump = 2, +/////
        Hurt = 3, +/////
        Climb_Idle = 4, 
        Shoot = 5, +////
        Run = 1, +/////
        Die = 6,
        Crouch = 7,
        Climb_Move = 9,//////
        Climb_End_Point = 10, /////
        Climb_Shoot= 11 //////default = left
        JumpShoot = 12 +//////
    }
 */


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveX, moveY;
    public float speed = 10f;
    public float swimSpeed = 4f;
    public float jumpForce = 4f;
    public float climSpeed = 10f;

    public Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private bool isRight = true;
    
    [Header("Combat")]
    private Transform firePoint; //quyet dinh vi tri cua firePoint
    public GameObject bulletPrefab;
    public bool isShooting = false;

    [Header("PlayerStatus")]
    private IPlayerState currentState;
    
    public bool isClimb = false;
    private bool dead = false;
    public Hurt h;
    private bool isHurt = false;
    public Action<float> onHpChange;
    public bool isGround = true;
    public bool isMoving = false;
    public bool isClimbEnd = false;


    [Header("Effect")]
    public GameObject foot;
    public GameObject up;
    public GameObject down;
    
    
    [Header("Boss")]
    private NuclearMonkeyController nuclearMonkeyController;


    [Header("Camera")]
    public Transform cameraPos;
    private Camera camera;
    private bool changeCamera = false;

    [Header("Dialogue")]
    public TypewriterText typeWriterText;

    [Header("IPlayerState")] 
    public PlayerIdle _playerIdle;
    public PlayerRun playerRun;
    public PlayerShoot playerShoot;
    public PlayerJump playerJump;
    public PlayerJumpShoot playerJumpShoot;
    public PlayerClimb playerClimb;
    public PlayerHurt playerHurt;
    public ClimbEndPoint climbEndPoint;
    public PlayerClimbShoot playerClimbShoot;



    private void Start()
    {
        _playerIdle = new PlayerIdle(this);
        playerRun = new PlayerRun(this);
        playerShoot = new PlayerShoot(this);
        playerJump = new PlayerJump(this);
        playerJumpShoot = new PlayerJumpShoot(this);
        playerClimb = new PlayerClimb(this);
        playerHurt = new PlayerHurt(this);
        climbEndPoint = new ClimbEndPoint(this);
        playerClimbShoot = new PlayerClimbShoot(this);

        currentState = _playerIdle;
        
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Movement();
        Direction();

        Debug.Log("isClimb: " + isClimb);
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround == true)
            {
                ChangeState(playerShoot);
            }
            else
            {
                if (isClimb == false)
                    ChangeState(playerJumpShoot);
                else
                {
                    ChangeState(playerClimbShoot);
                }
            }
        }
        
        if (moveX == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isClimb == true)
            {
                ChangeState(playerClimb);
            }
            
            if (isClimb == false && isGround == true)
            {
                isGround = false;
                Jump();
                ChangeState(playerJump);
            }
        }
        
        Debug.Log("currentState: " + currentState);
        currentState.Update();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("isGround"))
        {
            isGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BulletEnemy"))
        {
            TakeDamage();
        }

        if (other.gameObject.CompareTag("ClimbEndPoint"))
        {
            Debug.Log("Dung vao ClimbEndPoint!!!!!!!!!!!!!!!!!!! ");
            isClimbEnd = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("isGround"))
        {
            isGround = false;
        }  
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        } 
    }

    void Movement()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
    }

    void Direction()
    {
        if (moveX == -1)
        {
            //di sang trai
            isRight = false;
            sr.flipX = false;
        }
        
        if (moveX == 1)
        {
            //di sang phai
            isRight = true;
            sr.flipX = true;
        }
    }

    public void ChangeState(IPlayerState currentState)
    {
        this.currentState = currentState;
        currentState.Enter();
    }

    public void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }

    public void ShootBullet()
    {
        
            if (isRight == true)
            {
                firePoint = transform.GetChild(1);
            }
            else
            {
                firePoint = transform.GetChild(0);
            }
        

        //lay cai bullet ra roi moi doi huong
        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, 
            Quaternion.identity);
        
        BulletController bulletController = bullet.GetComponent<BulletController>();

        bulletController.setDirection(isRight);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public void Climb()
    {
        rb.linearVelocity = new Vector2(0, moveY * climSpeed);
        rb.bodyType = RigidbodyType2D.Kinematic;
        if (moveY != 0)
        {
            StartCoroutine(ClimbCoroutine());
        }
    }

    IEnumerator ClimbCoroutine()
    {
        anim.SetInteger("Status", 9);
        yield return new WaitForSeconds(0.25f);
        anim.SetInteger("Status", 4);
    }

    void TakeDamage()
    {
        ChangeState(playerHurt);
        onHpChange?.Invoke(-1);
    }

    public void ExitClimb()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        ChangeState(playerJump);
    }
}
