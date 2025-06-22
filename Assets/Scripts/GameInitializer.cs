using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public TextAsset levelDataJSON;

    void Start()
    {
        string mode = PlayerPrefs.GetString("GameMode");
        if (mode == "Endless")
        {
            GameManager.Instance.StartEndless();
        }
        else if (mode == "Level")
        {
            LevelLoader.Instance.LoadLevelsFromJSON(levelDataJSON.text);
            GameManager.Instance.StartLevel(LevelLoader.Instance.GetCurrentLevel());

        }
    }
}
