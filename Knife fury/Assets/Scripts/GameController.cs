using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static int score = 0;
    private static int highScore = 0;
    public static int currentLevel = 1;
    private static int levelsPerScene = 10;

    private const string HighScoreKey = "HighScore";
    private const string CurrentLevelKey = "CurrentLevel";
    private const string CurrentSceneKey = "CurrentScene";

    void Start()
    {
        LoadProgress();
       
    }

    public static void SetScore(int newScore)
    {
        score += newScore;
        if (score > highScore)
            highScore = score;
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
        if (score > highScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, highScore);
        }
    }

    public static void SaveProgress()
    {
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.SetInt(CurrentLevelKey, currentLevel);
        PlayerPrefs.SetInt(CurrentSceneKey, SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
    }

    void LoadProgress()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        currentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1);
        Debug.Log($"Loaded Progress: HighScore = {highScore}, CurrentLevel = {currentLevel}");
    }

   public void LoadScene()
    {
        int savedSceneIndex = PlayerPrefs.GetInt(CurrentSceneKey, 1);
       
        SceneManager.LoadScene(savedSceneIndex);
        Debug.Log($"Loaded Scene: Index = {savedSceneIndex}");
    }

    public static void LevelCompleted()
    {
        // Move to the next level within the current scene
        if (currentLevel % levelsPerScene == 0)
        {
            // Check if it's the last scene
            int totalScenes = 10; // Adjust if you have more scenes
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex < totalScenes - 1)
            {
                // Move to next scene
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
            else
            {
                // All levels completed
                SceneManager.LoadScene("VictoryScene"); // Load a victory scene or show final message
            }
        }
        else
        {
            // Move to the next level within the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        currentLevel++;
        SaveProgress(); // Save progress after completing a level
    }

    public static int GetCurrentLevel()
    {
        return currentLevel;
    }
    public static void ResetGame()
    {
        // Reset current level to 1
        

        // Get the index of the current scene
        int currentSceneIndex = PlayerPrefs.GetInt(CurrentSceneKey, 1);
        currentLevel = (10 * (currentSceneIndex - 1)) + 1 ;
        Debug.Log("currectleve =" + currentLevel);
        Debug.Log("current scene =" +currentSceneIndex);

        // Save the current scene index so that it loads correctly
        PlayerPrefs.SetInt(CurrentLevelKey, currentLevel);
        PlayerPrefs.SetInt(CurrentSceneKey, currentSceneIndex);
        PlayerPrefs.Save();

        // Load the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }



    public void Play()
    {
        LoadScene();
        LoadProgress();
    }

    public void Quit()
    {
        SaveProgress(); // Save progress when quitting
        Application.Quit();
    }
}
