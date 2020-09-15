using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint = null;
    [SerializeField]
    private GameObject bullet1 = null;
    [SerializeField]
    private GameObject bullet2 = null;

    Animator animator;
    AnimatorClipInfo[] currentClipInfo;
    AudioManager audioManager;

    private float clipNormalizedTime;
    private bool FirstShot;
    private bool SecondShot;


    private void Start()
    {
        animator = GetComponent<Animator>();
        FirstShot = false;
        SecondShot = false;
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager non trovato");
        }
    }

    private void Update()
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
                audioManager.PlaySound("Shot");
            }
            else if (currentClipInfo[0].clip.name.Equals("Walk"))
            {
                animator.SetBool("Attack_Walk", true);
                audioManager.PlaySound("Shot");
            }
            else if (currentClipInfo[0].clip.name.Equals("Run"))
            {
                animator.SetBool("Attack_Run", true);
                audioManager.PlaySound("Shot");

            }
            else if ((currentClipInfo[0].clip.name.Equals("Attack_Player")) && (clipNormalizedTime > 0.85))
            {
                animator.SetBool("Attack_Extra", true);
                audioManager.PlaySound("Shot");
            }

        }
        if ((clipNormalizedTime >= .3f) && (clipNormalizedTime < 1f))
        {
            if (((currentClipInfo[0].clip.name.Equals("Attack_Player")) || (currentClipInfo[0].clip.name.Equals("Attack_Walk")) || (currentClipInfo[0].clip.name.Equals("Attack_Run")))&& !FirstShot)
            {
                Shoot(1);
                FirstShot = true;
            }
            else if ((currentClipInfo[0].clip.name.Equals("Attack_Extra")) && !SecondShot)
            {
                Shoot(2);
                SecondShot = true;
            }

        }
        else if (clipNormalizedTime >= 1f)
        {
            animator.SetBool("Attack_Player", false);
            animator.SetBool("Attack_Walk", false);
            animator.SetBool("Attack_Run", false);
            animator.SetBool("Attack_Extra", false);
            clipNormalizedTime = 0f;
            if (FirstShot)
                FirstShot = !FirstShot;
            if (SecondShot)
                SecondShot = !SecondShot;
        }
    }

    private void Shoot(int bulletNumber)
    {
        if (bulletNumber == 1)
            Instantiate(bullet1, firePoint.position, firePoint.rotation);
        else if (bulletNumber == 2)
            Instantiate(bullet2, firePoint.position, firePoint.rotation);
    }
}
