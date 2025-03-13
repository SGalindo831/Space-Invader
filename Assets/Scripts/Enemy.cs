using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public GameObject enemyBulletPrefab;
    public Transform shootingOffset;
    public float fireRate = 2f;
    
    public string deathTriggerName = "enemyDeath";
    public float deathAnimationDuration = 1.0f;
    
    //Audio variables
    public AudioClip shootSound;
    public AudioClip deathSound;
    
    //Volume controls
    [Range(0f, 1f)]
    public float shootVolume = 0.3f;
    
    [Range(0f, 1f)]
    public float deathVolume = 0.4f;
    
    private static AudioClip sharedDeathSound;
    private static float sharedDeathVolume = 0.4f;
    private static bool volumeInitialized = false;
    
    private AudioSource audioSource;
    private float nextFireTime;
    private Animator animator;
    private bool isDying = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        nextFireTime = Time.time + Random.Range(0.1f, fireRate);
        
        if (shootingOffset == null)
        {
            shootingOffset = transform;
        }
        
        //Get AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Set up the shared death sound and volume
        if (sharedDeathSound == null && deathSound != null)
        {
            //If this enemy has a death sound, share it with others
            sharedDeathSound = deathSound;
            sharedDeathVolume = deathVolume;
            volumeInitialized = true;
            Debug.Log($"Enemy {gameObject.tag} is sharing death sound with volume {deathVolume}");
        }
        else if (deathSound == null && sharedDeathSound != null)
        {
            deathSound = sharedDeathSound;
            
            if (volumeInitialized)
            {
                deathVolume = sharedDeathVolume;
            }
        }
    }
    
    void OnValidate()
    {
        if (deathSound != null && deathSound == sharedDeathSound)
        {
            sharedDeathVolume = deathVolume;
            Debug.Log($"Updated shared death volume to {deathVolume}");
        }
    }
    
    void Update()
    {
        if (isDying)
            return;
            
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            
            if (Random.value < 0.3f && enemyBulletPrefab != null)
            {
                Instantiate(enemyBulletPrefab, shootingOffset.position, Quaternion.identity);
                
                if (shootSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(shootSound, shootVolume);
                }
            }
        }
    
        if (deathSound != null && deathSound == sharedDeathSound && sharedDeathVolume != deathVolume)
        {
            sharedDeathVolume = deathVolume;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null && !isDying)
        {
            Destroy(collision.gameObject);
            Die();
        }
    }
    
    void Die()
    {
        if (isDying)
            return;
            
        isDying = true;
        
        if (deathSound != null)
        {
            GameObject soundObject = new GameObject("DeathSound");
            AudioSource soundSource = soundObject.AddComponent<AudioSource>();
            soundSource.clip = deathSound;
            
            if (deathSound == sharedDeathSound)
            {
                soundSource.volume = sharedDeathVolume;
            }
            else
            {
                soundSource.volume = deathVolume;
            }
            
            soundSource.Play();
            Destroy(soundObject, deathSound.length + 0.1f);
        }
        
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.EnemyDestroyed(gameObject.tag);
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        if (animator != null && HasParameter(animator, deathTriggerName))
        {
            animator.SetTrigger(deathTriggerName);
            StartCoroutine(DestroyAfterDelay(deathAnimationDuration));
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
            {
                return true;
            }
        }
        return false;
    }
    
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}