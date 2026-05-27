using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MageEnemy : BaseEnemy
{
    private Rigidbody2D rb;

    public override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        // Keeps the mage upright
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (IsDead)
            return;

        // Stops horizontal drifting
        rb.linearVelocity = new Vector2(0f, rb.linearVelocityY);
    }

    public void DestroyMage()
    {
        DestroyEnemy();
    }
}