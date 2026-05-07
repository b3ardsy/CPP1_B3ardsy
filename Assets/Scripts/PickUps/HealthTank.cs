using UnityEngine;

public class HealthTank : PickUp
{
    [SerializeField] private int healthTank = 1;

    [Header("Hover Settings")]
    [SerializeField] private float hoverHeight = 0.25f;

    [SerializeField] private float hoverSpeed = 2f;

    [Header("Flip Settings")]
    [SerializeField] private float flipSpeed = 2f;

    private Vector3 startPosition;
    private Vector3 startScale;

    public override void OnPickup(GameObject player)
    {
        player.GetComponent<PlayerController>().healthTank += healthTank;
    }

    void Start()
    {
        startPosition = transform.position;
        startScale = transform.localScale;
    }

    void Update()
    {
        Hover();
        FlipPickup();
    }

    private void Hover()
    {
        float yOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        transform.position = new Vector3(
            startPosition.x,
            startPosition.y + yOffset,
            startPosition.z
        );
    }

    private void FlipPickup()
    {
        float scaleX = Mathf.Sin(Time.time * flipSpeed);

        transform.localScale = new Vector3(
            scaleX * startScale.x,
            startScale.y,
            startScale.z
        );
    }
}