using UnityEngine;

public class Apple : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Knife"))
        {
            // Add points to the score
            GameController.SetScore(10);
            // Destroy the apple object
            Destroy(gameObject);
        }
    }
}
