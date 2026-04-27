using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRayLength = 0.15f;
    [SerializeField] private float maxSlopeAngle = 45f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private SpriteRenderer sr;
    private Animator anim;

    private float horizontalInput;
    private bool jumpPressed;
    private bool aimInput;
    private bool shootInput;

    private bool _isGrounded;
    private Vector2 groundNormal = Vector2.up;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        aimInput = Input.GetMouseButton(1);
        shootInput = Input.GetMouseButtonDown(0);

        if (horizontalInput != 0)
            SpriteFlip(horizontalInput);

        anim.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));
        anim.SetBool("isGrounded", _isGrounded);
        anim.SetFloat("yVel", rb.linearVelocityY);
        anim.SetBool("isAiming", aimInput);

        if (shootInput)
            anim.SetTrigger("shoot");
    }

    void FixedUpdate()
    {
        CheckGround();

        Move();

        if (jumpPressed && _isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        jumpPressed = false;
    }

    void CheckGround()
    {
        Bounds bounds = col.bounds;

        Vector2 rayOrigin = new Vector2(bounds.center.x, bounds.min.y + 0.05f);

        RaycastHit2D hit = Physics2D.Raycast(
            rayOrigin,
            Vector2.down,
            groundRayLength,
            groundLayer
        );

        if (hit.collider != null)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            _isGrounded = slopeAngle <= maxSlopeAngle;
            groundNormal = hit.normal;
        }
        else
        {
            _isGrounded = false;
            groundNormal = Vector2.up;
        }

        Debug.DrawRay(rayOrigin, Vector2.down * groundRayLength, Color.red);
    }

    void Move()
    {
        if (_isGrounded)
        {
            Vector2 slopeDirection = new Vector2(groundNormal.y, -groundNormal.x);

            if (horizontalInput < 0)
                slopeDirection *= -1;

            rb.linearVelocity = new Vector2(
                slopeDirection.x * Mathf.Abs(horizontalInput) * moveSpeed,
                slopeDirection.y * Mathf.Abs(horizontalInput) * moveSpeed
            );
        }
        else
        {
            rb.linearVelocity = new Vector2(
                horizontalInput * moveSpeed,
                rb.linearVelocityY
            );
        }
    }

    void SpriteFlip(float horizontalInput)
    {
        sr.flipX = horizontalInput < 0;
    }
}