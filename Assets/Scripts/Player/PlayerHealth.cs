using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float clipNormalizedTime;
    private AnimatorClipInfo[] currentClipInfo;
    private Rigidbody2D body;
    private Animator animator;
    private PlayerMovement playerMovement;
    private AudioManager audioManager;
    private MainMenu mainMenu;
    [SerializeField]
    private HealthBar healthBar;
    private bool alive;
    [SerializeField]
    private int maxHealth;
    private int currentHealth;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        alive = true;
        currentHealth = maxHealth = 5;
        healthBar.SetMaxHealth(maxHealth);

        mainMenu = MainMenu.instance;
        if (mainMenu == null)
        {
            GameObject gameObject = GameObject.Find("Menu");
            mainMenu = gameObject.GetComponent<MainMenu>();
        }

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            GameObject gameObject = GameObject.Find("AudioManager");
            audioManager = gameObject.GetComponent<AudioManager>();
        }
    }
    private void Update()
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
    public void Heal (int heal)
    {
        if (currentHealth + heal > maxHealth)
        {
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            currentHealth += heal;
            healthBar.SetHealth(currentHealth);
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

        CoinCounter.CounterReset();
        EnemyCounter.CounterReset();
        DiamondCounter.CounterReset();
        Time.timeScale = 0f;
        gameObject.SetActive(false);
        mainMenu.GameOver();
    }
}