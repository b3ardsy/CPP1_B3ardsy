using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkerEnemy : BaseEnemy
{
    [Header("Movement")]
    [SerializeField] private float xVel = 2f;

    [Header("Contact Damage")]
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float damageCooldown = 0.75f;

    private Rigidbody2D rb;
    private float nextDamageTime = 0f;

    public override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
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
        HandleCollider(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollider(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollider(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollider(collision.collider);
    }

    private void HandleCollider(Collider2D collision)
    {
        if (IsDead)
            return;

        if (collision.CompareTag("Barrier"))
        {
            TurnAround();
            return;
        }

        if (collision.CompareTag("Player"))
        {
            DamagePlayer(collision);
        }
    }

    private void DamagePlayer(Collider2D collision)
    {
        if (Time.time < nextDamageTime)
            return;

        PlayerController player = collision.GetComponent<PlayerController>();

        if (player == null)
            return;

        Vector2 knockbackDirection = collision.transform.position - transform.position;
        knockbackDirection = knockbackDirection.normalized;

        player.TakeDamage(contactDamage, knockbackDirection);

        nextDamageTime = Time.time + damageCooldown;
    }

    private void TurnAround()
    {
        sr.flipX = !sr.flipX;
    }

    protected override void PlayHitSFX()
    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayWalkerHit();
        }
    }

    protected override void PlayDeathSFX()
    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayWalkerDeath();
        }
    }
}