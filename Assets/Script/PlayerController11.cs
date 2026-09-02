using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController11 : MonoBehaviour
{
    public CharacterData characterData;
    private float moveX, moveY;


    public float speed = 10f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isRight = true;
    private Animator anim;
    private bool isClimb = false;
    private bool dead = false;
    private bool changeCamera = false;
    private NuclearMonkeyController nuclearMonkeyController;
    
    public float swimSpeed = 4f;
    public GameObject gameOver;
    public TypewriterText typeWriterText;
    private Camera camera;
    public Transform cameraPos;

    public GameObject foot;
   
    private enum PlayerState
    {
        Idle = 0,
        Jump = 2,
        Hurt = 3,
        Climb_Idle = 4,
        Shoot = 5,
        Run = 1,
        Die = 6,
        Crouch = 7,
        Swim = 8, // ko co animation
        Climb_Move = 9,
        Climb_End_Point = 10,
        Climb_Shoot= 11 //default = left
    }

    private PlayerState currentState = PlayerState.Idle;

    private bool isGround = true;
    private Hurt h;
    public Action<float> onHpChange;
    
    public Action onMeetNuclearMoney;
    public bool nuclearMonkeyAppear = false;

    public float jumpForce = 7f;
    private bool isHurt = false;
    public GameObject up;
    public GameObject down;

    private Transform firePoint; //quyet dinh vi tri cua firePoint
    public GameObject bulletPrefab;
    public bool playerDie = false;
    public bool isShooting = false;



    //khai bao su kien thong bao hp thay doi
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        h = GetComponent<Hurt>();
        //sr.sprite = characterData.player;
        //anim.runtimeAnimatorController = characterData.animatorController;
        //GameManager.Instance.heath = characterData.hp;
        if (typeWriterText != null)
            typeWriterText.gameObject.SetActive(false);
        camera = GetComponentInChildren<Camera>();
        nuclearMonkeyController = FindAnyObjectByType<NuclearMonkeyController>(FindObjectsInactive.Include);
    }
    

    public void isAttack(float damage)
    {
        //tru diem trong GameManager
        Debug.Log("bi tru: " + damage);
        GameManager.Instance.hp -= damage;
        Debug.Log("con lai: " + GameManager.Instance.hp);

        //hien tri tren UI
        onHpChange?.Invoke(damage);
        //action hurt
        h.getHurt();
        //neu cap nhat currentState ngay bay gio thi currenState sau do se ngay lap tuc doi sang
        //Idle (do anh huong cua nhung code trong update)
        //--> ko hien thi animation hurt
        //isHurt = false;
        //them coroutine vao de kip thay trnag thai Hurt

        //SoundManager.Instance.currentSound = SoundManager.SoundID.Hurt;
        SoundManager.Instance.currentSound = SoundManager.SoundID.PlayerHurt;
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("isGround"))
        {
            //dang cham dat
            isGround = true;
            Instantiate(foot, down.transform.position, Quaternion.identity);
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
            Debug.Log("Player roi dat");
            isGround = false;
        }
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            isHurt = false;
        } 
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MeetBoss"))
        {
            onMeetNuclearMoney?.Invoke();
        }
        
        if (other.gameObject.CompareTag("ChangeCamera"))
        {
            changeCamera = true;
        }

        if (other.gameObject.CompareTag("ClimbEndPoint"))
        {
            stairCase(false);
        }

        if (other.gameObject.CompareTag("isGround"))
        {
            isGround = true;
            Instantiate(foot, down.transform.position, Quaternion.identity);

        }
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("currentState = " + currentState);
        Debug.Log("isGround = " + isGround);
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isShooting = true;
            Debug.Log("la shoot!!!!!");
            currentState = PlayerState.Shoot;
            Shoot();
            StartCoroutine(ShootCoroutine());
            //Shoot();
            SoundManager.Instance.currentSound = SoundManager.SoundID.Shoot;
        }

        if (isShooting == false)
        {
            Debug.Log("khac shoot!!!!!");
            if (moveX == 1)
            {
                //neu di sang trai
                isRight = true;
                sr.flipX = true;
                currentState = PlayerState.Run;
                Debug.Log("StartPlayerRun");
                SoundManager.Instance.StartPlayerRun();
            }
            else if (moveX == -1)
            {
                //neu di sang trai
                isRight = false;
                sr.flipX = false;
                currentState = PlayerState.Run;
                Debug.Log("StartPlayerRun");
                SoundManager.Instance.StartPlayerRun();
            }
            else
            {
                Debug.Log("StopPlayerRun");
                SoundManager.Instance.StopPlayerRun();
            }

            if (moveX == 0 && moveY == 0 && isGround == true) //neu dung yen tren mat dat
            {
                Debug.Log("thay doi sang idle");
                currentState = PlayerState.Idle;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                //vat nhay len
                if (isGround == true)
                {
                    Instantiate(foot, down.transform.position, Quaternion.identity);
                    currentState = PlayerState.Jump;
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    SoundManager.Instance.currentSound = SoundManager.SoundID.Jump;
                }
            }



            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("nhan S");
                //cui nguoi xuong
                currentState = PlayerState.Crouch;
                StartCoroutine(wait());
                //y: 2.84, x: 3.8
            }
        }
        
        updatePlayerState(currentState);

        
        if (changeCamera == true)
        {
            float speedCamera = 5f;
            camera.transform.position = Vector3.MoveTowards(
                camera.transform.position,
                cameraPos.position,
                speedCamera * Time.deltaTime);

            if (camera.transform.position == cameraPos.position)
            {
                changeCamera = false;
            }
            
            if (Vector3.Distance(cameraPos.position, camera.transform.position) <= 1f)
            {
                camera.transform.SetParent(null);
                //camera.transform.SetParent(null);
                if (nuclearMonkeyAppear == false)
                {
                    nuclearMonkeyController.Appear();
                    nuclearMonkeyAppear = true;
                }
            }
        }
        
    }

    private void FixedUpdate()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }

    public void stairCase(bool isClimb)
    {
        this.isClimb = isClimb;
        if (isClimb == false)
        {
            StartCoroutine(ClimbEndPoint());
            Debug.Log("roi khoi cau thang");
            Debug.Log("trang thai sau khi roi cau thang: " + currentState);
        }
    }

    void Shoot()
    {
        if (isRight == false)
        {
            //neu o ben trai
            firePoint = gameObject.transform.GetChild(0).transform;
        }
        else if (isRight == true)
        {
            //neu o ben phai
            firePoint = gameObject.transform.GetChild(1).transform;
        }
        //lay duoc vi tri roi thi cho xuat hien hinh anh
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletController bc = newBullet.gameObject.GetComponent<BulletController>();
        Debug.Log("player da ban dan");
        //tao xong roi moi thay doi direction
        if (isRight == true)
            bc.setDirection(true);
        else if (isRight == false)
            bc.setDirection(false);
        
    }

    public void isDead()
    {
        dead = true;
    }
    

    void updatePlayerState(PlayerState playerState)
    {
         /*
         * status = 0: idle
         * status = 1: run
         * status = 2: jump
         * status = 3: hurt
         * status = 4: climb_idle
         * status = 5: shoot
         * status = 6: die
         * status = 7: Crouch 
        * status = 8: swim
        * status = 9: climb_move
        * status = 10: climb end point
        * status = 11: player_run
        */
         
         if (dead == true)
         {
             anim.SetInteger("Status", (int)PlayerState.Die);
             currentState = PlayerState.Die;
             if (playerDie == false)
             {
                 Debug.Log("phat nhac player da chet");
                 //SoundManager.Instance.currentSound = SoundManager.SoundID.GameOver;
                 //GameManager.Instance.currentScene = SceneManager.GetActiveScene().name;
                 //SceneManager.LoadScene("Replay");
                 StartCoroutine(GameOver());
                 playerDie = true;
             }
           
         }

        if (isHurt == true) //tach rieng ra switch chu ko de chung
        {
            //tach rieng re animation ko chay tu cai nay sang cai kia
            anim.SetInteger("Status", (int)PlayerState.Hurt);
            currentState = PlayerState.Hurt;
            StartCoroutine(wait());
            //them cai nay vao de ko chay xuong switch o duoi
            //neu no di xuong switch no no se cap nhat lai animation
        }

        if (isClimb)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            //anim.SetInteger("Status", (int)PlayerState.Climb);
            Debug.Log("isClimb: " + isClimb);
            currentState = PlayerState.Climb_Idle;
            //them cai nay vao de ko chay xuong switch o duoi
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveY * speed);
            if (moveY == 0)
                anim.SetInteger("Status", (int)PlayerState.Climb_Idle);
            else if (moveY == 1 || moveY == -1)
                anim.SetInteger("Status", (int)PlayerState.Climb_Move);
        }

        
        
        switch (currentState)
        {
            
            case PlayerState.Idle:
                anim.SetInteger("Status", (int)currentState);
                break;
            
            case PlayerState.Jump:
                anim.SetInteger("Status", (int)currentState);
                break;
            
            
            case PlayerState.Run:
                anim.SetInteger("Status", (int)currentState);
                break;
            
            case PlayerState.Crouch:
                anim.SetInteger("Status", (int)currentState);
                StartCoroutine((wait()));
                break;
            
            case PlayerState.Swim:
                anim.SetInteger("Status", (int)PlayerState.Jump); //xem jump giong nhu dang swim
                //di chuyen player len xuong nhu dang boi
                swim();
                break;
            
            case PlayerState.Hurt:
                anim.SetInteger("Status", (int)currentState);
                StartCoroutine(wait());
                break;
            
            case PlayerState.Climb_End_Point:
                anim.SetInteger("Status", (int)currentState);
                StartCoroutine(wait());
                break;
        }
    }

    void swim()
    {
        GameObject[] swimPosition = { up, down};
        int targetSwimPosition = 0;
        gameObject.transform.position = Vector2.MoveTowards(transform.position, 
            swimPosition[targetSwimPosition].transform.position,
            swimSpeed * Time.deltaTime);
        targetSwimPosition++;
        if (targetSwimPosition == swimPosition.Length)
            targetSwimPosition = 0;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("cho 3f");

        currentState = PlayerState.Idle;
        //anim.SetInteger("Status", (int)PlayerState.Idle);
    }

    IEnumerator ShootCoroutine()
    {
        anim.SetInteger("Status", (int)PlayerState.Shoot);
        
        yield return new WaitForSeconds(0.1f);
        
        isShooting = false;

    }

    IEnumerator GameOver()
    {
        SoundManager.Instance.currentSound = SoundManager.SoundID.GameOver;
        yield return new WaitForSeconds(1f);
        GameManager.Instance.currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Replay");
    }

    IEnumerator ClimbEndPoint()
    {
        currentState = PlayerState.Climb_End_Point;
        Debug.Log("currentState = " + currentState);
        
        yield return new WaitForSeconds(0.5f);
        currentState = PlayerState.Idle;
        Debug.Log("currentState = " + currentState);
        
        rb.bodyType = RigidbodyType2D.Dynamic;

    }

    public void Carrying()
    {
        currentState = PlayerState.Idle;
    }
}
