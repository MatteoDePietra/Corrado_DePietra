using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    [SerializeField]
    private AudioManager audioManager;
    private bool FacingRight = true;
    [SerializeField]
    internal Vector2 moveSpeed;
    [SerializeField]
    private float timerIdle = 10f;
    [SerializeField]
    private float timerRun = 2.5f;
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
        moveSpeed = new Vector2(1.5f, 1.2f);
        audioManager = AudioManager.instance;
        isClimbing = false;
        StartCoroutine(Footstep());
        if (audioManager == null)
        {
            Debug.LogError("AudioManager non trovato");
        }
    }
    private void Update()                                                               // Update is called once per frame
    {
        Movement();
        AnimationMovement();
    }
    internal void SetTimerRun(float value)
    {
        timerRun = value;
    }
    private void Movement()
    {
        h = Input.GetAxisRaw("Horizontal");

        if (timerRun >= 0)
            body.velocity = new Vector2(Vector2.right.x * moveSpeed.x * h * Time.timeScale, body.velocity.y);
        else
            body.velocity = new Vector2(Vector2.right.x * moveSpeed.x * h * 1.5f * Time.timeScale, body.velocity.y);

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
    private void AnimationMovement()
    {
        if ((body.velocity.x < 0) && FacingRight)
        {
            Flip();
        }
        else if ((body.velocity.x > 0) && !FacingRight)
        {
            Flip();
        }

        if (isClimbing)
        {
            if (body.velocity.y == 0)
            {
                animator.SetBool("IsClimbing", false);
                animator.SetBool("IsClimbing_Idle", true);
            }
            else
            {
                animator.SetBool("IsClimbing", true);
                animator.SetBool("IsClimbing_Idle", false);
            }
        }
        else if (!isClimbing)
        {
            animator.SetBool("IsClimbing", false);
            animator.SetBool("IsClimbing_Idle", false);
        }

        if ((body.velocity.x != 0) && (body.velocity.y == 0) && (timerRun >= 0))
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", false);
            timerRun -= Time.deltaTime;
            timerIdle = 10f;
            animator.SetBool("IsStill", false);
        }
        else if ((body.velocity.x != 0) && (body.velocity.y == 0) && (timerRun < 0))
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", true);
        }
        else if (body.velocity.y != 0)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
        else if ((body.velocity.x == 0) && (body.velocity.y == 0) && (timerIdle > 0))
        {
            timerRun = 2.5f;
            timerIdle -= Time.deltaTime;
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
        if ((body.velocity.y != 0) || (animator.GetBool("Attack_Player")) || (animator.GetBool("Damage")))
        {
            timerIdle = 10f;
            animator.SetBool("IsStill", false);
        }

        if (timerIdle <= 0)                                                         // If the player is still more than 10 sec, animation Still
        {
            animator.SetBool("IsStill", true);
        }
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
                if (timerRun >= 0)
                {
                    audioManager.PlaySound("Footstep", 0.5f);
                    yield return new WaitForSeconds(.2815f);
                }
                else if (timerRun < 0)
                {
                    audioManager.PlaySound("Footstep", 0.75f);
                    yield return new WaitForSeconds(.35f);
                }
                //yield return new WaitForSeconds(.563f);
            }
            else if ((body.velocity.x == 0) || (body.velocity.y !=0))
            {
                audioManager.StopSound("Footstep");
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}