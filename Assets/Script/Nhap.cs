using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nhap : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float jumpForce = 7f;
    public float swimSpeed = 4f;

    private float moveX;
    private float moveY;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isRight = true;
    private bool isGround = true;
    private bool isClimb = false;


    [Header("Combat")]
    public GameObject bulletPrefab;

    private Transform firePoint;
    private bool isShooting = false;


    [Header("Player Status")]
    private bool dead = false;
    private bool isHurt = false;

    public bool playerDie = false;

    private Hurt h;

    public Action<float> onHpChange;


    [Header("Effects")]
    public GameObject foot;
    public GameObject up;
    public GameObject down;


    [Header("Boss")]
    public Action onMeetNuclearMoney;

    public bool nuclearMonkeyAppear = false;

    private NuclearMonkeyController nuclearMonkeyController;


    [Header("Camera")]
    public Transform cameraPos;

    private Camera playerCamera;
    private bool changeCamera = false;


    [Header("Dialogue")]
    public TypewriterText typeWriterText;


    // =========================================================
    // STATE
    // =========================================================

    private enum PlayerState
    {
        Idle = 0,
        Run = 1,
        Jump = 2,
        Hurt = 3,
        Climb_Idle = 4,
        Shoot = 5,
        Die = 6,
        Crouch = 7,
        Swim = 8,
        Climb_Move = 9,
        Climb_End_Point = 10,
        Climb_Shoot = 11
    }

    private PlayerState currentState = PlayerState.Idle;

    private Animator anim;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        h = GetComponent<Hurt>();

        if (typeWriterText != null)
        {
            typeWriterText.gameObject.SetActive(false);
        }

        playerCamera = GetComponentInChildren<Camera>();

        nuclearMonkeyController =
            FindAnyObjectByType<NuclearMonkeyController>(
                FindObjectsInactive.Include
            );
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        ReadInput();

        if (dead)
        {
            HandleDeath();
            return;
        }

        if (isHurt)
        {
            ChangeState(PlayerState.Hurt);
            return;
        }

        if (isClimb)
        {
            HandleClimb();
            return;
        }

        HandleDirection();

        HandleShoot();

        if (!isShooting)
        {
            HandleJump();
            HandleCrouch();
            UpdateMovementState();
        }

        HandleCamera();
    }


    // =========================================================
    // PHYSICS
    // =========================================================

    private void FixedUpdate()
    {
        if (dead)
            return;

        if (isClimb)
        {
            rb.linearVelocity =
                new Vector2(
                    rb.linearVelocity.x,
                    moveY * speed
                );

            return;
        }

        rb.linearVelocity =
            new Vector2(
                moveX * speed,
                rb.linearVelocity.y
            );
    }


    // =========================================================
    // INPUT
    // =========================================================

    private void ReadInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
    }


    // =========================================================
    // MOVEMENT
    // =========================================================

    private void HandleDirection()
    {
        if (moveX > 0)
        {
            isRight = true;
            sr.flipX = true;
        }
        else if (moveX < 0)
        {
            isRight = false;
            sr.flipX = false;
        }
    }


    private void HandleJump()
    {
        if (!Input.GetKeyDown(KeyCode.W))
            return;

        if (!isGround)
            return;

        rb.linearVelocity =
            new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

        isGround = false;

        SpawnFootEffect();

        ChangeState(PlayerState.Jump);

        SoundManager.Instance.currentSound =
            SoundManager.SoundID.Jump;
    }


    private void HandleCrouch()
    {
        if (!Input.GetKeyDown(KeyCode.S))
            return;

        if (!isGround)
            return;

        StartCoroutine(CrouchCoroutine());
    }


    private void UpdateMovementState()
    {
        if (!isGround)
        {
            ChangeState(PlayerState.Jump);
            SoundManager.Instance.StopPlayerRun();
            return;
        }

        if (moveX != 0)
        {
            ChangeState(PlayerState.Run);

            SoundManager.Instance.StartPlayerRun();

            return;
        }

        ChangeState(PlayerState.Idle);

        SoundManager.Instance.StopPlayerRun();
    }


    // =========================================================
    // SHOOT
    // =========================================================

    private void HandleShoot()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (isShooting)
            return;

        StartCoroutine(ShootCoroutine());
    }


    private void Shoot()
    {
        if (isRight)
        {
            firePoint = transform.GetChild(1);
        }
        else
        {
            firePoint = transform.GetChild(0);
        }

        GameObject newBullet =
            Instantiate(
                bulletPrefab,
                firePoint.position,
                firePoint.rotation
            );

        BulletController bulletController =
            newBullet.GetComponent<BulletController>();

        bulletController.setDirection(isRight);
    }


    private IEnumerator ShootCoroutine()
    {
        isShooting = true;

        ChangeState(PlayerState.Shoot);

        Shoot();

        SoundManager.Instance.currentSound =
            SoundManager.SoundID.Shoot;

        yield return new WaitForSeconds(0.1f);

        isShooting = false;
    }


    // =========================================================
    // CLIMB
    // =========================================================

    private void HandleClimb()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;

        rb.linearVelocity =
            new Vector2(
                rb.linearVelocity.x,
                moveY * speed
            );

        if (moveY == 0)
        {
            ChangeState(PlayerState.Climb_Idle);
        }
        else
        {
            ChangeState(PlayerState.Climb_Move);
        }
    }


    public void stairCase(bool isClimb)
    {
        this.isClimb = isClimb;

        if (!isClimb)
        {
            StartCoroutine(ClimbEndPointCoroutine());
        }
    }


    private IEnumerator ClimbEndPointCoroutine()
    {
        ChangeState(PlayerState.Climb_End_Point);

        yield return new WaitForSeconds(0.5f);

        rb.bodyType = RigidbodyType2D.Dynamic;

        ChangeState(PlayerState.Idle);
    }


    // =========================================================
    // DAMAGE / DEATH
    // =========================================================

    public void isAttack(float damage)
    {
        GameManager.Instance.hp -= damage;

        onHpChange?.Invoke(damage);

        h.getHurt();

        SoundManager.Instance.currentSound =
            SoundManager.SoundID.PlayerHurt;
    }


    public void isDead()
    {
        dead = true;
    }


    private void HandleDeath()
    {
        ChangeState(PlayerState.Die);

        rb.linearVelocity = Vector2.zero;

        if (playerDie)
            return;

        playerDie = true;

        StartCoroutine(GameOverCoroutine());
    }


    private IEnumerator GameOverCoroutine()
    {
        SoundManager.Instance.currentSound =
            SoundManager.SoundID.GameOver;

        yield return new WaitForSeconds(1f);

        GameManager.Instance.currentScene =
            SceneManager.GetActiveScene().name;

        SceneManager.LoadScene("Replay");
    }


    // =========================================================
    // STATE / ANIMATION
    // =========================================================

    private void ChangeState(PlayerState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        anim.SetInteger(
            "Status",
            (int)currentState
        );
    }


    private IEnumerator CrouchCoroutine()
    {
        ChangeState(PlayerState.Crouch);

        yield return new WaitForSeconds(3f);

        ChangeState(PlayerState.Idle);
    }


    // =========================================================
    // CAMERA / BOSS
    // =========================================================

    private void HandleCamera()
    {
        if (!changeCamera)
            return;

        float cameraSpeed = 5f;

        playerCamera.transform.position =
            Vector3.MoveTowards(
                playerCamera.transform.position,
                cameraPos.position,
                cameraSpeed * Time.deltaTime
            );

        if (Vector3.Distance(
                playerCamera.transform.position,
                cameraPos.position) <= 1f)
        {
            playerCamera.transform.SetParent(null);

            if (!nuclearMonkeyAppear)
            {
                nuclearMonkeyController.Appear();

                nuclearMonkeyAppear = true;
            }
        }

        if (playerCamera.transform.position ==
            cameraPos.position)
        {
            changeCamera = false;
        }
    }


    // =========================================================
    // SWIM
    // =========================================================

    private void Swim()
    {
        ChangeState(PlayerState.Jump);

        transform.position =
            Vector2.MoveTowards(
                transform.position,
                up.transform.position,
                swimSpeed * Time.deltaTime
            );
    }


    // =========================================================
    // COLLISION
    // =========================================================

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("isGround"))
        {
            isGround = true;

            SpawnFootEffect();
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            isHurt = true;
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
            isHurt = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MeetBoss"))
        {
            onMeetNuclearMoney?.Invoke();
        }

        if (other.CompareTag("ChangeCamera"))
        {
            changeCamera = true;
        }

        if (other.CompareTag("ClimbEndPoint"))
        {
            stairCase(false);
        }

        if (other.CompareTag("isGround"))
        {
            isGround = true;

            SpawnFootEffect();
        }
    }


    // =========================================================
    // EFFECT
    // =========================================================

    private void SpawnFootEffect()
    {
        if (foot == null || down == null)
            return;

        Instantiate(
            foot,
            down.transform.position,
            Quaternion.identity
        );
    }


    // =========================================================
    // ANIMATION EVENT
    // =========================================================

    public void Carrying()
    {
        ChangeState(PlayerState.Idle);
    }
}