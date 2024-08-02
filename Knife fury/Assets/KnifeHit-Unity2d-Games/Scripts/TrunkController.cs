using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrunkController : MonoBehaviour
{
    public float speed = -1.5f;
    public int health = 5;
    public AudioClip breakClip;
    public AudioClip startClip;

    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(startClip);
    }

    void Update()
    {
        // Rotate Trunk in Z axis
        transform.Rotate(new Vector3(0, 0, speed));
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
            FragmentTrunk();

            GetComponent<AudioSource>().PlayOneShot(breakClip);

            // Show "YOU WIN!" and go to the next level
            GameObject.Find("TextMessage").GetComponent<Text>().text = "YOU WIN!";
            StartCoroutine(HandleLevelCompletion());
        }
    }

    void FragmentTrunk()
    {
        TransformFragment(transform.GetChild(0), new Vector3(400, 800, 0), new Vector3(100, 100, 100));
        TransformFragment(transform.GetChild(0), new Vector3(-400, 800, 0), new Vector3(-100, 100, 100));
        TransformFragment(transform.GetChild(0), new Vector3(0, 800, 0), new Vector3(-200, 100, -100));

        while (transform.childCount > 0)
        {
            Rigidbody2D rb = transform.GetChild(0).GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
            rb.AddForce(new Vector2(Random.Range(-400f, 400f), Random.Range(400f, 800f)));
            rb.AddTorque(Random.Range(-400, 400));
            transform.GetChild(0).parent = null;
        }
    }

    void TransformFragment(Transform fragment, Vector3 force, Vector3 torque)
    {
        Rigidbody rb = fragment.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(force);
        rb.AddTorque(torque);
        fragment.parent = null;
    }

    private IEnumerator HandleLevelCompletion()
    {
        // Wait for 2 seconds before transitioning to the next level
        yield return new WaitForSeconds(2f);

        // Notify GameController to handle level progression
        GameController.LevelCompleted();
    }
}