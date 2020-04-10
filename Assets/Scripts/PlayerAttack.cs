using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //Rigidbody2D body;
    Animator animator;
    EnemyHealth enemyHealth;
    AnimatorClipInfo[] currentClipInfo;
    private float clipNormalizedTime;
    private float damage = 1;
    private bool AttackRange;
    private bool Attacked;

    // Start is called before the first frame update
    void Start()
    {
        //body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Attacked = false;
        AttackRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
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
            AttackRange = true;
        else if (clipNormalizedTime >= 1f)
        {
            animator.SetBool("Attack_Player", false);
            animator.SetBool("Attack_Walk", false);
            animator.SetBool("Attack_Run", false);
            animator.SetBool("Attack_Extra", false);
            clipNormalizedTime = 0f;
            AttackRange = false;
            if (Attacked)
                Attacked = !Attacked;
        }
    }

    /*private void OnCollisionStay2D(Collision2D other)
    {
        if ((other.gameObject.tag == "Enemy") && (AttackRange))
        {
            if (!Attacked)
            {
                enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.Damage(damage);
                Attacked = true;
            }
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.tag == "Enemy") && (AttackRange))
        {
            if (!Attacked)
            {
                enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.Damage(damage);
                Attacked = true;
            }
        }
    }
}
