using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public float initialTime = 60f; // Initial time allowed for the first level
    public float timeDecreasePerLevel = 5f; // Time decrease per level
    private float timeLeft;
    public Text timerText; // UI Text to display the timer
    private bool levelCompleted = false;
    private int currentLevel;
    private int levelsPerScene = 10;

    void Start()
    {
        // Initialize the timer based on the current level and scene
        int globalLevel = GameController.GetCurrentLevel();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentLevel = (globalLevel - 1) % levelsPerScene + 1;

        timeLeft = Mathf.Max(initialTime - (currentLevel - 1) * timeDecreasePerLevel, 5f); // Minimum of 5 seconds
        StartCoroutine(TimerCountdown());
    }

    void Update()
    {
        if (levelCompleted) return;

        // Update timer display
        timerText.text = $"Time Left: {Mathf.Ceil(timeLeft)}";
    }

    private IEnumerator TimerCountdown()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        // Time has run out
        HandleGameOver();
    }

    public void CompleteLevel()
    {
        // Called when the level is successfully completed
        levelCompleted = true;
    }

    private void HandleGameOver()
    {
        // Handle game over
        GameObject.Find("TextMessage").GetComponent<Text>().text = "GAME OVER";
        GameController.SaveHighScore();
        GameController.ResetScore();
        StartCoroutine(RestartLevel());
    }

    private IEnumerator RestartLevel()
    {
        // Wait for 2 seconds before restarting the game
        yield return new WaitForSeconds(2f);

        // Get the current scene and level
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int currentLevel = GameController.GetCurrentLevel();

        // Reload the scene
        SceneManager.LoadScene(currentSceneIndex);

        // Ensure the level is reset to the correct one
        GameController.SetCurrentLevel(currentLevel);
    }
}
