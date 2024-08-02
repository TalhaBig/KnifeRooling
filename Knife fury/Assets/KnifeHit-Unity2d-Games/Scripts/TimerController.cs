using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public float timeLimit = 30f; // Set the time limit to 30 seconds
    private float timeRemaining;
    public TMP_Text timeText;
    public bool timeIsRunning = false;

    private void Start()
    {
        timeRemaining = timeLimit; // Initialize TimeRemaining with the time limit
        timeIsRunning = true;
        timeText.text = "Timer Initialized"; // Debug text
    }

    private void Update()
    {
        if (timeIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Countdown the timer
                DisplayTime(timeRemaining);
                Debug.Log("Time Remaining: " + timeRemaining); // Debug output
            }
            else
            {
                timeRemaining = 0; // Ensure the timer does not go negative
                timeIsRunning = false;
                DisplayTime(timeRemaining);
                RestartGame(); // Restart the game when time is up
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void RestartGame()
    {
        // Restart the game by reloading the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
