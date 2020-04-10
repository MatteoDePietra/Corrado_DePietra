using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;
    LayerMask layer;
    PlayerMovement playerMovement;

    [SerializeField]
    private float life = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        layer = LayerMask.GetMask("Tilemap");
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        if (((Input.GetKeyUp(KeyCode.Escape)) && (Time.timeScale == 1)) || (life == 0))
            Time.timeScale = 0;
        else if ((Input.GetKeyUp(KeyCode.Escape)) && (Time.timeScale == 0))
            Time.timeScale = 1;
        if ((life == 0) && (Input.GetKeyUp(KeyCode.Escape)))
            Application.LoadLevel(0);
    }

    private void CheckGround()
    {
        float distance = 0.03f;

        if ((Physics2D.Raycast(transform.position, Vector2.down, distance, layer)) && (animator.GetBool("Damage")))
        {
            animator.SetBool("Damage", false);
            playerMovement.moveSpeed = 1.7f;
        }
    }

    public void Damage(float damage)
    {
        life -= damage;
        animator.SetBool("Damage", true);
        playerMovement.moveSpeed = 0f;
        body.velocity = new Vector2(0, 0);
        body.AddForce(Vector2.up *  0.2f, ForceMode2D.Impulse);
        Debug.Log("La vita del giocatore è: " + life);
    }
}