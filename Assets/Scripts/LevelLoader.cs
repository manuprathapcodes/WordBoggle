using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for managing LEVEL mode games. Orchestrates the level count. 
/// </summary>
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public LevelDataList levelList;
    public int currentLevelIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void LoadLevelsFromJSON(string json)
    {
        levelList = JsonUtility.FromJson<LevelDataList>(json);
        currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 0);
    }

    public LevelData GetCurrentLevel() => levelList.data[currentLevelIndex];

    public bool HasMoreLevels() => currentLevelIndex < levelList.data.Count - 1;

    public void ProceedToNextLevel()
    {
        currentLevelIndex++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GamePlay"); 
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void ShowGameCompletedScreen()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("CurrentLevel", 0);
    }
}