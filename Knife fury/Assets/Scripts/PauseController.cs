using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel; // Assign this in the Inspector
    private bool isPaused = false;

    void Start()
    {
        // Ensure the pause panel is initially hidden
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // You can use another key or button for pausing
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0; // Stop all game time
        isPaused = true;
    }

    private void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1; // Resume game time
        isPaused = false;
    }

    public void OnPauseButtonPressed()
    {
        TogglePause();
    }
}
