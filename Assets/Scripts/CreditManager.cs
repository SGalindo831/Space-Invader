using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    public float displayTime = 5f;
    
    void Start()
    {
        // Start the timer to return to the menu
        StartCoroutine(ReturnToMenu());
    }
    
    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(displayTime);
        SceneManager.LoadScene("DemoMenu");
    }
}