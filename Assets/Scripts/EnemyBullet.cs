using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 3f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) 
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            //Get the Player component and call Die()
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Die();
            }
            
            // Destroy the bullet
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Barricade"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
