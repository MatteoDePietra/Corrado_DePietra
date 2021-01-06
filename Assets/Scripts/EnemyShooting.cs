using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;
    LayerMask playerLayer;
    
    [SerializeField]
    private Transform firePoint = null;
    [SerializeField]
    private GameObject bullet = null;
    Vector2 spawnPosition;
    Vector2 actualPosition;
    Vector2 playerPosition;

    private bool FacingRight = true;                                            ////////////////////
    public float movementSpeed = 0.5f;                                         //
    private float movementTime = 2f;                                            //
    private float restTime = 3f;                                                //
    private float dangerDistance = 2.5f;                                        //
    [SerializeField]                                                            //
    private float attackDistance = 1f;                                          //  Movement and path variables
    private float spawnDistance = 3f;                                           //
    private float timer;                                                        //
    private float h = 0;                                                        //
    private bool left = true;                                                   //
    private int n = 5;                                                          ////////////////////

    private bool attacking;                                                     //
    private bool shooted;                                                      //
    AnimatorClipInfo[] currentClipInfo;                                         //
    private float clipNormalizedTime;                                           //
    private float timerAttack;

    void Start()                                                                // Start is called before the first frame update
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerLayer = LayerMask.GetMask("Player");
        timer = restTime;
        timerAttack = .5f;
        StartCoroutine(DoCheck());
        spawnPosition = transform.position;
        attacking = false;
        shooted = false;
    }
    void Update()                                                               // Update is called once per frame
    {
        Path();
        if (!attacking)
            body.velocity = new Vector2(Vector2.right.x * h * movementSpeed, body.velocity.y);
        else 
            body.velocity = new Vector2(Vector2.right.x * 0 * movementSpeed, body.velocity.y);
        MovementAnimation();
        AttackAnimation();
    }
    private IEnumerator DoCheck()                                               // Check all distance 
    {
        for (; ; )
        {
            actualPosition = transform.position;
            if (GameObject.FindGameObjectWithTag("Player") != null)
                playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            DistanceCheck();
            DistanceCheck(actualPosition, spawnPosition);
            yield return new WaitForSeconds(.3f);
        }
    }
    private float DistanceCheck()                                               // Check distance between player and enemy
    {
        return Mathf.Abs(actualPosition.x - playerPosition.x);
    }
    private float DistanceCheck(Vector2 Position1, Vector2 Position2)           // Check distance between 2 position
    {
        return Mathf.Abs(Position1.x - Position2.x);
    }
    private void Path()                                                         //  Path design of walking enemy
    {
        if ((DistanceCheck(actualPosition, spawnPosition) >= spawnDistance) && (DistanceCheck(playerPosition, spawnPosition) >= spawnDistance))
        {
            h = 0;
        }
        else if (DistanceCheck(actualPosition, spawnPosition) >= spawnDistance)
        {
            if ((spawnPosition.x - actualPosition.x) > 0)
            {
                h = 1;
                n = 1;
                left = false;
                timer = restTime;
            }
            else
            {
                h = -1;
                n = 1;
                left = true;
                timer = restTime;
            }
        }
        else if ((DistanceCheck() > dangerDistance) && (DistanceCheck(actualPosition, spawnPosition) < spawnDistance))
        {
            if ((timer < 0) && (n < 10))
            {
                n++;
                if (n == 10)
                {
                    n = 2;
                    left = !left;
                }
                if (n % 2 == 0)
                {
                    if (left == true)
                        h = -1;
                    else h = 1;
                    timer = movementTime;
                }
                else
                {
                    h = 0;
                    timer = restTime;
                }
            }
            else timer -= Time.deltaTime;
        }
        else if (((DistanceCheck() > attackDistance) && (DistanceCheck() <= dangerDistance) && (DistanceCheck(actualPosition, spawnPosition) < spawnDistance)) && (!attacking))
        {
            if ((playerPosition.x - actualPosition.x) > 0)
                h = 2;
            else h = -2;
        }
        else if ((DistanceCheck() <= attackDistance) && (DistanceCheck(actualPosition, spawnPosition) < spawnDistance))
        {
            if ((playerPosition.x - actualPosition.x) > 0)
                h = 2;
            else h = -2;
            attacking = true;
        }
    }
    private void MovementAnimation ()                                           // Start animation
    {
        if ((h < 0) && FacingRight)
        {
            Flip();
        }
        else if ((h > 0) && !FacingRight)
        {
            Flip();
        }

        if (this.body.velocity.x != 0)
        {
            timerAttack = .5f;
            animator.SetBool("Walk", true);
        }
        else animator.SetBool("Walk", false);
    }
    private void Flip()
    {
        FacingRight = !FacingRight;
        body.transform.Rotate(0f, 180f, 0f);
    }
    private void AttackAnimation()
    {
        currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);

        if (currentClipInfo[0].clip.name.Equals("Attack"))
        {
            clipNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        if (attacking)
        {
            if (((currentClipInfo[0].clip.name.Equals("Idle")) || (currentClipInfo[0].clip.name.Equals("Walk"))) && (timerAttack <= 0))
                animator.SetBool("Attack", true);
            timerAttack -= Time.deltaTime;
        }

        if ((clipNormalizedTime >= .5f) && (clipNormalizedTime < 1f) && (!shooted))
        {
            Shoot(shooted);
            shooted = true;
        }
        else if ((clipNormalizedTime >= 1f) || (currentClipInfo[0].clip.name.Equals("Damage")))
        {
            timerAttack = .5f;
            animator.SetBool("Attack", false);
            clipNormalizedTime = 0f;
            if (shooted)
                shooted = !shooted;
            if (attacking)
                attacking = !attacking;
        }
    }
    private void Shoot(bool shooted)
    {
        if (!shooted)
            Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}