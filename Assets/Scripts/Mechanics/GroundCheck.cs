using UnityEngine;

public class GroundCheck
{
    private bool isGrounded = false;
    private Vector2 groundNormal = Vector2.up;

    private LayerMask groundLayer;
    private Rigidbody2D rb;
    private Collider2D col;
    private float rayLength;
    private float maxSlopeAngle;

    public bool IsGrounded => isGrounded;
    public Vector2 GroundNormal => groundNormal;

    public GroundCheck(Collider2D col, Rigidbody2D rb, float rayLength, float maxSlopeAngle, LayerMask groundLayer)
    {
        this.col = col;
        this.rb = rb;
        this.rayLength = rayLength;
        this.maxSlopeAngle = maxSlopeAngle;
        this.groundLayer = groundLayer;
    }

    public void Check()
    {
        Bounds bounds = col.bounds;

        Vector2 rayOrigin = new Vector2(
            bounds.center.x,
            bounds.min.y + 0.05f
        );

        RaycastHit2D hit = Physics2D.Raycast(
            rayOrigin,
            Vector2.down,
            rayLength,
            groundLayer
        );

        if (hit.collider != null)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            isGrounded = slopeAngle <= maxSlopeAngle;
            groundNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector2.up;
        }

        Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);
    }
}