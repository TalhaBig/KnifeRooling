using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TrunkReplacementController : MonoBehaviour
{ public static TrunkReplacementController instance { get; private set; }
    public GameObject trunkPrefab; // Reference to the trunk prefab to instantiate
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
        HandleTrunkReplacement(trunkPrefab.transform.position);

       
    }

    public  void Pluscounter()
    {
        currentcount++;
        if (currentcount == maxcount)
        {
            counter.win();
        }
        else
        {
            HandleTrunkReplacement(trunkPrefab.transform.position);
        }
    }

    public void HandleTrunkReplacement(Vector3 position) { 

        if(counter !=null)
        {
            Destroy(counter.gameObject);
        }

        GameObject trunk;


        // Instantiate a new trunk at the position of the broken trunk

       trunk =  Instantiate(trunkPrefab, position, Quaternion.identity);
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
