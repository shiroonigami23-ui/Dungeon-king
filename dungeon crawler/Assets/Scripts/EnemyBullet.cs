using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 10f;
    public float lifetime = 4f;

    void Start()
    {
        // Don't let bullets fly forever
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move forward (up direction of the bullet)
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(damage);
            
            // Add a small hit sound if you have one
            Destroy(gameObject); 
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Destroy(gameObject); 
        }
    }
}