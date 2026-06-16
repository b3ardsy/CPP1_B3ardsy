using UnityEngine;

public class Shoot : MonoBehaviour
{
    private SpriteRenderer sr;
    private PlayerController playerController;
    private PlayerStats playerStats;

    [SerializeField] private Vector2 initialShotVelocity = new Vector2(15, 0);

    [Header("Spawn Points")]
    [SerializeField] private Transform spawnPointLeft;
    [SerializeField] private Transform spawnPointRight;

    [Header("Projectile Prefabs")]
    [SerializeField] private Projectile basicArrowPrefab;
    [SerializeField] private Projectile fireArrowPrefab;
    [SerializeField] private Projectile iceArrowPrefab;

    [Header("Special Ammo Costs")]
    [SerializeField] private int fireArrowAmmoCost = 1;
    [SerializeField] private int iceArrowAmmoCost = 1;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        playerStats = GetComponent<PlayerStats>();

        if (initialShotVelocity == Vector2.zero)
        {
            initialShotVelocity = new Vector2(15, 0);
            Debug.LogWarning("Shoot: Initial shot velocity was not set, defaulting to (15, 0)");
        }

        if (spawnPointLeft == null || spawnPointRight == null)
        {
            Debug.LogError("Shoot: Spawn points must be assigned in the inspector on " + gameObject.name);
        }

        if (basicArrowPrefab == null)
        {
            Debug.LogError("Shoot: Basic Arrow prefab must be assigned in the inspector on " + gameObject.name);
        }

        if (fireArrowPrefab == null)
        {
            Debug.LogWarning("Shoot: Fire Arrow prefab is not assigned yet.");
        }

        if (iceArrowPrefab == null)
        {
            Debug.LogWarning("Shoot: Ice Arrow prefab is not assigned yet.");
        }

        if (playerController == null)
        {
            Debug.LogError("Shoot: PlayerController was not found on " + gameObject.name);
        }

        if (playerStats == null)
        {
            Debug.LogError("Shoot: PlayerStats was not found on " + gameObject.name);
        }
    }

    public void Fire()
    {
        Projectile projectileToShoot = GetProjectileToShoot();

        if (projectileToShoot == null)
            return;

        float direction = sr.flipX ? -1f : 1f;
        Transform spawnPoint = sr.flipX ? spawnPointLeft : spawnPointRight;

        Projectile curProjectile = Instantiate(
            projectileToShoot,
            spawnPoint.position,
            Quaternion.identity
        );

        curProjectile.SetVelocity(initialShotVelocity * direction);
    }

    private Projectile GetProjectileToShoot()
    {
        bool wantsSpecialArrow = Input.GetMouseButton(1);

        if (!wantsSpecialArrow || playerStats == null)
            return basicArrowPrefab;

        if (playerStats.EquippedSpecialArrow == SpecialArrowType.Fire)
        {
            if (fireArrowPrefab != null && playerStats.TrySpendSpecialAmmo(fireArrowAmmoCost))
                return fireArrowPrefab;
        }

        if (playerStats.EquippedSpecialArrow == SpecialArrowType.Ice)
        {
            if (iceArrowPrefab != null && playerStats.TrySpendSpecialAmmo(iceArrowAmmoCost))
                return iceArrowPrefab;
        }

        return basicArrowPrefab;
    }
}