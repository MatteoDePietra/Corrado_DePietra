using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D boxCollider2D;
    private LayerMask layer;
    private Animator animator;
    private AudioManager audioManager;
    [SerializeField]
    private float jumpForce = 0.45f;
    private byte jump = 0;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        layer = LayerMask.GetMask("Tilemap");
        animator = GetComponent<Animator>();
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager non trovato");
        }
    }
    private void Update()
    {
        Jumping();
        if (IsGrounded() && (body.velocity.y == 0) && (jump != 0))
        {
            animator.SetBool("IsGrounded", true);
            jump = 0;
            audioManager.PlaySound("Landing");
        }
        if ((body.velocity.y != 0) && (jump == 0))
        {
            jump = 1;
        }
    }
    private bool IsGrounded() 
    {
        float extraHeightText = .07f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeightText, layer);

        Color rayColor;
        if (raycastHit.collider != null) 
        {
            rayColor = Color.green;
        } 
        else 
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2D.bounds.extents.x * 2f), rayColor);

        return raycastHit.collider != null;
    }
    private void Jumping()
    {
        if ((Input.GetKeyDown(KeyCode.Space)) && (jump  == 0) && IsGrounded())
        {
            body.velocity = new Vector2(Vector2.right.x, 0);
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = 1;
            animator.SetBool("IsGrounded", false);

            animator.SetTrigger("Jump");
            audioManager.PlaySound("Jump1");
        }
        else if ((Input.GetKeyDown(KeyCode.Space)) && (jump == 1))
        {
            body.velocity = new Vector2(Vector2.right.x, 0);
            body.AddForce(Vector2.up * jumpForce * .9f, ForceMode2D.Impulse);
            jump = 2;
            animator.SetBool("IsGrounded", false);
            animator.SetTrigger("HighJump");
            audioManager.PlaySound("Jump2");
        }
    }
}