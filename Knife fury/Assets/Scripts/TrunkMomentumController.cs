using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrunkMomentumController : MonoBehaviour
{
    public float initialSpeed = -1.5f;
    public float slowSpeed = -0.5f;
    public float slowDownInterval = 2f;
    public float speedUpInterval = 2f;
    private float currentSpeed;
    private int currentLevel;
    private int levelsPerScene = 10;
    public float speedIncreasePerLevel = -0.5f;

    void Start()
    {
        // Get the current level from the GameController
        int globalLevel = GameController.GetCurrentLevel();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentLevel = (globalLevel - 1) % levelsPerScene + 1;

        // Calculate the current speed based on the level
        currentSpeed = initialSpeed + (speedIncreasePerLevel * (currentLevel - 1));

        // Start the coroutine to handle speed changes
        StartCoroutine(ChangeSpeedOverTime());
    }

    void Update()
    {
        // Rotate the trunk in the Z axis
        if (Time.timeScale > 0)
        {
            transform.Rotate(new Vector3(0, 0, currentSpeed));
        }
    }

    private IEnumerator ChangeSpeedOverTime()
    {
        while (true)
        {
            // Keep the trunk at the initial speed for the specified interval
            yield return new WaitForSeconds(slowDownInterval);

            // Slow down the trunk
            currentSpeed = slowSpeed;

            // Keep the trunk at the slow speed for the specified interval
            yield return new WaitForSeconds(speedUpInterval);

            // Speed up the trunk back to the initial speed
            currentSpeed = initialSpeed + (speedIncreasePerLevel * (currentLevel - 1));
        }
    }
}