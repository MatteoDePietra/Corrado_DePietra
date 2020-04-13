using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : MonoBehaviour
{
    Animator animator;
    PlayerMovement playerMovement;
    [SerializeField]
    private float life = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((Input.GetKeyUp(KeyCode.Escape)) && (Time.timeScale == 1)) || (life == 0))
            Time.timeScale = 0;
        else if ((Input.GetKeyUp(KeyCode.Escape)) && (Time.timeScale == 0))
            Time.timeScale = 1;
        if ((life == 0) && (Input.GetKeyUp(KeyCode.Escape)))
            Application.LoadLevel(0);
    }

    public void Damage(float damage)
    {
        life -= damage;
        animator.SetTrigger("Damage");
        StartCoroutine(stopMovement());
        Debug.Log("La vita del giocatore è: " + life);
    }

    private IEnumerator stopMovement()
    {
        playerMovement.moveSpeed = 0f;
        yield return new WaitForSecondsRealtime(.3f);
        playerMovement.moveSpeed = 1.7f;
    }
}