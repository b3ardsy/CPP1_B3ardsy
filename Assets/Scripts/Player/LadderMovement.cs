using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LadderMovement : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 8f;
    [SerializeField] private float normalGravity = 2f;

    private Rigidbody2D rb;

    private float verticalInput;
    private bool isTouchingLadder;
    private bool isClimbing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        verticalInput = Input.GetAxisRaw("Vertical");

        if (isTouchingLadder && Mathf.Abs(verticalInput) > 0f)
        {
            isClimbing = true;
        }

        if (!isTouchingLadder)
        {
            isClimbing = false;
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
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
            isClimbing = false;
            rb.gravityScale = normalGravity;
        }
    }
}