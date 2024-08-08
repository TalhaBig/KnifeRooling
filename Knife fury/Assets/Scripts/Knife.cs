using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Knife : MonoBehaviour
{
    public float speed = 20f;
    public float downwardSpeed = 10f; // Speed at which the knife moves downward after collision
    public AudioClip hitSound;
    public AudioClip fail;
    private Rigidbody2D knifeRigid;
    private bool moving = false;
    private GameObject spawn;
    public GameObject particle;
    public bool scriptEnabled = true;
    private AudioSource audioSource;
    private bool hasHitTrunk = false;

    private Camera mainCamera; // Reference to the main camera

    void Start()
    {
        knifeRigid = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Cache AudioSource
        spawn = GameObject.Find("Spawn");
        mainCamera = Camera.main; // Get the main camera
    }

    void FixedUpdate()
    {
        if (moving)
            knifeRigid.MovePosition(knifeRigid.position + Vector2.up * speed * Time.deltaTime);

        if (Input.GetMouseButton(0))
            moving = true;

        // Check if the knife has gone out of the camera view
        if (!IsKnifeInCameraView())
        {
            HandleGameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (scriptEnabled)
        {
            if (collider.CompareTag("Trunk"))
            {
                hasHitTrunk = true;
                HandleTrunkCollision(collider);
            }
            else if (collider.CompareTag("Hurdle") || collider.CompareTag("Knife"))
            {
                HandleObstacleCollision();
            }
            else if (collider.CompareTag("Apple"))
            {
                return;
            }
            else
            {
                HandleGameOver();
            }
        }
    }

    private void HandleTrunkCollision(Collider2D trunk)
    {
        moving = false;
        transform.parent = trunk.transform;
        GetComponent<PolygonCollider2D>().enabled = false;
        knifeRigid.bodyType = RigidbodyType2D.Kinematic;
        transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = true;
        audioSource.PlayOneShot(hitSound);
        spawn.GetComponent<SpawnController>().CreateKnife();
        trunk.GetComponent<Animator>().SetTrigger("Hit");
        trunk.GetComponent<TrunkController>().Damage(1);

        Instantiate(particle, transform.position + transform.up * 0.25f, Quaternion.identity);

        GameController.SetScore(10);
        scriptEnabled = false;
        GetComponent<Knife>().enabled = false; // Disable the script
    }

    private void HandleObstacleCollision()
    {
        moving = false;
        audioSource.PlayOneShot(fail);
        GameObject.Find("TextMessage").GetComponent<Text>().text = "GAME OVER";
        GameController.SaveHighScore();
        GameController.ResetScore();
        StartCoroutine(MoveKnifeDownward());
    }

    public void HandleGameOver()
    {
        if (scriptEnabled && !hasHitTrunk)
        {
            moving = false;
            knifeRigid.velocity = Vector2.zero;
            knifeRigid.bodyType = RigidbodyType2D.Kinematic;
            GetComponent<PolygonCollider2D>().enabled = false;

            GameObject.Find("TextMessage").GetComponent<Text>().text = "GAME OVER";
            audioSource.PlayOneShot(fail);
            GameController.SaveHighScore();
            GameController.ResetScore();
            StartCoroutine(HandleGameOverCoroutine());
        }
    }

    private bool IsKnifeInCameraView()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    private IEnumerator MoveKnifeDownward()
    {
        knifeRigid.velocity = Vector2.zero; // Stop any existing movement
        knifeRigid.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<PolygonCollider2D>().enabled = false; // Disable collider to avoid interactions

        while (true)
        {
            knifeRigid.MovePosition(knifeRigid.position + Vector2.down * downwardSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator HandleGameOverCoroutine()
    {
        yield return new WaitForSeconds(2f);
        GameController.ResetGame(); // Handle game restart
    }
}
