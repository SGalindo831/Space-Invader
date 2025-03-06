using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  private Rigidbody2D myRigidbody2D;

  public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
      myRigidbody2D = GetComponent<Rigidbody2D>();
      Fire();
    }

    // Update is called once per frame
    private void Fire()
    {
      myRigidbody2D.linearVelocity = Vector2.up * speed; 
      Debug.Log("Wwweeeeee");
    }

       void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet hit a barricade
        if (collision.gameObject.CompareTag("Barricade"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            
            Debug.Log("Bullet destroyed barricade!");
        }
    }
}
