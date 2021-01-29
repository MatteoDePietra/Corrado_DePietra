using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float clipNormalizedTime;
    private AnimatorClipInfo[] currentClipInfo;
    public Rigidbody2D body;
    public Animator animator;
    public PlayerMovement playerMovement;
    public AudioManager audioManager;
    public HealthBar healthBar; 
    public MainMenu mainMenu;
    private bool alive;

    [SerializeField]
    private int maxHealth;
    private int currentHealth;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        audioManager = AudioManager.instance;
        currentHealth = maxHealth = 5;
        alive = true;
        healthBar.SetMaxHealth(maxHealth);
        if (audioManager == null)
        {
            Debug.LogError("AudioManager non trovato");
        }
    }
    void Update()
    {
        if (mainMenu == null)
        {
            GameObject go = GameObject.Find("Menu");
            mainMenu = go.GetComponent<MainMenu>();
        }

        currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (currentClipInfo[0].clip.name.Equals("Death"))
        {
            clipNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        
        HealthCheck();
    }
    public void Damage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            animator.SetTrigger("Damage");
            StartCoroutine(stopMovement());
            audioManager.PlaySound("Damage");
        }
    }
    private IEnumerator stopMovement()
    {
        playerMovement.moveSpeed.x = 0f;
        yield return new WaitForSecondsRealtime(.3f);
        playerMovement.moveSpeed.x = 1.8f;
    }
    private void HealthCheck()
    {
        if ((currentHealth <= 0) && (alive))
        {
            StartCoroutine(Death());
        }
    }
    private IEnumerator Death()
    {
        alive = false;
        animator.SetTrigger("Death");
        playerMovement.moveSpeed = new Vector2 (0f, 0f);
        body.velocity = new Vector2(0, 0);
        body.bodyType = RigidbodyType2D.Kinematic;
        clipNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        yield return new WaitUntil(() => (currentClipInfo[0].clip.name.Equals("Death")) && (clipNormalizedTime > 1));

        CoinCounter.CoinReset();
        Time.timeScale = 0f;
        gameObject.SetActive(false);
        mainMenu.GameOver();
    }
}