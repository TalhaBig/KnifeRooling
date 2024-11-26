using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrunkOscillatingRotationController : MonoBehaviour
{
    public float maxSpeed = 1.5f; // The maximum speed the trunk will reach before slowing down
    public float acceleration = 0.1f; // The rate at which the trunk speeds up or slows down
    private float currentSpeed;
    private bool slowingDown = false;
    private int currentLevel;
    private int levelsPerScene = 10;

    void Start()
    {
        // Get the current level from the GameController
        int globalLevel = GameController.GetCurrentLevel();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentLevel = (globalLevel - 1) % levelsPerScene + 1;

        // Calculate the initial speed based on the level
        currentSpeed = maxSpeed + (acceleration * (currentLevel - 1));

        // Start the coroutine to handle speed changes
        StartCoroutine(OscillateSpeed());
    }

    void Update()
    {
        // Rotate the trunk in the Z axis
        transform.Rotate(new Vector3(0, 0, currentSpeed) * Time.deltaTime);
    }

    private IEnumerator OscillateSpeed()
    {
        while (true)
        {
            if (slowingDown)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, acceleration * Time.deltaTime);
                if (Mathf.Approximately(currentSpeed, 0))
                {
                    slowingDown = false;
                    maxSpeed = -maxSpeed; // Reverse the direction of maxSpeed
                }
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
                if (Mathf.Approximately(currentSpeed, maxSpeed))
                {
                    slowingDown = true;
                }
            }

            yield return null;
        }
    }
}