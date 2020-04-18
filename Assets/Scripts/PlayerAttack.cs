using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;
    AnimatorClipInfo[] currentClipInfo;
    LayerMask enemyLayer;
    [SerializeField]
    private Transform attackPoint = null;

    private float clipNormalizedTime;
    private float damage = 1;
    private bool Attacked;
    private float AttackRadius = .3f;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyLayer = LayerMask.GetMask("Enemy");
        Attacked = false;
    }

    void Update()
    {
        AttackAnimation();
    }

    private void AttackAnimation()
    {
        currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);

        if ((currentClipInfo[0].clip.name.Equals("Attack_Player")) || (currentClipInfo[0].clip.name.Equals("Attack_Walk")) || (currentClipInfo[0].clip.name.Equals("Attack_Run")) || (currentClipInfo[0].clip.name.Equals("Attack_Extra")))
        {
            clipNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            if ((currentClipInfo[0].clip.name.Equals("Idle")) || (currentClipInfo[0].clip.name.Equals("Still")))
            {
                animator.SetBool("Attack_Player", true);
            }
            else if (currentClipInfo[0].clip.name.Equals("Walk"))
            {
                animator.SetBool("Attack_Walk", true);
            }
            else if (currentClipInfo[0].clip.name.Equals("Run"))
            {
                animator.SetBool("Attack_Run", true);
            }
            else if (((currentClipInfo[0].clip.name.Equals("Attack_Player")) || (currentClipInfo[0].clip.name.Equals("Attack_Walk")) || (currentClipInfo[0].clip.name.Equals("Attack_Run"))) && (clipNormalizedTime > 0.85))
            {
                animator.SetBool("Attack_Extra", true);
            }
        }
        if ((clipNormalizedTime >= .3f) && (clipNormalizedTime < 1f))
            Attack();
        else if (clipNormalizedTime >= 1f)
        {
            animator.SetBool("Attack_Player", false);
            animator.SetBool("Attack_Walk", false);
            animator.SetBool("Attack_Run", false);
            animator.SetBool("Attack_Extra", false);
            clipNormalizedTime = 0f;
            if (Attacked)
                Attacked = !Attacked;
        }
    }

    private void Attack()
    {
        Collider2D[] enemyHit = Physics2D.OverlapCircleAll(attackPoint.position, AttackRadius, enemyLayer);

        foreach(Collider2D enemy in enemyHit)
        {
            if (!Attacked)
            {
                enemy.GetComponent<EnemyHealth>().Damage(damage);
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