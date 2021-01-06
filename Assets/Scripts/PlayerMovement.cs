using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;

    [SerializeField]
    AudioManager audioManager;
    Vector2 velocity;                                                           

    private bool FacingRight = true;
    [SerializeField]
    public float moveSpeed = 1.8f;
    //public float moveCoin = 1f;                                                 // If Coin velocity, moveCoin = 2;
    //public bool moveMirror = false;                                             // If Coin mirror, velocity = -1;
    [SerializeField]
    public float timer = 10f;
    public float h;

    void Start()                                                                // Start is called before the first frame update
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioManager = AudioManager.instance;
        StartCoroutine(Footstep());
        if (audioManager == null)
        {
            Debug.LogError("AudioManager non trovato");
        }
    }
                                                                                
    void Update()                                                               // Update is called once per frame
    {
        Movement();
        animationMovement();
    }

    void Movement()
    {
        h = Input.GetAxisRaw("Horizontal");

        /*if (moveMirror)
        {
            velocity = new Vector2(Vector2.right.x * moveSpeed * -h, body.velocity.y);
        }
        else
        {
        }*/

        velocity = new Vector2(Vector2.right.x * moveSpeed * h * Time.timeScale, body.velocity.y);
        body.velocity = velocity;
    }

    void animationMovement()
    {
        if ((velocity.x < 0) && FacingRight)
        {
            Flip();
        }
        else if ((velocity.x > 0) && !FacingRight)
        {
            Flip();
        }
        else if ((velocity.x != 0) && (velocity.y == 0))
        {
            animator.SetBool("IsWalking", true);
            timer = 10f;
            animator.SetBool("IsStill", false);
        }
        else if (velocity.y != 0)
        {
            animator.SetBool("IsWalking", false);
        }
        else if ((velocity.x == 0) && (velocity.y == 0) && (timer > 0))
        {
            timer -= Time.deltaTime;
            animator.SetBool("IsWalking", false);
        }
        if ((velocity.y != 0) || (animator.GetBool("Attack_Player")) || (animator.GetBool("Damage")))
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
        //float x;
        for( ; ; )
        {
            //x = Input.GetAxisRaw("Horizontal");
            if ((velocity.x != 0) && (velocity.y == 0))
            {
                audioManager.PlaySound("Footstep");
                yield return new WaitForSeconds(.563f);
            }
            else if ((velocity.x == 0) || (velocity.y !=0))
            {
                audioManager.StopSound("Footstep");
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}