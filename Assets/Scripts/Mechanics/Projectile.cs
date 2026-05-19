using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0.05f, 10f)] private float lifetime = 10f;

    [Header("Damage")]
    [SerializeField] private int damage = 1;
    [SerializeField] private DamageType damageType = DamageType.Default;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetVelocity(Vector2 velocity)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = velocity;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.flipX = velocity.x < 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;

        BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage, damageType);
        }

        Destroy(gameObject);
    }
}

public enum ProjectileType
{
    PlayerProjectile,
    EnemyProjectile
}