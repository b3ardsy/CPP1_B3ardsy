using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public abstract class BaseEnemy : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Animator anim;
    protected int health;

    [SerializeField] protected int maxHealth = 5;

    [Header("Drops")]
    [SerializeField] private GameObject healthDropPrefab;
    [SerializeField] private GameObject ammoDropPrefab;
    [SerializeField, Range(0f, 1f)] private float dropChance = 0.75f;
    [SerializeField] private int minDrops = 1;
    [SerializeField] private int maxDrops = 5;
    [SerializeField] private float dropForce = 5f;
    [SerializeField] private float upwardForce = 2f;

    protected bool isDead = false;

    public bool IsDead => isDead;

    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (maxHealth <= 0)
        {
            maxHealth = 5;
        }

        health = maxHealth;
    }

    public virtual void TakeDamage(int damage, DamageType damageType = DamageType.Default)
    {
        if (isDead)
            return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("Hit");
        }
    }

    protected virtual void Die()
    {
        isDead = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Collider2D enemyCollider = GetComponent<Collider2D>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        DropOrbs();
        anim.SetTrigger("Death");
    }

    public virtual void DestroyEnemy()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void DropOrbs()
    {
        if (Random.value > dropChance)
            return;

        int dropCount = Random.Range(minDrops, maxDrops + 1);

        for (int i = 0; i < dropCount; i++)
        {
            GameObject dropPrefab = Random.value < 0.5f ? healthDropPrefab : ammoDropPrefab;

            if (dropPrefab == null)
                continue;

            GameObject drop = Instantiate(
                dropPrefab,
                transform.position,
                Quaternion.identity
            );

            Rigidbody2D dropRb = drop.GetComponent<Rigidbody2D>();

            if (dropRb != null)
            {
                Vector2 randomDirection = new Vector2(
                    Random.Range(-1f, 1f),
                    Random.Range(0.5f, 1f)
                ).normalized;

                dropRb.AddForce(
                    new Vector2(
                        randomDirection.x * dropForce,
                        randomDirection.y * dropForce + upwardForce
                    ),
                    ForceMode2D.Impulse
                );
            }
        }
    }
}

public enum DamageType
{
    Default,
    JumpedOn
}