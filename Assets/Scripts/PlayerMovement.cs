﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;

    [SerializeField]
    public float moveSpeed = 1.5f;
    [SerializeField]
    public float moveCoin = 1f;                                                 // If Coin velocity, moveCoin = 2;
    [SerializeField]
    public bool moveMirror = false;                                             // If Coin mirror, velocity = -1;
    [SerializeField]
    public float timer = 10f;
    private float h;

    void Start()                                                                // Start is called before the first frame update
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
                                                                                
    void Update()                                                               // Update is called once per frame
    {
        Movement();
        animationMovement();
    }

    void Movement()
    {
        h = Input.GetAxis("Horizontal");                                  
        Vector2 velocity;                                                       // local var

        if (moveMirror)
        {
            velocity = new Vector2(Vector2.right.x * moveSpeed * moveCoin * -h, body.velocity.y);
        }
        else
        {
            velocity = new Vector2(Vector2.right.x * moveSpeed * moveCoin * h, body.velocity.y);
        }

        body.velocity = velocity;
        
    }

    void animationMovement()
    {
        animator.SetFloat("IsWalking", Mathf.Abs(h));                           // If the player is moving without Coin

        if (body.velocity.x < 0)
        {
            body.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (body.velocity.x > 0)
        {
            body.transform.localScale = new Vector3(1, 1, 1);
        }
        else if ((body.velocity.x == 0) && (body.velocity.y == 0) && (timer > 0))
        {
            timer -= Time.deltaTime;
        }

        if ((body.velocity.x != 0) || (body.velocity.y != 0) || (animator.GetBool("Attack_Player")))
        {
            timer = 10f;
            animator.SetBool("IsStill", false);
        }

        if (timer <= 0)                                                         // If the player is still more than 10 sec, animation Still
        {
            animator.SetBool("IsStill", true);
        }

        if (moveCoin > 1f)                                                      // If the player has take Coin velocity, animation Run
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }
}

