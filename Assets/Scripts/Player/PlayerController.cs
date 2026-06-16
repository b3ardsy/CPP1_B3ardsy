using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    #region Settings and Configurable Variables

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRayLength = 0.15f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float acceleration = 80f;
    [SerializeField] private float deceleration = 100f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 18f;
    [SerializeField] private float fallMultiplier = 2.8f;
    [SerializeField] private float lowJumpMultiplier = 2.2f;
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("Roll")]
    [SerializeField] private float rollSpeed = 15f;
    [SerializeField] private float rollDuration = 0.35f;
    [SerializeField] private Vector2 rollingColliderSize = new Vector2(0.706f, 0.8f);
    [SerializeField] private Vector2 rollingColliderOffset = new Vector2(-0.027f, 0.4f);

    [Header("Ladder")]
    [SerializeField] private float climbSpeed = 5f;

    #endregion

    #region State Variables

    private bool isRolling;
    private float rollTimer;

    private Vector2 standingColliderSize;
    private Vector2 standingColliderOffset;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    #endregion

    #region Component References

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    private PlayerStats stats;

    private GroundCheck groundCheck;

    private float horizontalInput;
    private float verticalInput;

    private bool firePressed;
    private bool aimInput;
    private bool rollPressed;

    private bool _isGrounded;

    private bool isTouchingLadder;
    private bool isOnLadder;

    private float startingGravity;

    #endregion

    #region Compatibility Properties

    public int specialAmmo
    {
        get { return stats.CurrentSpecialAmmo; }
        set { stats.SetSpecialAmmo(value); }
    }

    public int bountyTokens
    {
        get { return stats.BountyTokens; }
        set { stats.SetBountyTokens(value); }
    }

    public bool HasRoll { get { return stats.HasRoll; } }
    public bool HasFireArrow { get { return stats.HasFireArrow; } }
    public bool HasIceArrow { get { return stats.HasIceArrow; } }
    public bool HasKey { get { return stats.HasKey; } }

    #endregion

    void Start()
    {
        GetComponents();
        SetupGroundCheck();

        startingGravity = rb.gravityScale;

        standingColliderSize = col.size;
        standingColliderOffset = col.offset;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsPaused)
            return;

        ReadInput();
        UpdateJumpTimers();
        MovementState();
        SpriteFlipping();
        FireAttack();
        Roll();
        UpdateAnims();
        HandleSpecialArrowSwap();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsPaused)
            return;

        CheckGround();

        if (isOnLadder)
        {
            LadderMovement();
        }
        else
        {
            GroundMovement();
            ApplyJumpGravity();
        }

        Jump();
    }

    private void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
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

    private void HandleSpecialArrowSwap()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            stats.ToggleSpecialArrow();
        }
    }

    private void ReadInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;

        firePressed = Input.GetButtonDown("Fire1");
        aimInput = Input.GetMouseButton(1);
        rollPressed = Input.GetKeyDown(KeyCode.LeftShift);
    }

    private void UpdateJumpTimers()
    {
        if (_isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0f)
            jumpBufferCounter -= Time.deltaTime;
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
    }

    private void GroundMovement()
    {
        if (isRolling)
            return;

        rb.gravityScale = startingGravity;

        float targetSpeed = horizontalInput * moveSpeed;

        float newXVelocity = Mathf.MoveTowards(
            rb.linearVelocityX,
            targetSpeed,
            (Mathf.Abs(horizontalInput) > 0.01f ? acceleration : deceleration) * Time.fixedDeltaTime
        );

        if (Mathf.Abs(horizontalInput) < 0.01f && Mathf.Abs(newXVelocity) < 0.05f)
            newXVelocity = 0f;

        rb.linearVelocity = new Vector2(newXVelocity, rb.linearVelocityY);
    }

    private void LadderMovement()
    {
        if (isRolling)
            return;

        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            verticalInput * climbSpeed
        );
    }

    private void Jump()
    {
        if (isRolling)
            return;

        if (jumpBufferCounter <= 0f)
            return;

        if (isOnLadder)
        {
            JumpOffLadder();
            jumpBufferCounter = 0f;
            return;
        }

        if (coyoteTimeCounter <= 0f)
            return;

        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;
    }

    private void JumpOffLadder()
    {
        isOnLadder = false;

        rb.gravityScale = startingGravity;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void ApplyJumpGravity()
    {
        if (isRolling || isOnLadder)
            return;

        if (rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocityY > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    private void SpriteFlipping()
    {
        if (isRolling)
            return;

        if (horizontalInput == 0)
            return;

        sr.flipX = horizontalInput < 0;
    }

    private void FireAttack()
    {
        if (isRolling)
            return;

        if (!firePressed)
            return;

        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length > 0 && clipInfo[0].clip.name != "Fire")
        {
            anim.SetTrigger("Fire");
        }
    }

    private void Roll()
    {
        if (!stats.HasRoll)
            return;

        if (isRolling)
        {
            rollTimer -= Time.deltaTime;

            float direction = sr.flipX ? -1f : 1f;

            rb.linearVelocity = new Vector2(
                direction * rollSpeed,
                rb.linearVelocityY
            );

            if (rollTimer <= 0f)
            {
                isRolling = false;
                col.size = standingColliderSize;
                col.offset = standingColliderOffset;
            }

            return;
        }

        if (rollPressed && _isGrounded && !isOnLadder)
        {
            isRolling = true;
            rollTimer = rollDuration;

            col.size = rollingColliderSize;
            col.offset = rollingColliderOffset;

            anim.SetTrigger("Roll");
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

    public void JumpForceChange()
    {
        // Not using this yet
    }

    public bool TrySpendSpecialAmmo(int amount)
    {
        return stats.TrySpendSpecialAmmo(amount);
    }

    public void PickUpAmmoTank()
    {
        stats.PickUpAmmoTank();
    }

    public void PickUpRoll()
    {
        stats.UnlockRoll();
    }

    public void PickUpFireArrow()
    {
        stats.UnlockFireArrow();
    }

    public void PickUpIceArrow()
    {
        stats.UnlockIceArrow();
    }

    public void PickUpKey()
    {
        stats.PickUpKey();
    }

    public void PickUpBountyToken()
    {
        stats.AddBountyToken();
    }

    public void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);

        if (stats.IsDead())
        {
            if (SFXManager.Instance != null)
            {
                SFXManager.Instance.PlayPlayerDeath();
            }

            anim.SetTrigger("Death");
        }
        else
        {
            if (SFXManager.Instance != null)
            {
                SFXManager.Instance.PlayPlayerHit();
            }

            anim.SetTrigger("Hit");
        }
    }

    public void LoadGameOver()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadGameOverFromAnimation();
        }
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

        rb.gravityScale = startingGravity;
    }
}