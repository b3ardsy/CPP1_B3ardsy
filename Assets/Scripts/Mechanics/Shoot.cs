using UnityEngine;

public class Shoot : MonoBehaviour
{

    private SpriteRenderer sr;

    [SerializeField] private Vector2 initialShotVelocity = new Vector2(3, 3);
    [SerializeField] private Transform spawnPointLeft;
    [SerializeField] private Transform spawnPointRight;
    [SerializeField] private Projectile projectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (initialShotVelocity == Vector2.zero)
        {
            initialShotVelocity = new Vector2(3, 3);
            Debug.LogWarning("Shoot: Initial shot velocity was not set, defaulting to (3,3)");
        }

        if (spawnPointLeft == null || spawnPointRight == null || projectilePrefab == null)
        {
            Debug.LogError("Shoot: Spawn points and projectile prefab must be assigned in the inspector on " + gameObject.name);
        }
    }

    public void Fire()
    {
        Projectile curProjectile;

        float direction = sr.flipX ? -1f : 1f;

        if (!sr.flipX)
        {
            curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, Quaternion.identity);
        }
        else
        {
            curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, Quaternion.identity);
        }

        curProjectile.SetVelocity(initialShotVelocity * direction);
    }
}
