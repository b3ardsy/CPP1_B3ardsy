using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private SpriteRenderer sr;
    private BaseEnemy enemy;
    private Animator anim;
    private Transform player;

    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private float playerAimHeightOffset = 0.8f;

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

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        attackTimer = attackCooldown;
    }

    void Update()
    {
        if (enemy != null && enemy.IsDead)
            return;

        if (player == null)
            return;

        FacePlayer();

        if (!PlayerInRange())
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            anim.SetTrigger("Fire");
            attackTimer = attackCooldown;
        }
    }

    private bool PlayerInRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer <= attackRange;
    }

    private void FacePlayer()
    {
        sr.flipX = player.position.x > transform.position.x;
    }

    // Called by the animation event named Fire
    public void Fire()
    {
        if (fireballPrefab == null || player == null)
            return;

        Transform spawnPoint = sr.flipX ? spawnPointRight : spawnPointLeft;

        if (spawnPoint == null)
            return;

        Vector2 targetPosition = player.position;
        targetPosition.y += playerAimHeightOffset;

        Vector2 shootDirection = (targetPosition - (Vector2)spawnPoint.position).normalized;

        EnemyProjectile curProjectile = Instantiate(
            fireballPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        curProjectile.SetVelocity(shootDirection * projectileSpeed);

        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayEnemyFireball();
        }
    }
}