using UnityEngine;

public class TrunkSpeedController : MonoBehaviour
{
    public float baseSpeed = -1.5f; // Base speed for level 1
    public float maxSpeed = -5.0f; // Maximum speed for level 10
    private float currentSpeed;
    private int currentLevel;

    private void Awake()
    {
        // Load the current level
        currentLevel = PlayerPrefs.GetInt("Level", 1);
        currentLevel = Mathf.Clamp(currentLevel, 1, 10); // Ensure level is within 1 to 10
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        // Linearly interpolate the speed based on the current level
        currentSpeed = Mathf.Lerp(baseSpeed, maxSpeed, (currentLevel - 1) / 9f);
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
