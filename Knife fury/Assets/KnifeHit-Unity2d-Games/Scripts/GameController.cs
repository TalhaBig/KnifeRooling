using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static int score = 0;
    private static int highScore = 0;
    public static int currentLevel = 1;
    private static int levelsPerScene = 10;

    void Start()
    {
        LoadHighScore();
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
        if (score == highScore)
        {
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public static void LevelCompleted()
    {
        if (currentLevel % levelsPerScene == 0)
        {
            // Check if it's the last scene
            int totalScenes = 10; // Adjust if you have more scenes
            if (SceneManager.GetActiveScene().buildIndex < totalScenes - 1)
            {
                // Move to next scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
    }

    public static void ResetGame()
    {
        // Reset current level and return to the first level of the first scene
        currentLevel = 1;
        SceneManager.LoadScene("Level1");
    }

    public void Play()
    {
        ResetGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}