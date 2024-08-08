using UnityEngine;

public class Disappearing : MonoBehaviour
{
    public static Disappearing instance { get; private set; }
    public static GameObject targetObject; // The object to be toggled
    public static float time = 0f;
    public float maxTime = 2f;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Trunk");
        if (targetObject == null)
        {
            Debug.LogError("Target object not set.");
        }
        time = 0f; // Initialize the timer
    }

    void Update()
    {
        if (targetObject == null) return;

        time += Time.deltaTime;

        if (time > maxTime)
        {
            time = 0f;
            ToggleActiveState();
        }
    }

    public  static  void SetTarget(GameObject target)
    {
        targetObject = target;
        if (targetObject == null)
        {
            Debug.LogError("Target object not set.");
        }
        time = 0f;
    }

    private void ToggleActiveState()
    {
        bool newState = !targetObject.activeSelf;
        targetObject.SetActive(newState);
        Debug.Log("Toggled visibility: " + newState);
    }
}
