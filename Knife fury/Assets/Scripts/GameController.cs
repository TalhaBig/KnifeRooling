using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private static int score = 0;
    private static int highScore = 0;
    public static int currentLevel = 1;
    private static int levelsPerScene = 10;
    private const string HighScoreKey = "HighScore";
    private const string CurrentLevelKey = "CurrentLevel";
    private const string CurrentSceneKey = "CurrentScene";
    private const string UnlockedLevelsKey = "UnlockedLevels";

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeUnlockedLevels();
        LoadProgress();
    }

    private void InitializeUnlockedLevels()
    {
        if (!PlayerPrefs.HasKey(UnlockedLevelsKey))
        {
            // Unlock only level 1 of stage 1 initially
            PlayerPrefs.SetString(UnlockedLevelsKey, "1");
            PlayerPrefs.Save();
        }
    }

    public static void SetScore(int newScore)
    {
        score += newScore;
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore(); // Save high score immediately when updated
        }
    }

    public static void ResetScore()
    {
        score = 0;
    }

    public static int GetScore()
    {
        return score;
    }

    public static int GetHighScore()
    {
        return highScore;
    }

    public static void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.Save();
    }

    public static void SaveProgress()
    {
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.SetInt(CurrentLevelKey, currentLevel);
        PlayerPrefs.SetInt(CurrentSceneKey, SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        currentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1);
        Debug.Log($"Loaded Progress: HighScore = {highScore}, CurrentLevel = {currentLevel}");
    }

    public static void SetLevel(int level, int stage)
    {
        currentLevel = (stage - 1) * levelsPerScene + level;
        PlayerPrefs.SetInt(CurrentLevelKey, currentLevel);
        PlayerPrefs.SetInt(CurrentSceneKey, stage);
        PlayerPrefs.Save();
    }

    public void LoadScene()
    {
        int savedSceneIndex = PlayerPrefs.GetInt(CurrentSceneKey, 1);
        SceneManager.LoadScene(savedSceneIndex);
        Debug.Log($"Loaded Scene: Index = {savedSceneIndex}");
    }

    public static void LevelCompleted()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Unlock the next level
        UnlockNextLevel();

        if (currentLevel % levelsPerScene == 0)
        {
            int totalScenes = 7;
            if (currentSceneIndex < totalScenes - 1)
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
            else
            {
                SceneManager.LoadScene("VictoryScene");
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        currentLevel++;
        SaveProgress();
    }

    private static void UnlockNextLevel()
    {
        List<int> unlockedLevels = GetUnlockedLevels();
        int nextLevel = currentLevel + 1;
        int totalLevels = levelsPerScene * 7; // 7 stages each with 10 levels

        if (nextLevel <= totalLevels)
        {
            if (!unlockedLevels.Contains(nextLevel))
            {
                unlockedLevels.Add(nextLevel);
                PlayerPrefs.SetString(UnlockedLevelsKey, string.Join(",", unlockedLevels));
                PlayerPrefs.Save();
            }
        }
    }

    private static List<int> GetUnlockedLevels()
    {
        string unlockedLevelsString = PlayerPrefs.GetString(UnlockedLevelsKey, "1");
        string[] unlockedLevelsArray = unlockedLevelsString.Split(',');
        List<int> unlockedLevels = new List<int>();

        foreach (string level in unlockedLevelsArray)
        {
            if (int.TryParse(level, out int levelNumber))
                unlockedLevels.Add(levelNumber);
        }

        return unlockedLevels;
    }

    public static bool IsLevelUnlocked(int level)
    {
        List<int> unlockedLevels = GetUnlockedLevels();
        return unlockedLevels.Contains(level);
    }

    public static int GetCurrentLevel()
    {
        return currentLevel;
    }

    public static void ResetGame()
    {
        int currentSceneIndex = PlayerPrefs.GetInt(CurrentSceneKey, 1);
        currentLevel = (levelsPerScene * (currentSceneIndex - 1)) + 1;
        Debug.Log("currentLevel = " + currentLevel);
        Debug.Log("currentScene = " + currentSceneIndex);

        PlayerPrefs.SetInt(CurrentLevelKey, currentLevel);
        PlayerPrefs.SetInt(CurrentSceneKey, currentSceneIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(currentSceneIndex);
    }

    public void Play()
    {
        LoadScene(); // Load the last played scene
        LoadProgress(); // Load the progress, including the current level and high score
    }

    public void Quit()
    {
        SaveProgress(); // Save progress when quitting
        Application.Quit();
    }

    public void LoadLevelsScene()
    {
        SceneManager.LoadScene("LevelMenu"); // Replace "LevelMenu" with the name of your levels scene
    }

public void BackToMainMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }
}