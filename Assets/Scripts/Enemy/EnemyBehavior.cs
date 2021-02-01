using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    private LayerMask playerLayer;
    private Vector2 spawnPosition;
    internal Vector2 actualPosition;
    internal Vector2 playerPosition;
    private bool facingRight = true;
    [SerializeField]
    internal float movementSpeed = 0.5f;
    private float movementTime = 2f;
    private float restTime = 3f;
    [SerializeField]
    internal float dangerDistance = 2.5f;
    [SerializeField]
    internal float attackDistance = 0.1f;
    private float spawnDistance = 3f;
    private float timerPath;
    private float h = 0;
    private bool left = true;
    private int n = 5;
    internal bool attacking;
    internal float timerAttack;
    void Start()                                                                // Start is called before the first frame update
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerLayer = LayerMask.GetMask("Player");
        timerPath = restTime;
        timerAttack = .2f;
        StartCoroutine(DoCheck());
        spawnPosition = transform.position;
        attacking = false;
    }
    void Update()                                                               // Update is called once per frame
    {
        Path();
        if (!attacking)
            body.velocity = new Vector2(Vector2.right.x * h * movementSpeed, body.velocity.y);
        else 
            body.velocity = new Vector2(Vector2.right.x * 0 * movementSpeed, body.velocity.y);
        MovementAnimation();
    }
    private IEnumerator DoCheck()                                               // Check all distance every .3 sec
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
    private void Path()                                                         //  Path design of walking enemies
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
                timerPath = restTime;
            }
            else
            {
                h = -1;
                n = 1;
                left = true;
                timerPath = restTime;
            }
        }
        else if ((DistanceCheck() > dangerDistance) && (DistanceCheck(actualPosition, spawnPosition) < spawnDistance))
        {
            if ((timerPath < 0) && (n < 10))
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
                    timerPath = movementTime;
                }
                else
                {
                    h = 0;
                    timerPath = restTime;
                }
            }
            else timerPath -= Time.deltaTime;
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
        if ((h < 0) && facingRight)
        {
            Flip();
        }
        else if ((h > 0) && !facingRight)
        {
            Flip();
        }

        if (body.velocity.x != 0)
        {
            timerAttack = .2f;
            animator.SetBool("Walk", true);
        }
        else animator.SetBool("Walk", false);
    }
    internal void Flip()
    {
        facingRight = !facingRight;
        body.transform.Rotate(0f, 180f, 0f);
    }
}