using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Projectile : MonoBehaviour
{

    [SerializeField, Range(0.05f, 10f)] private float lifetime = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetVelocity(Vector2 velocity)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = velocity;

        // Flip sprite based on direction
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (velocity.x < 0)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    //add collision detectioin functions for the projectile to interact with the environemnt and other objects, such as damaging enemies or being destroyed 
    //on impact with walls
}
