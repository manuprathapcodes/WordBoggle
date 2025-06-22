using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// The manager class which orchestrates the game. 
/// </summary>
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

    // Initial point of execution for ENDLESS mode. 
    public void StartEndless()
    {
        isEndless = true;
        scoreTotal = wordCount = 0;
        gridMgr.GenerateGrid(true);
        UpdateUI();
    }

    // Initial point of execution for LEVELS mode. 
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
        // Time LOGIC.
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

    // Method to check if the tile path the user has chosen is a valid word or not. 
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

    // Updates the TMP values on the screen as the game progresses. 
    void UpdateUI()
    {
        scoreTMP.text = $"Score: {scoreTotal}";
        avgTMP.text = $"Avg: {avgScore:0.##}";
        if (!isEndless)
        {
            objectiveTMP.text = $"Words: {wordCount}/{levelData.wordCount}, Bugs Left: {levelData.bugCount}";
        }
    }

    // Method to check if the level is completed or not based on word count, time. 
    void CheckLevelCompletion()
    {
        if (!isEndless && wordCount >= levelData.wordCount &&
            (levelData.timeSec == 0 || timeLeft > 0)) EndLevel(true);
    }

    // Method to see various options after a level ends. 
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

    // Loads the next level. 
    void LoadNextLevel()
    {
        LevelLoader.Instance.ProceedToNextLevel();
    }

    // Restarts the LEVEL game from start. 
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}