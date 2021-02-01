using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Animator animator; 
    private LayerMask playerLayer;
    private EnemyBehavior enemyBehavior;
    [SerializeField]
    private Transform attackPoint = null;
    private int damage = 1;
    private AnimatorClipInfo[] currentClipInfo;
    private float clipNormalizedTime;
    [SerializeField]
    private float attackRadius = .6f;
    private bool attacked;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerLayer = LayerMask.GetMask("Player");
        enemyBehavior = GetComponent<EnemyBehavior>();
        attacked = false;
    }
    private void Update()
    {
        AttackAnimation();
    }
    private void AttackAnimation()
    {
        currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);

        if (currentClipInfo[0].clip.name.Equals("Attack"))
        {
            clipNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        if (enemyBehavior.attacking)
        {
            if (((currentClipInfo[0].clip.name.Equals("Idle")) || (currentClipInfo[0].clip.name.Equals("Walk"))) && (enemyBehavior.timerAttack <= 0))
                animator.SetBool("Attack", true);
            enemyBehavior.timerAttack -= Time.deltaTime;
        }

        if ((clipNormalizedTime >= .5f) && (clipNormalizedTime < 1f))
            Attack();
        else if ((clipNormalizedTime >= 1f) || (currentClipInfo[0].clip.name.Equals("Damage")))
        {
            if ((enemyBehavior.playerPosition.x - enemyBehavior.actualPosition.x) > 0)
                enemyBehavior.Flip();
            enemyBehavior.timerAttack = .5f;
            animator.SetBool("Attack", false);
            clipNormalizedTime = 0f;
            if (attacked)
                attacked = !attacked;
            if (enemyBehavior.attacking)
                enemyBehavior.attacking = !enemyBehavior.attacking;
        }
    }
    private void Attack()
    {
        Collider2D[] playerHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);

        foreach (Collider2D player in playerHit)
        {
            if (!attacked)
            {
                player.GetComponent<PlayerHealth>().Damage(damage);
                attacked = true;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}