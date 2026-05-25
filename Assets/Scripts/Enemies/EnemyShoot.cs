using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private SpriteRenderer sr;
    private BaseEnemy enemy;
    private Animator anim;
    private Transform player;

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

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        attackTimer = attackCooldown;
    }

    void Update()
    {
        if (enemy != null && enemy.IsDead)
            return;

        FacePlayer();

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            anim.SetTrigger("Fire");
            attackTimer = attackCooldown;
        }
    }

    private void FacePlayer()
    {
        if (player == null)
            return;

        // Mage faces right when player is to the right
        sr.flipX = player.position.x > transform.position.x;
    }

    // Called by the animation event named Fire
    public void Fire()
    {
        if (fireballPrefab == null)
            return;

        Transform spawnPoint = sr.flipX ? spawnPointRight : spawnPointLeft;
        float direction = sr.flipX ? 1f : -1f;

        if (spawnPoint == null)
            return;

        EnemyProjectile curProjectile = Instantiate(
            fireballPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        curProjectile.SetVelocity(initialShotVelocity * direction);
    }
}