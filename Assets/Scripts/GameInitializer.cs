// GameInitializer.cs
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        string mode = PlayerPrefs.GetString("GameMode");
        if (mode == "Endless")
        {
            GameManager.Instance.StartEndless();
        }
        else if (mode == "Level")
        {
            string json = PlayerPrefs.GetString("LevelData");
            Debug.Log($"LevelData json : {json}");

            LevelDataList list = JsonUtility.FromJson<LevelDataList>(json);
            LevelData ld = list.data[0];

            GameManager.Instance.StartLevel(ld);
        }
    }
}
