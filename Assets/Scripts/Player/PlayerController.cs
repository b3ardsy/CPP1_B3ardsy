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

    [Header("Ladder")]
    [SerializeField] private float climbSpeed = 5f;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private SpriteRenderer sr;
    private Animator anim;

    private GroundCheck groundCheck;

    private float horizontalInput;
    private float verticalInput;

    private bool jumpPressed;
    private bool aimInput;

    private bool _isGrounded;
    private Vector2 groundNormal = Vector2.up;

    private bool isTouchingLadder;
    private bool isOnLadder;
    private bool isClimbing;

    private float startingGravity;

    private bool _isFiring;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        groundCheck = new GroundCheck(
            col,
            rb,
            groundRayLength,
            maxSlopeAngle,
            groundLayer
        );

        startingGravity = rb.gravityScale;
    }

    void Update()
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        bool fireInput = Input.GetButtonDown("Fire1");

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        aimInput = Input.GetMouseButton(1);
        

        if (horizontalInput != 0)
            SpriteFlip(horizontalInput);

        if (isTouchingLadder && Mathf.Abs(verticalInput) > 0f)
        {
            isOnLadder = true;
        }

        isClimbing = isOnLadder && Mathf.Abs(verticalInput) > 0f;

        anim.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));
        anim.SetBool("isGrounded", _isGrounded);
        anim.SetFloat("yVel", rb.linearVelocityY);
        anim.SetBool("isAiming", aimInput);
        anim.SetBool("isOnLadder", isOnLadder);
        anim.SetBool("isClimbing", isClimbing);

        if (fireInput && clipInfo[0].clip.name != "Fire")
        {
            anim.SetTrigger("Fire");
        }
 
    }

    void FixedUpdate()
    {
        groundCheck.Check();

        _isGrounded = groundCheck.IsGrounded;
        groundNormal = groundCheck.GroundNormal;

        if (isOnLadder)
        {
            ClimbLadder();
        }
        else
        {
            Move();

            if (jumpPressed && _isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        jumpPressed = false;
    }

    void Move()
    {
        rb.gravityScale = startingGravity;

        if (_isGrounded)
        {
            Vector2 slopeDirection = new Vector2(
                groundNormal.y,
                -groundNormal.x
            );

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

    void ClimbLadder()
    {
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            verticalInput * climbSpeed
        );

        if (jumpPressed)
        {
            isOnLadder = false;
            isClimbing = false;

            rb.gravityScale = startingGravity;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void SpriteFlip(float horizontalInput)
    {
        sr.flipX = horizontalInput < 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = false;
            isOnLadder = false;
            isClimbing = false;
            rb.gravityScale = startingGravity;
        }
    }
}