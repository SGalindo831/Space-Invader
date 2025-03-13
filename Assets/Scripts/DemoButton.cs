using UnityEngine;
using UnityEngine.SceneManagement;
public class DemoButton : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("DemoScene");
    }
}
