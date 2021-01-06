using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;
    LayerMask playerLayer;
    
    [SerializeField]
    private Transform attackPoint = null;
    Vector2 spawnPosition;
    Vector2 actualPosition;
    Vector2 playerPosition;

    private bool FacingRight = true;                                            ////////////////////
    public float movementSpeed = 0.5f;                                         //
    private float movementTime = 2f;                                            //
    private float restTime = 3f;                                                //
    private float dangerDistance = 2.5f;                                        //
    [SerializeField]                                                            //
    private float attackDistance = 0;                                           //  Movement and path variables
    private float spawnDistance = 3f;                                           //
    private float timer;                                                        //
    private float h = 0;                                                        //
    private bool left = true;                                                   //
    private int n = 5;                                                          ////////////////////

    private int damage = 1;                                                   //
    private bool Attacking;                                                     //
    private bool Attacked;                                                      //
    AnimatorClipInfo[] currentClipInfo;                                         //
    private float clipNormalizedTime;                                           //
    private float timerAttack;
    [SerializeField]
    private float AttackRadius = .6f;

    void Start()                                                                // Start is called before the first frame update
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerLayer = LayerMask.GetMask("Player");
        timer = restTime;
        timerAttack = .2f;
        StartCoroutine(DoCheck());
        spawnPosition = transform.position;
        Attacking = false;
        Attacked = false;
    }
    void Update()                                                               // Update is called once per frame
    {
        Path();
        if (!Attacking)
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
        else if (((DistanceCheck() > attackDistance) && (DistanceCheck() <= dangerDistance) && (DistanceCheck(actualPosition, spawnPosition) < spawnDistance)) && (!Attacking))
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
            Attacking = true;
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
            timerAttack = .2f;
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

        if (Attacking)
        {
            if (((currentClipInfo[0].clip.name.Equals("Idle")) || (currentClipInfo[0].clip.name.Equals("Walk"))) && (timerAttack <= 0))
                animator.SetBool("Attack", true);
            timerAttack -= Time.deltaTime;
        }

        if ((clipNormalizedTime >= .5f) && (clipNormalizedTime < 1f))
            Attack();
        else if ((clipNormalizedTime >= 1f) || (currentClipInfo[0].clip.name.Equals("Damage")))
        {
            if ((playerPosition.x - actualPosition.x) > 0)
                Flip();
            timerAttack = .5f;
            animator.SetBool("Attack", false);
            clipNormalizedTime = 0f;
            if (Attacked)
                Attacked = !Attacked;
            Attacking = false;
        }
    }
    private void Attack()
    {
        Collider2D[] playerHit = Physics2D.OverlapCircleAll(attackPoint.position, AttackRadius, playerLayer);

        foreach (Collider2D player in playerHit)
        {
            if (!Attacked)
            {
                player.GetComponent<PlayerHealth>().Damage(damage);
                Attacked = true;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, AttackRadius);
    }
}