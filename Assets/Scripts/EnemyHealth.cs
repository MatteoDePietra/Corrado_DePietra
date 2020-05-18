using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    private float life = 3;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthCheck();
    }

    public void Damage(float damage)
    {
        life -= damage;
        animator.SetTrigger("Damage");
        Debug.Log("Vita del nemico: " + life);
    }

    private void HealthCheck()
    {
        if (life <= 0)
            gameObject.SetActive(false);
    }
}