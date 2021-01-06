using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float clipNormalizedTime;
    private AnimatorClipInfo[] currentClipInfo;
    private Rigidbody2D body;
    private Animator animator;
    private EnemyBehavior enemyBehavior;
    private EnemyShooting enemyShooting;
    [SerializeField]
    private bool shooting;
    private bool alive;

    [SerializeField]
    private int maxHealth;
    private int currentHealth;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        alive = true;
        if (!shooting)
            enemyBehavior = GetComponent<EnemyBehavior>();
        else 
            enemyShooting = GetComponent<EnemyShooting>();
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (currentClipInfo[0].clip.name.Equals("Death"))
        {
            clipNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        
        HealthCheck();
    }
    public void Damage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            //healthBar.SetHealth(currentHealth);
            animator.SetTrigger("Damage");
            Debug.Log("Vita del nemico: " + currentHealth);
            StartCoroutine(stopMovement());
        }
    }

    private IEnumerator stopMovement()
    {
        if (!shooting)
            enemyBehavior.movementSpeed = 0f;
        else
            enemyShooting.movementSpeed = 0f;
        yield return new WaitForSecondsRealtime(.4f);
        if (!shooting)
            enemyBehavior.movementSpeed = .5f;
        else
            enemyShooting.movementSpeed = .5f;
    }

    private void HealthCheck()
    {
        if ((currentHealth <= 0) && (alive))
        {
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        alive = false;
        yield return new WaitForSecondsRealtime(.4f);
        animator.SetTrigger("Death");
        if (!shooting)
            enemyBehavior.movementSpeed = 0f;
        else
            enemyShooting.movementSpeed = 0f;
        body.velocity = new Vector2(0, 0);
        body.bodyType = RigidbodyType2D.Kinematic;
        clipNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        yield return new WaitUntil(() => (currentClipInfo[0].clip.name.Equals("Death")) && (clipNormalizedTime > 1));

        gameObject.SetActive(false);
    }

}