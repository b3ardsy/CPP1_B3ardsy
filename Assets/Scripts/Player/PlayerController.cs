using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool jumpInput = Input.GetButtonDown("Jump");
        bool aimInput = Input.GetMouseButton(1);
        bool shootInput = Input.GetMouseButtonDown(0);

        // Move player
        rb.linearVelocityX = horizontalInput * moveSpeed;

        // Animator parameters
        animator.SetFloat("xVelocity", Mathf.Abs(horizontalInput));
        animator.SetFloat("yVelocity", rb.linearVelocityY);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isAiming", aimInput);

        // Shoot while idle
        if (shootInput && Mathf.Abs(horizontalInput) < 0.1f && isGrounded)
        {
            animator.SetTrigger("shoot");
        }

        // Flip sprite
        if (horizontalInput > 0.01f)
        {
            sr.flipX = false;
        }
        else if (horizontalInput < -0.01f)
        {
            sr.flipX = true;
        }

        // Jump
        if (jumpInput && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
}