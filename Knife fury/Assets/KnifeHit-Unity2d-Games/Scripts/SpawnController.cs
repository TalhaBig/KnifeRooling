using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnController : MonoBehaviour
{

    public GameObject knife;
    public Text text_score, text_high_score;

    // Use this for initialization
    void Start()
    {
        CreateKnife();
    }

    // Update is called once per frame
    void Update()
    {
        text_score.text = "Score: " + GameController.GetScore();
        text_high_score.text = "High score: " + GameController.GetHighScore();
    }

    public void CreateKnife()
    {
        // Create knife if Trunk is alive!
        if (GameObject.Find("Trunk").GetComponent<TrunkController>().health > 1)
            Instantiate(knife, transform.position, Quaternion.identity);
    }
}