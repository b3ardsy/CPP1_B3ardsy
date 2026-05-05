using UnityEngine;

public class Life : PickUp 
{
    [SerializeField] private int livesToAdd = 1;
    private Rigidbody2D rb;

    public override void OnPickup(GameObject player)
    {
        player.GetComponent<PlayerController>().lives += livesToAdd;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-4, 4);
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(-2f, rb.linearVelocityY);
    }
}