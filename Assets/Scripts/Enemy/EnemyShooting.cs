using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    private Animator animator;  
    private LayerMask playerLayer;
    private EnemyBehavior enemyBehavior;
    [SerializeField]
    private Transform firePoint = null;
    [SerializeField]
    private GameObject bullet = null;
    private bool shooted;
    private AnimatorClipInfo[] currentClipInfo;
    private float clipNormalizedTime;
    private void Start()
    {
        animator = GetComponent<Animator>();
        playerLayer = LayerMask.GetMask("Player");
        enemyBehavior = GetComponent<EnemyBehavior>();
        shooted = false;
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

        if ((clipNormalizedTime >= .5f) && (clipNormalizedTime < 1f) && (!shooted))
        {
            Shoot(shooted);
            shooted = true;
        }
        else if ((clipNormalizedTime >= 1f) || (currentClipInfo[0].clip.name.Equals("Damage")))
        {
            enemyBehavior.timerAttack = .5f;
            animator.SetBool("Attack", false);
            clipNormalizedTime = 0f;
            if (shooted)
                shooted = !shooted;
            if (enemyBehavior.attacking)
                enemyBehavior.attacking = !enemyBehavior.attacking;
        }
    }
    private void Shoot(bool shooted)
    {
        if (!shooted)
            Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}