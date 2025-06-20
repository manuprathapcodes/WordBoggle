using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public TextAsset levelDataJSON;

    public void PlayEndless()
    {
        PlayerPrefs.SetString("GameMode", "Endless");
        SceneManager.LoadScene("GamePlay");
    }

    public void PlayLevel()
    {
        PlayerPrefs.SetString("GameMode", "Level");
        PlayerPrefs.SetString("LevelData", levelDataJSON.text);
        SceneManager.LoadScene("GamePlay");
    }
}
