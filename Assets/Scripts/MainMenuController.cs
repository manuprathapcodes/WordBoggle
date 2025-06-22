using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayEndless()
    {
        PlayerPrefs.SetString("GameMode", "Endless");
        SceneManager.LoadScene("GamePlay");
    }

    public void PlayLevel()
    {
        PlayerPrefs.SetString("GameMode", "Level");
        SceneManager.LoadScene("GamePlay");
    }
}
