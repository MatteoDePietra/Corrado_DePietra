using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Animator animator;
    PlayerMovement playerMovement;
    [SerializeField]
    private float life = 5;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if ((Input.GetKeyUp(KeyCode.Escape)) && (Time.timeScale == 1))
            Time.timeScale = 0;
        else if ((Input.GetKeyUp(KeyCode.Escape)) && (Time.timeScale == 0))
            Time.timeScale = 1;
        HealthCheck();
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
        playerMovement.moveSpeed = 1.5f;
    }

    private void HealthCheck()
    {
        if (life <= 0)
        {
            Time.timeScale = 0;
            if (Input.GetKeyUp(KeyCode.Escape))
                Application.LoadLevel(0);
        }
    }
}