using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrunkController : MonoBehaviour
{
    public int health = 5;
    public AudioClip breakClip;
    public AudioClip startClip;
    public GameObject applePrefab; // Reference to the apple prefab
    public Transform trunkTransform; // Reference to the trunk's transform
    public float appleRadius = 0.5f; // Adjust based on apple size to ensure no overlap

   private KnifeManager knifeManager;

    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(startClip);

        // Find the KnifeManager and initialize it with the trunk's health
     knifeManager = FindObjectOfType<KnifeManager>();

        if (knifeManager != null)
        {
            knifeManager.InitializeKnives(health);
        }
        else
        {
            Debug.LogError("KnifeManager not found in the scene.");
        }

        // Spawn apples on the trunk
        SpawnApples();
    }

    public void win()
    {
        GameObject.Find("TextMessage").GetComponent<Text>().text = "YOU WIN!";
        StartCoroutine(HandleLevelCompletion());
    }

    // Decrease health
    public void Damage(int value)
    {
        health -= value;

        if (health <= 0) // If there is no health
        {
            // Deactivate Trunk collider
            GetComponent<CircleCollider2D>().enabled = false;

            // Trunk fragmentation
          

            GetComponent<AudioSource>().PlayOneShot(breakClip);

            TrunkReplacementController.instance.Pluscounter();

            FragmentTrunk();
        }
    }


    private void FragmentTrunk()
    {
        // Fragmentation logic
        // Fragmentation 1
        transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
        transform.GetChild(0).GetComponent<Rigidbody>().AddForce(400, 800, 0);
        transform.GetChild(0).GetComponent<Rigidbody>().AddTorque(100, 100, 100);
        transform.GetChild(0).parent = null;
        // Fragmentation 2
        transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
        transform.GetChild(0).GetComponent<Rigidbody>().AddForce(-400, 800, 0);
        transform.GetChild(0).GetComponent<Rigidbody>().AddTorque(-100, 100, 100);
        transform.GetChild(0).parent = null;
        // Fragmentation 3
        transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
        transform.GetChild(0).GetComponent<Rigidbody>().AddForce(0, 800, 0);
        transform.GetChild(0).GetComponent<Rigidbody>().AddTorque(-200, 100, -100);
        transform.GetChild(0).parent = null;

        while (transform.childCount > 0)
        {
            // Knives apart from Trunk
            transform.GetChild(0).GetComponent<Rigidbody2D>().isKinematic = false;
            transform.GetChild(0).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-400f, 400f), Random.Range(400f, 800f)));
            transform.GetChild(0).GetComponent<Rigidbody2D>().AddTorque(Random.Range(-400, 400));
            transform.GetChild(0).parent = null;
        }
    }


    private void SpawnApples()
    {
        // Place a random number of apples (between 1 and 3) on the trunk
        int numberOfApples = Random.Range(1, 4); // Random number between 1 and 3
        List<Vector2> applePositions = new List<Vector2>();

        for (int i = 0; i < numberOfApples; i++)
        {
            Vector2 position;
            bool positionValid;
            int attempts = 0;
            do
            {
                positionValid = true;
                float angle = Random.Range(0f, 360f);
                position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 2f; // Adjust the radius as needed

                // Check for overlap with already placed apples
                foreach (Vector2 existingPosition in applePositions)
                {
                    if (Vector2.Distance(existingPosition, position) < appleRadius * 2f)
                    {
                        positionValid = false;
                        break;
                    }
                }

                // Check for overlap with knives
                foreach (Transform knife in trunkTransform)
                {
                    if (knife.CompareTag("Knife") && Vector2.Distance(knife.position, trunkTransform.position + (Vector3)position) < appleRadius * 2f)
                    {
                        positionValid = false;
                        break;
                    }
                }

                attempts++;
                if (attempts > 100)
                {
                    Debug.LogWarning("Could not find a valid position for the apple.");
                    break;
                }

            } while (!positionValid);

            if (positionValid)
            {
                // Instantiate and position the apple
                GameObject apple = Instantiate(applePrefab, trunkTransform.position + (Vector3)position, Quaternion.identity);
                apple.transform.SetParent(trunkTransform); // Parent it to the trunk to rotate with it
                applePositions.Add(position); // Track the position of this apple
            }
        }
    }

    private IEnumerator HandleLevelCompletion()
    {
        // Wait for 2 seconds before transitioning to the next level
        yield return new WaitForSeconds(2f);

        // Notify GameController to handle level progression
        GameController.LevelCompleted();
    }
}
