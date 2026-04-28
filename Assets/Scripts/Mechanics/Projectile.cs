using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0.05f, 10f)] private float lifetime = 10f;

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
        //this ensures the projectile doesn't destroy within the playercollider
        if (collision.gameObject.CompareTag("Player"))
            return;
        //destroys projectile after colliding with anything else
        Destroy(gameObject);
    }
}