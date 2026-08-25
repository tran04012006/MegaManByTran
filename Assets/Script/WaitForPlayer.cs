using UnityEngine;

public class WaitForPlayer : MonoBehaviour
{
    public float distance = 2;
    private Animator anim;
    public GameObject playerPos;
    private GameObject player;
    private BoxCollider2D _boxCollider2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim.SetInteger("Status", 0);
        _boxCollider2D = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        player.transform.position = player.transform.position;
        if (playerPos == null)
        {
            if (Vector2.Distance(player.transform.position, gameObject.transform.position) <= distance)
            {
                //neu player toi roi
                anim.SetInteger("Status", 1);
                //shoot
            }
            else
            {
                anim.SetInteger("Status", 0);

            }
        }

        else
        {
            //Debug.Log("player.transform.position: " + player.transform.position);
            //Debug.Log("playerPos.transform.position: " + playerPos.transform.position);
            //Debug.Log("distance: " + Vector2.Distance(player.transform.position, playerPos.transform.position));

            //neu player di toi vi tri nay thi moi ban
            if (Vector2.Distance(player.transform.position, playerPos.transform.position) <= 2f)
            {
                //neu player toi roi
                anim.SetInteger("Status", 1);
                //shoot
            }
            else 
            {
                Debug.Log("tro ve idle");
                anim.SetInteger("Status", 0);
                _boxCollider2D.offset = new Vector2(0, 0.1f);
            }
        }
    }

    
}
