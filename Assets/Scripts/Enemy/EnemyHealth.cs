using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float clipNormalizedTime;
    private AnimatorClipInfo[] currentClipInfo;
    private Rigidbody2D body;
    private Animator animator;
    private EnemyBehavior enemyBehavior;
    [SerializeField]
    private HealthBar healthBar;
    private bool alive;

    [SerializeField]
    private int maxHealth;
    private int currentHealth;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyBehavior = GetComponent<EnemyBehavior>();

        alive = true;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    private void Update()
    {
        currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (currentClipInfo[0].clip.name.Equals("Death"))
        {
            clipNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        
        HealthCheck();
    }
    internal void Damage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            animator.SetTrigger("Damage");
            StartCoroutine(stopMovement());
        }
    }
    private IEnumerator stopMovement()
    {
            enemyBehavior.h = 0;
        yield return new WaitForSecondsRealtime(.4f);
            enemyBehavior.h = 1;
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
        EnemyCounter.AddEnemy();

        alive = false;
        //yield return new WaitForSecondsRealtime(.4f);
        animator.SetTrigger("Death");
        enemyBehavior.movementSpeed = 0f;
        body.velocity = new Vector2(0, 0);
        body.bodyType = RigidbodyType2D.Kinematic;
        clipNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        yield return new WaitUntil(() => (currentClipInfo[0].clip.name.Equals("Death")) && (clipNormalizedTime > 1));

        gameObject.SetActive(false);
    }
}