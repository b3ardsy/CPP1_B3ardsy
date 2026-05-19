using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkerEnemy : BaseEnemy
{
    [SerializeField] private float xVel = 2f;

    private Rigidbody2D rb;

    public override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        // Stops the enemy from tipping/rotating
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (IsDead)
            return;

        float direction = sr.flipX ? 1f : -1f;

        rb.linearVelocity = new Vector2(direction * xVel, rb.linearVelocityY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsDead)
            return;

        if (collision.CompareTag("Barrier"))
        {
            TurnAround();
        }
    }

    private void TurnAround()
    {
        sr.flipX = !sr.flipX;
    }
}