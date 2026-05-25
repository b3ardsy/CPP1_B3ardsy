using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private SpriteRenderer sr;
    private BaseEnemy enemy;
    private Animator anim;

    [SerializeField] private Vector2 initialShotVelocity = new Vector2(8f, 0f);
    [SerializeField] private float attackCooldown = 2f;

    [Header("Spawn Points")]
    [SerializeField] private Transform spawnPointLeft;
    [SerializeField] private Transform spawnPointRight;

    [Header("Projectile Prefab")]
    [SerializeField] private EnemyProjectile fireballPrefab;

    private float attackTimer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        enemy = GetComponent<BaseEnemy>();
        anim = GetComponent<Animator>();

        attackTimer = attackCooldown;
    }

    void Update()
    {
        if (enemy != null && enemy.IsDead)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            anim.SetTrigger("Fire");
            attackTimer = attackCooldown;
        }
    }

    // Called by animation event
    public void Fire()
    {
        Debug.Log("Mage Fire() animation event was called!");

        if (fireballPrefab == null)
            return;

        Transform spawnPoint = sr.flipX ? spawnPointRight : spawnPointLeft;
        float direction = sr.flipX ? 1f : -1f;

        EnemyProjectile curProjectile = Instantiate(
            fireballPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        curProjectile.SetVelocity(initialShotVelocity * direction);
    }
}