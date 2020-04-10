using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;
    LayerMask layer;

    private float life = 3;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        layer = LayerMask.GetMask("Tilemap");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        HealthCheck();
    }

    private void CheckGround()
    {
        float distance = 0.03f;
        if ((Physics2D.Raycast(transform.position, Vector2.down, distance, layer)) && (animator.GetBool("Damage")))
        {
            animator.SetBool("Damage", false);
        }
    }

    public void Damage(float damage)
    {
        life -= damage;
        Debug.Log("Vita del nemico: " + life);
        animator.SetBool("Damage", true);
        body.velocity = new Vector2(0, 0);
        body.AddForce(Vector2.up * 0.2f, ForceMode2D.Impulse);
    }

    private void HealthCheck()
    {
        if (life == 0)
            gameObject.SetActive(false);
    }
}
