using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterData characterData;
    
    private float moveX;
    public float speed = 10f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isRight = true;
    private Animator anim;
    private bool isJump = false;
    private float jumpForce = 10f;
    private bool isGround = false;
    private bool isHurt = false;
    private bool isClimb = false;
    private bool isShoot = false;
    private Hurt h;

    private Transform firePoint; //quyet dinh vi tri cua firePoint
    public GameObject bulletPrefab;
    //khai bao su kien thong bao hp thay doi
    public Action<int> onHpChange;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        h = GetComponent<Hurt>();
        sr.sprite = characterData.player;
        anim.runtimeAnimatorController = characterData.animatorController;
        GameManager.Instance.heath = characterData.hp;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("isGround"))
        {
            isGround = true;//dang cham dat
            
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            //neu player dung vao enemy
            //vat bi tru hp
            onHpChange?.Invoke(-1);
            isHurt = true;
            h.getHurt();
        }

        
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Staircase") && Input.GetKey(KeyCode.W))
        {
            Debug.Log("dung cau thang");
            isClimb = true;
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
        if (other.gameObject.CompareTag("Staircase"))
        {
            isClimb = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();

        if (moveX == 1)
        {
            //neu di sang phai
            isRight = true;
            sr.flipX = true;
        }
        else if (moveX == -1)
        {
            //neu di sang trai
            isRight = false;
            sr.flipX = false;
        }
        if (Input.GetKey(KeyCode.W) && isGround == true)
        {
            //vat nhay len
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isShoot = true;
            anim.SetInteger("Status", 5);
            //instantiate
            //khoi tao bullet o update la vi bullet bi destroy
            //the nen minh phai lien tuc tao bullet moi o day
            // chu ko khoi tao no trong start
            if (isRight == false)
            {
                //neu o ben trai
                firePoint = gameObject.transform.GetChild(1).transform;
            }
            else if (isRight == true)
            {
                //neu o ben phai
                firePoint = gameObject.transform.GetChild(2).transform;
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
            isShoot = false;

        }
    }

    private void FixedUpdate()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }

    void UpdateAnimation()
    {
        /*
         * status = 0: idle
         * status = 1: run
         * status = 2: jump
         * status = 3: hurt
         * status = 4: climb
         * status = 5: shoot
         */
        int status = 0;
        /*
        if (Input.GetKey(KeyCode.W) && isGround == true)
            status = 2;
            */
        if (isGround == false)
        {
            status = 2;

        }
        else if (moveX != 0)
        {
            status = 1;
        }
        else if (isHurt == true)
            status = 3;
        else if (isClimb == true)
            status = 4;

        anim.SetInteger("Status", status);
    }
}
