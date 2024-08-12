using UnityEngine;
using System.Collections;

public class TrunkReplacementController : MonoBehaviour
{
    public static TrunkReplacementController instance { get; private set; }
    public GameObject[] trunkPrefabs; // Array of trunk prefabs
    private TrunkController counter;
    private GameObject knife;

    [SerializeField] int maxcount;
    private int currentcount;

    private void Start()
    {
        Debug.Log("hi");
    }

    private void Awake()
    {
        knife = GameObject.Find("Spawn");
        instance = this;
        HandleTrunkReplacement();
    }

    public void Pluscounter()
    {
        currentcount++;
        if (currentcount == maxcount)
        {
            counter.win();
        }
        else
        {
            HandleTrunkReplacement();
        }
    }

    public void HandleTrunkReplacement()
    {
        if (counter != null)
        {
            Destroy(counter.gameObject);
        }

        GameObject trunk;

        // Instantiate a new trunk using a prefab from the array, cycling through them or using another logic
        int prefabIndex = currentcount % trunkPrefabs.Length;
        trunk = Instantiate(trunkPrefabs[prefabIndex], transform.position, Quaternion.identity);

        Disappearing.SetTarget(trunk);

        counter = trunk.GetComponent<TrunkController>();
        // Move to the next level
        StartCoroutine(KnifeAi());
    }

    private IEnumerator KnifeAi()
    {
        yield return new WaitForSeconds(0f);
        knife.GetComponent<SpawnController>().CreateKnife();
    }
}
