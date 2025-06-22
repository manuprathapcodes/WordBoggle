using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class controls the Main Scene. Helps the transition from main scene to gamePlay scene. 
/// </summary>
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
