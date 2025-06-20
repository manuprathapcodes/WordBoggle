using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GridManager gridMgr;
    public DictionaryManager dictMgr;
    public TextMeshProUGUI scoreTMP, avgTMP, objectiveTMP, timerTMP;
    int scoreTotal, wordCount;
    float avgScore => wordCount == 0 ? 0 : scoreTotal / (float)wordCount;

    bool isEndless;
    LevelData levelData;
    float timeLeft;

    void Awake()
    {
        Instance = this;
    }

    public void StartEndless()
    {
        Debug.LogWarning("Starting Endless");
        isEndless = true;
        scoreTotal = wordCount = 0;
        gridMgr.GenerateGrid(true);
        UpdateUI();
    }

    public void StartLevel(LevelData ld)
    {
        Debug.LogWarning($"Starting Level: {ld.wordCount}");
        isEndless = false;
        levelData = ld;
        scoreTotal = ld.totalScore;
        wordCount = 0;
        timeLeft = ld.timeSec;
        gridMgr.width = ld.gridSize.x;
        gridMgr.height = ld.gridSize.y;
        gridMgr.GenerateGrid(false, ld);
        UpdateUI();
    }

    void Update()
    {
        if (!isEndless && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerTMP.text = $"Time: {timeLeft:0}";
            if (timeLeft <= 0) EndLevel(false);
        }
    }

    public void OnWordComplete(List<Tile> path)
    {
        string w = "";
        foreach (var t in path) w += t.letter;
        if (!dictMgr.IsValid(w)) return;

        int pts = w.Length * 10;
        scoreTotal += pts;
        wordCount++;
        if (isEndless)
        {
            gridMgr.RemoveTiles(path);
        }
        else
        {
            foreach (var t in path)
            {
                if (t.tileType == TileType.Bug) levelData.bugCount--;
            }
        }

        UpdateUI();
        CheckLevelCompletion();
    }

    void UpdateUI()
    {
        scoreTMP.text = $"Score: {scoreTotal}";
        avgTMP.text = $"Avg: {avgScore:0.##}";
        if (!isEndless)
        {
            objectiveTMP.text = $"Words: {wordCount}/{levelData.wordCount}, Bugs Left: {levelData.bugCount}";
        }
    }

    void CheckLevelCompletion()
    {
        if (!isEndless && wordCount >= levelData.wordCount &&
            (levelData.timeSec == 0 || timeLeft > 0)) EndLevel(true);
    }

    void EndLevel(bool won)
    {
        objectiveTMP.text = won ? "You Win!" : "Time's Up!";
    }
}