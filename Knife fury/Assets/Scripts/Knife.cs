using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // Needed for scene management

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

    // Colliders for different parts of the knife
    public PolygonCollider2D fullPolygonCollider; // Entire knife
    public BoxCollider2D externalBoxCollider; // External part
    public BoxCollider2D internalBoxCollider; // Internal part

    private bool isPaused = false; // Flag to check if the game is paused

    // Reference to the SpriteRenderer (for 2D)
    private SpriteRenderer spriteRenderer;

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

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isPaused) return; // Skip updates if game is paused

        // Check if input is from a UI button
        if (Input.GetMouseButtonDown(0) && !moving && knifeManager.AreKnivesAvailable() && !IsPointerOverButton())
        {
            Debug.Log("Mouse button pressed and not over UI button.");
            moving = knifeManager.UseKnife();
        }
    }

    void FixedUpdate()
    {
        if (isPaused || !scriptEnabled) return; // Skip updates if game is paused or script is disabled

        if (moving)
            knifeRigid.MovePosition(knifeRigid.position + Vector2.up * speed * Time.deltaTime);

        // Check if the knife has gone out of the camera view
        if (!IsKnifeInCameraView())
        {
            HandleGameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!scriptEnabled) return; // Prevent further processing if game over has been triggered

        if (collider.CompareTag("Hurdle") || collider.CompareTag("Knife"))
        {
            HandleGameOver();
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

    private void HandleTrunkCollision(Collider2D trunk)
    {
        if (!scriptEnabled) return; // Prevent further processing if game over has been triggered

        moving = false;
        AlignKnifeWithTrunk(trunk.transform); // Align the knife with the trunk's surface
        transform.parent = trunk.transform;

        // Disable the full collider and internal collider, enable the external collider
        fullPolygonCollider.enabled = false;
        internalBoxCollider.enabled = false;
        externalBoxCollider.enabled = true;

        knifeRigid.bodyType = RigidbodyType2D.Kinematic;
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

    private void HandleGameOver()
    {
        if (scriptEnabled)
        {
            // Prioritize game over
            moving = false;
            knifeRigid.velocity = Vector2.zero; // Stop all momentum immediately
            knifeRigid.bodyType = RigidbodyType2D.Dynamic; // Allow the knife to fall down naturally

            // Disable all colliders
            fullPolygonCollider.enabled = false;
            internalBoxCollider.enabled = false;
            externalBoxCollider.enabled = false;

            // Apply a small force to push the knife away from the trunk and make it spin
            knifeRigid.AddForce(new Vector2(Random.Range(-2f, 2f), 1f) * 5f, ForceMode2D.Impulse);
            knifeRigid.angularVelocity = 500f; // Apply rapid spinning

            // Set the knife to be in front of everything

            GameObject knifeObject = knifeRigid.gameObject;
            knifeObject.transform.position += new Vector3(0,0,-1);

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

    private bool IsPointerOverButton()
    {
        // Check if the pointer is over a button
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Get the game object that the pointer is over
            GameObject currentObject = EventSystem.current.currentSelectedGameObject;

            // Check if the current UI object is a Button
            if (currentObject != null && currentObject.GetComponent<Button>() != null)
            {
                Debug.Log("Pointer is over a button.");
                return true;
            }
        }

        return false;
    }

    private IEnumerator HandleGameOverCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    // Method to call when pausing the game
    public void PauseGame(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0 : 1; // Freeze or unfreeze game time
        if (pause)
        {
            // Additional pause logic if needed
        }
    }
}
