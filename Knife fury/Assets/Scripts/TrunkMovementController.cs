using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrunkMovementController : MonoBehaviour
{
    public float initialSpeed = 2f; // Initial horizontal speed
    public float speedIncreasePerLevel = 0.5f; // Speed increase per level
    private float currentSpeed;
    private int currentLevel;
    private int levelsPerScene = 10; // Number of levels per scene
    private bool movingRight = true;
    public float horizontalRange = 5f; // The range within which the trunk moves left and right

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
        // Move the trunk left and right
        if (movingRight)
        {
            transform.position += (Vector3.right * currentSpeed * Time.deltaTime);
            if (transform.position.x >= horizontalRange)
                movingRight = false;
        }
        else
        {
            transform.position += (Vector3.left * currentSpeed * Time.deltaTime);
            if (transform.position.x <= -horizontalRange)
                movingRight = true;
        }
    }
}
