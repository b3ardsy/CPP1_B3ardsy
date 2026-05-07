using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    #region Settings and Configurable Variables

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRayLength = 0.15f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Roll")]
    [SerializeField] private float rollSpeed = 15f;
    [SerializeField] private float rollDuration = 0.35f;
    [SerializeField] private Vector2 rollingColliderSize = new Vector2(0.706f, 0.8f);
    [SerializeField] private Vector2 rollingColliderOffset = new Vector2(-0.027f, 0.4f);

    [Header("Ladder")]
    [SerializeField] private float climbSpeed = 5f;

    [Header("Player Settings")]
    [SerializeField] private int maxHealthTanks = 9;
    [SerializeField] private int maxSpecialAmmo = 20;

    #endregion

    #region State Variables

    private int _healthTank = 3;

    public int healthTank
    {
        get { return _healthTank; }
        set
        {
            if (value > maxHealthTanks)
                _healthTank = maxHealthTanks;
            else if (value < 0)
                _healthTank = 0;
            else
                _healthTank = value;

            Debug.Log($"Health Tanks have changed to {_healthTank}");
        }
    }

    private int _specialAmmo = 5;

    public int specialAmmo
    {
        get { return _specialAmmo; }
        set
        {
            if (value > maxSpecialAmmo)
                _specialAmmo = maxSpecialAmmo;
            else if (value < 0)
                _specialAmmo = 0;
            else
                _specialAmmo = value;

            Debug.Log($"Special Ammo has changed to {_specialAmmo}");
        }
    }

    private int _bountyTokens = 0;

    public int bountyTokens
    {
        get { return _bountyTokens; }
        set
        {
            _bountyTokens = Mathf.Max(0, value);
            Debug.Log($"Bounty Tokens Acquired {_bountyTokens}");
        }
    }

    private bool hasRoll = false;
    private bool hasFireArrow = false;
    private bool hasIceArrow = false;
    private bool hasKey = false;

    private bool isRolling;
    private float rollTimer;

    private Vector2 standingColliderSize;
    private Vector2 standingColliderOffset;

    #endregion

    #region Component References

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
    private bool rollPressed;

    private bool _isGrounded;

    private bool isTouchingLadder;
    private bool isOnLadder;

    private float startingGravity;

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
        ReadInput();
        MovementState();
        SpriteFlipping();
        FireAttack();
        Roll();
        UpdateAnims();
    }

    void FixedUpdate()
    {
        CheckGround();

        if (isOnLadder)
            LadderMovement();
        else
            GroundMovement();

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
            jumpPressed = true;

        firePressed = Input.GetButtonDown("Fire1");
        aimInput = Input.GetMouseButton(1);
        rollPressed = Input.GetKeyDown(KeyCode.LeftShift);
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

        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            rb.linearVelocityY
        );
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

        rb.gravityScale = startingGravity;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
        if (!hasRoll)
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

    public void PickUpRoll()
    {
        hasRoll = true;
        Debug.Log("Roll unlocked");
    }

    public void PickUpFireArrow()
    {
        hasFireArrow = true;
        Debug.Log("Fire Arrow unlocked");
    }

    public void PickUpIceArrow()
    {
        hasIceArrow = true;
        Debug.Log("Ice Arrow unlocked");
    }

    public void PickUpKey()
    {
        hasKey = true;
        Debug.Log("Key acquired");
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