using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float clipNormalizedTime;
    private AnimatorClipInfo[] currentClipInfo;
    [SerializeField]
    private float speed = 3.5f;
    [SerializeField]
    private Rigidbody2D body = null;
    private AudioManager audioManager;
    private Animator animator;
    [SerializeField]
    private int damage;
    private float timer = 1.1f;
    private void Start()
    {
        animator = GetComponent<Animator>();
        body.velocity = transform.right * speed;
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager non trovato");
        }
    }
    private void Update()
    {
        currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);

        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            StartCoroutine(BulletDestroy());
        }
        if (currentClipInfo[0].clip.name.Equals("Bullet_End"))
            clipNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if ((other.CompareTag("Enemy")) && (this.CompareTag("Player")))
        {
            other.GetComponent<EnemyHealth>().Damage(damage);
            StartCoroutine(BulletDestroy());
        }
        else if ((other.CompareTag("Player")) && (this.CompareTag("Enemy")))
        {
            other.GetComponent<PlayerHealth>().Damage(damage);
            StartCoroutine(BulletDestroy());
        }
    }
    private IEnumerator BulletDestroy()
    {
        speed = 0;
        body.velocity = transform.right * speed;
        audioManager.PlaySound("Explosion");
        animator.SetTrigger("Impact");

        yield return new WaitUntil(() => (currentClipInfo[0].clip.name.Equals("Bullet_End")) && (clipNormalizedTime >= 1));
       
        Destroy(gameObject);
    }
}