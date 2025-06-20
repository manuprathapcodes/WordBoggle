using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevelJson(TextAsset ta)
    {
        var ld = JsonUtility.FromJson<LevelData>(ta.text);
        GameManager.Instance.StartLevel(ld);
    }
}