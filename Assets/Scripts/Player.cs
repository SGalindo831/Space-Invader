using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
  [FormerlySerializedAs("bullet")]
  public GameObject bulletPrefab;
  public Transform shottingOffset;
  
  //Audio variables
  public AudioClip shootSound;
  public AudioClip deathSound;
  
  //Volume controls
  [Range(0f, 1f)]
  public float shootVolume = 0.3f;
  
  [Range(0f, 1f)]
  public float deathVolume = 0.4f;
  
  private AudioSource audioSource;
  private Animator animator;
  private bool isDead = false;
  
  void Start()
  {
    //Get the animator component
    animator = GetComponent<Animator>();
    animator.ResetTrigger("isDead");
    
    audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
    {
      audioSource = gameObject.AddComponent<AudioSource>();
    }
  }
  
  void Update()
  {
    if (isDead)
      return;
      
    if (Input.GetKeyDown(KeyCode.Space))
    {
      GameObject shot = Instantiate(bulletPrefab, shottingOffset.position, Quaternion.identity);

      if (shootSound != null && audioSource != null)
      {
        audioSource.PlayOneShot(shootSound, shootVolume);
      }
      
      Debug.Log("Bang!");
    }
  }
  
  public void Die()
  {
    if (isDead)
      return;
      
    isDead = true;
    
    //Play death sound
    if (deathSound != null && audioSource != null)
    {
      audioSource.PlayOneShot(deathSound, deathVolume);
      Debug.Log("Playing player death sound");
    }
    
    //Trigger death animation
    if (animator != null)
    {
      animator.SetTrigger("isDead");
      Debug.Log("Set isDead trigger");
      
      StartCoroutine(LoadCreditsAfterDelay(1.5f));
    }
    else
    {
      SceneManager.LoadScene("DemoCredits");
    }
  }
  
  IEnumerator LoadCreditsAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);
    SceneManager.LoadScene("DemoCredits");
  }
}