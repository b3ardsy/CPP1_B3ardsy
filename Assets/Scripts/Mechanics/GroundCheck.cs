using UnityEngine;

public class GroundCheck
{
    private bool isGrounded = false;

    private LayerMask groundLayer;
    private Rigidbody2D rb;
    private Collider2D col;
    private float rayLength;

    public bool IsGrounded => isGrounded;

    public GroundCheck(Collider2D col, Rigidbody2D rb, float rayLength, LayerMask groundLayer)
    {
        this.col = col;
        this.rb = rb;
        this.rayLength = rayLength;
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

        isGrounded = hit.collider != null;

        Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);
    }
}