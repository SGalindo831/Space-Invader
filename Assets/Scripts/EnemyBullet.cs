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
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Barricade"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}