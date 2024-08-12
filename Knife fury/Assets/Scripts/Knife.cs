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
    private KnifeManager knifeManager; // Reference to the KnifeManager

    void Start()
    {
        knifeRigid = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Cache AudioSource
        spawn = GameObject.Find("Spawn");
        mainCamera = Camera.main; // Get the main camera
        knifeManager = FindObjectOfType<KnifeManager>();

        if (knifeManager == null)
        {
            Debug.LogError("KnifeManager not found in the scene.");
        }
    }

    void FixedUpdate()
    {
        if (moving)
            knifeRigid.MovePosition(knifeRigid.position + Vector2.up * speed * Time.deltaTime);

        if (Input.GetMouseButton(0) && !moving && knifeManager.AreKnivesAvailable())
        {
            moving = knifeManager.UseKnife();
        }

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
            if (collider.CompareTag("Hurdle") || collider.CompareTag("Knife"))
            {
                HandleObstacleCollision(); // Prioritize game over
            }
            else if (collider.CompareTag("Trunk"))
            {
                if (!hasHitTrunk) // Ensure trunk hit processing only if not already hit
                {
                    hasHitTrunk = true;
                    HandleTrunkCollision(collider);
                }
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
        if (!scriptEnabled) return; // Prevent further processing if game over has been triggered

        moving = false;
        AlignKnifeWithTrunk(trunk.transform); // Align the knife with the trunk's surface
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

    private void AlignKnifeWithTrunk(Transform trunkTransform)
    {
        // Get the direction from the trunk center to the knife position
        Vector2 direction = (Vector2)transform.position - (Vector2)trunkTransform.position;
        direction.Normalize();

        // Align the knife's up direction to this direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 270f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void HandleObstacleCollision()
    {
        if (!scriptEnabled) return; // Prevent further processing if game over has been triggered

        moving = false;
        knifeRigid.velocity = Vector2.zero; // Stop all momentum immediately
        knifeRigid.bodyType = RigidbodyType2D.Dynamic; // Allow gravity to take effect
        audioSource.PlayOneShot(fail);
        GameObject.Find("TextMessage").GetComponent<Text>().text = "GAME OVER";
        GameController.SaveHighScore();
        GameController.ResetScore();

        scriptEnabled = false; // Prevent further processing
        GetComponent<Knife>().enabled = false; // Disable the script

        StartCoroutine(HandleGameOverCoroutine());
    }

    public void HandleGameOver()
    {
        if (scriptEnabled && !hasHitTrunk)
        {
            moving = false;
            knifeRigid.velocity = Vector2.zero; // Stop all momentum immediately
            knifeRigid.bodyType = RigidbodyType2D.Dynamic; // Allow the knife to fall down naturally
            GetComponent<PolygonCollider2D>().enabled = false;

            GameObject.Find("TextMessage").GetComponent<Text>().text = "GAME OVER";
            audioSource.PlayOneShot(fail);
            GameController.SaveHighScore();
            GameController.ResetScore();

            scriptEnabled = false; // Prevent further processing
            GetComponent<Knife>().enabled = false; // Disable the script

            StartCoroutine(HandleGameOverCoroutine());
        }
    }

    private bool IsKnifeInCameraView()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    private IEnumerator HandleGameOverCoroutine()
    {
        yield return new WaitForSeconds(2f);
        GameController.ResetGame(); // Handle game restart
    }
}