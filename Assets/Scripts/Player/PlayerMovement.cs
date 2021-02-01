using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    [SerializeField]
    private AudioManager audioManager;
    private Vector2 velocity;                                                           
    private bool FacingRight = true;
    [SerializeField]
    internal Vector2 moveSpeed;
    //public float moveCoin = 1f;                                                 // If Coin velocity, moveCoin = 2;
    [SerializeField]
    private float timer = 10f;
    private float h;
    private float j;
    private float distance;
    [SerializeField]
    private LayerMask stairsLayer;
    private bool isClimbing;
    private void Start()                                                                // Start is called before the first frame update
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        moveSpeed = new Vector2(1.8f, 1.2f);
        audioManager = AudioManager.instance;
        StartCoroutine(Footstep());
        if (audioManager == null)
        {
            Debug.LogError("AudioManager non trovato");
        }
    }
    private void Update()                                                               // Update is called once per frame
    {
        Movement();
        animationMovement();
    }
    private void Movement()
    {
        h = Input.GetAxisRaw("Horizontal");

        /*if (moveMirror)
        {
            velocity = new Vector2(Vector2.right.x * moveSpeed * -h, body.velocity.y);
        }
        else
        {
        }*/

        body.velocity = new Vector2(Vector2.right.x * moveSpeed.x * h * Time.timeScale, body.velocity.y);
        
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, stairsLayer);
        
        if (hitInfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                isClimbing = true;
        }
        else 
        {
            if ((Input.GetKeyDown(KeyCode.LeftArrow)) || (Input.GetKeyDown(KeyCode.RightArrow)))
                isClimbing = false;
        }

        if ((isClimbing) && (hitInfo.collider != null))
        {
            j = Input.GetAxisRaw("Vertical");
            body.velocity = new Vector2(body.velocity.x, Vector2.up.y * moveSpeed.y * j * Time.timeScale);
            body.gravityScale = 0;
        }
        else
        {
            body.gravityScale = 1;
        }
    }
    private void animationMovement()
    {
        if ((body.velocity.x < 0) && FacingRight)
        {
            Flip();
        }
        else if ((body.velocity.x > 0) && !FacingRight)
        {
            Flip();
        }
        else if ((body.velocity.x != 0) && (body.velocity.y == 0))
        {
            animator.SetBool("IsWalking", true);
            timer = 10f;
            animator.SetBool("IsStill", false);
        }
        else if (body.velocity.y != 0)
        {
            animator.SetBool("IsWalking", false);
        }
        else if ((body.velocity.x == 0) && (body.velocity.y == 0) && (timer > 0))
        {
            timer -= Time.deltaTime;
            animator.SetBool("IsWalking", false);
        }
        if ((body.velocity.y != 0) || (animator.GetBool("Attack_Player")) || (animator.GetBool("Damage")))
        {
            timer = 10f;
            animator.SetBool("IsStill", false);
        }

        if (timer <= 0)                                                         // If the player is still more than 10 sec, animation Still
        {
            animator.SetBool("IsStill", true);
        }

        /*if (moveCoin > 1f)                                                      // If the player has take Coin velocity, animation Run
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }*/
    }
    private void Flip()
    {
        FacingRight = !FacingRight;
        body.transform.Rotate(0f, 180f, 0f);
    }
    private IEnumerator Footstep()
    {
        yield return new WaitForSeconds(.5f);
        for( ; ; )
        {
            if ((body.velocity.x != 0) && (body.velocity.y == 0))
            {
                audioManager.PlaySound("Footstep");
                yield return new WaitForSeconds(.563f);
            }
            else if ((body.velocity.x == 0) || (body.velocity.y !=0))
            {
                audioManager.StopSound("Footstep");
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}