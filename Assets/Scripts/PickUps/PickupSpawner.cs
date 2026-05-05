using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Pickup Prefabs")]
    [SerializeField] private GameObject lifePrefab;
    [SerializeField] private GameObject specialAmmoPrefab;
    [SerializeField] private GameObject fireArrowPrefab;
    [SerializeField] private GameObject iceArrowPrefab;
    [SerializeField] private GameObject rollPrefab;
    [SerializeField] private GameObject bountyTokenPrefab;

    [Header("Spawn Boundaries")]
    [SerializeField] private float minX = -17f;
    [SerializeField] private float maxX = 19f;
    [SerializeField] private float minY = -7f;
    [SerializeField] private float maxY = 10f;

    void Start()
    {
        SpawnPickup(lifePrefab);
        SpawnPickup(specialAmmoPrefab);
        SpawnPickup(fireArrowPrefab);
        SpawnPickup(iceArrowPrefab);
        SpawnPickup(rollPrefab);
        SpawnPickup(bountyTokenPrefab);
    }

    private void SpawnPickup(GameObject pickupPrefab)
    {
        if (pickupPrefab == null)
            return;

        Vector2 spawnPosition = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );

        Instantiate(pickupPrefab, spawnPosition, Quaternion.identity);
    }
}