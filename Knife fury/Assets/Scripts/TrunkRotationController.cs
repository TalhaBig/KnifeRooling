using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrunkRotationController : MonoBehaviour
{
    public float initialSpeed = -1.5f;
    public float speedIncreasePerLevel = -0.5f;
    private float currentSpeed;
    private int currentLevel;
    private int levelsPerScene = 10;

    void Start()
    {
        // Get the current level from the GameController
        int globalLevel = GameController.GetCurrentLevel();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentLevel = (globalLevel - 1) % levelsPerScene + 1;

        // Calculate the current speed based on the level
        currentSpeed = initialSpeed + (speedIncreasePerLevel * (currentLevel - 1));
    }

    void Update()
    {
        // Rotate Trunk in Z axis
        if (Time.timeScale > 0)
        {
            transform.Rotate(new Vector3(0, 0, currentSpeed));
        }

    }
}