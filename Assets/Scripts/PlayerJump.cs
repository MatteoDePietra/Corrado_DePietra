using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody2D body;
    LayerMask layer;
    Animator animator;

    [SerializeField]
    public static float jumpForce = 0.4f;

    // Var for jumps
    private byte jump = 0;

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
        Jumping();
    }

    // Check if player is on the ground
    private void CheckGround()
    {
        float distance = 0.01f;
        if (Physics2D.Raycast(transform.position, Vector2.down, distance, layer) && (jump!=0))
        {
            Debug.Log("a terra!");
            jump = 0;
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsHighJumping", false);
        }
    }

    // Control multiple jump
    private void Jumping()
    {
        if ((Input.GetKeyDown(KeyCode.Space)) && (jump  == 0))
        {
            body.velocity = new Vector2(Vector2.right.x, 0);
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = 1;
            animator.SetBool("IsJumping", true);
        }
        else if ((Input.GetKeyDown(KeyCode.Space)) && (jump == 1))
        {
            body.velocity = new Vector2(Vector2.right.x, 0);
            body.AddForce(Vector2.up * jumpForce * (float) 1.2, ForceMode2D.Impulse);
            jump = 2;
            animator.SetBool("IsHighJumping", true);
        }
    }
}