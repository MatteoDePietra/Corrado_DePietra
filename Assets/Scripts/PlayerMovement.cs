using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;

    public float moveSpeed = 1.5f;
    public float moveCoin = 1f;                                                 // If Coin velocity, moveCoin = 2;
    public bool moveMirror = false;                                             // If Coin mirror, velocity = -1;
    [SerializeField]
    public float timer = 10f;
    private float h;

    void Start()                                                                // Start is called before the first frame update
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
                                                                                
    void Update()                                                               // Update is called once per frame
    {
        Movement();
        animationMovement();
    }

    void Movement()
    {
        h = Input.GetAxisRaw("Horizontal");                                  
        Vector2 velocity;                                                       // local var
        if (moveMirror)
        {
            velocity = new Vector2(Vector2.right.x * moveSpeed * moveCoin * -h, body.velocity.y);
        }
        else
        {
            velocity = new Vector2(Vector2.right.x * moveSpeed * moveCoin * h, body.velocity.y);
        }
        body.velocity = velocity;
    }

    void animationMovement()
    {
        if (body.velocity.x < 0)
        {
            body.transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("IsWalking", true);
        }
        else if (body.velocity.x > 0)
        {
            body.transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("IsWalking", true);
        }
        else if ((body.velocity.x == 0) && (body.velocity.y == 0) && (timer > 0))
        {
            timer -= Time.deltaTime;
            animator.SetBool("IsWalking", false);
        }

        if ((body.velocity.x != 0) || (body.velocity.y != 0) || (animator.GetBool("Attack_Player")) || (animator.GetBool("Damage")))
        {
            timer = 10f;
            animator.SetBool("IsStill", false);
        }

        if (timer <= 0)                                                         // If the player is still more than 10 sec, animation Still
        {
            animator.SetBool("IsStill", true);
        }

        if (moveCoin > 1f)                                                      // If the player has take Coin velocity, animation Run
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }
}