using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GridManager gridMgr;
    public DictionaryManager dictMgr;
    public TextMeshProUGUI scoreTMP, avgTMP, objectiveTMP, timerTMP;
    public GameObject restartButton;
    public InputController inputController;
    private bool gameOver = false;
    int scoreTotal, wordCount;
    float avgScore => wordCount == 0 ? 0 : scoreTotal / (float)wordCount;
    private HashSet<string> usedWords = new HashSet<string>();

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
        if (timeLeft == 0)
            timerTMP.gameObject.SetActive(false);

        if (!isEndless && !gameOver && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerTMP.text = $"Time: {timeLeft:0}";

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                EndLevel(false);
            }
        }
    }

    public void OnWordComplete(List<Tile> path)
    {
        string w = "";
        foreach (var t in path) w += t.letter;
        w = w.ToUpper(); 

        if (!dictMgr.IsValid(w))
        {
            Debug.Log($"Invalid word: {w}");
            return;
        }

        if (usedWords.Contains(w))
        {
            Debug.Log($"Word already used: {w}");
            return;
        }

        usedWords.Add(w);

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
        gameOver = true;
        timeLeft = 0;
        timerTMP.text = "Time: 0";

        inputController.enabled = false;

        objectiveTMP.text = won ? "You Win!" : "Time's Up!";

        if (!won)
        {
            if (restartButton != null)
            {
                gridMgr.gameObject.SetActive(false);
                restartButton.SetActive(true);
            }
        }
        else
        {
            if (LevelLoader.Instance.HasMoreLevels())
                Invoke(nameof(LoadNextLevel), 2f);
            else
                LevelLoader.Instance.ShowGameCompletedScreen();
        }
    }

    void LoadNextLevel()
    {
        LevelLoader.Instance.ProceedToNextLevel();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}