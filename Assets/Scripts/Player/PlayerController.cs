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
        bool aimInput = Input.GetMouseButton(1); // RMB held

        // Move player
        rb.linearVelocityX = horizontalInput * moveSpeed;

        // Send movement to Animator
        animator.SetFloat("xVelocity", Mathf.Abs(horizontalInput));
        animator.SetBool("isAiming", aimInput);

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
        if (jumpInput)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}