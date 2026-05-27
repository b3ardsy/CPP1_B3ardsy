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

    [Header("Drop Explosion")]
    [SerializeField] private float dropSpawnHeight = 0.75f;
    [SerializeField] private float horizontalDropForce = 4f;
    [SerializeField] private float upwardDropForce = 6f;

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

            Vector3 dropSpawnPosition = transform.position + Vector3.up * dropSpawnHeight;

            GameObject drop = Instantiate(
                dropPrefab,
                dropSpawnPosition,
                Quaternion.identity
            );

            Rigidbody2D dropRb = drop.GetComponent<Rigidbody2D>();

            if (dropRb != null)
            {
                Vector2 explosionForce = new Vector2(
                    Random.Range(-horizontalDropForce, horizontalDropForce),
                    Random.Range(upwardDropForce * 0.75f, upwardDropForce)
                );

                dropRb.AddForce(explosionForce, ForceMode2D.Impulse);
            }
        }
    }
}

public enum DamageType
{
    Default,
    JumpedOn
}