using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRayLength = 0.15f;

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
    private bool firePressed;
    private bool aimInput;

    private bool _isGrounded;

    private bool isTouchingLadder;
    private bool isOnLadder;
    //private bool isClimbing;

    private float startingGravity;

    void Start()
    {
        GetComponents();
        SetupGroundCheck();

        startingGravity = rb.gravityScale;
    }

    void Update()
    {
        ReadInput();
        MovementState();
        SpriteFlipping();
        FireAttack();
        UpdateAnims();
    }

    void FixedUpdate()
    {
        CheckGround();

        if (isOnLadder)
        {
            LadderMovement();
        }
        else
        {
            GroundMovement();
        }

        Jump();

        jumpPressed = false;
    }

    private void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void SetupGroundCheck()
    {
        groundCheck = new GroundCheck(
            col,
            rb,
            groundRayLength,
            groundLayer
        );
    }

    private void ReadInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }

        firePressed = Input.GetButtonDown("Fire1");
        aimInput = Input.GetMouseButton(1);
    }

    private void CheckGround()
    {
        groundCheck.Check();
        _isGrounded = groundCheck.IsGrounded;
    }

    private void MovementState()
    {
        if (isTouchingLadder && Mathf.Abs(verticalInput) > 0f)
        {
            isOnLadder = true;
        }

        //isClimbing = isOnLadder && Mathf.Abs(verticalInput) > 0f;
    }

    private void GroundMovement()
    {
        rb.gravityScale = startingGravity;

        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            rb.linearVelocityY
        );
    }

    private void LadderMovement()
    {
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            verticalInput * climbSpeed
        );
    }

    private void Jump()
    {
        if (!jumpPressed)
            return;

        if (isOnLadder)
        {
            JumpOffLadder();
            return;
        }

        if (!_isGrounded)
            return;

        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void JumpOffLadder()
    {
        isOnLadder = false;
        //isClimbing = false;

        rb.gravityScale = startingGravity;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void SpriteFlipping()
    {
        if (horizontalInput == 0)
            return;

        sr.flipX = horizontalInput < 0;
    }

    private void FireAttack()
    {
        if (!firePressed)
            return;

        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length > 0 && clipInfo[0].clip.name != "Fire")
        {
            anim.SetTrigger("Fire");
        }
    }

    private void UpdateAnims()
    {
        anim.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));
        anim.SetBool("isGrounded", _isGrounded);
        anim.SetFloat("yVel", rb.linearVelocityY);
        anim.SetBool("isAiming", aimInput);
        anim.SetBool("isOnLadder", isOnLadder);
        anim.SetFloat("climbAnimSpeed", verticalInput);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LadderTriggerEnter(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        LadderTriggerEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ladder"))
            return;

        ExitLadder();
    }

    private void LadderTriggerEnter(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isTouchingLadder = true;
        }
    }

    private void ExitLadder()
    {
        isTouchingLadder = false;
        isOnLadder = false;
        //isClimbing = false;

        rb.gravityScale = startingGravity;
    }
}