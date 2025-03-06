using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemyBulletPrefab;
    public Transform shootingOffset;
    public float fireRate = 2f;
    
    private float nextFireTime;
    
    void Start()
    {
        //Randomize initial fire time
        nextFireTime = Time.time + Random.Range(0.1f, fireRate);
        
        if (shootingOffset == null)
        {
            shootingOffset = transform;
        }
    }
    
    void Update()
    {
        //Handle shooting
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            
            if (Random.value < 0.3f && enemyBulletPrefab != null)
            {
                Instantiate(enemyBulletPrefab, shootingOffset.position, Quaternion.identity);
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if what hit us is a bullet
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            Destroy(collision.gameObject);
            if (EnemyManager.Instance != null)
            {
                EnemyManager.Instance.EnemyDestroyed(gameObject.tag);
            }
            
            Destroy(gameObject);
        }
    }
}