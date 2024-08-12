using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnController : MonoBehaviour
{
    public GameObject knife;
    public Text text_score, text_high_score;

    void Start()
    {
      //  CreateKnife();
    }

    void Update()
    {
        text_score.text = "Score: " + GameController.GetScore();
        text_high_score.text = "High score: " + GameController.GetHighScore();
    }

    public void CreateKnife()
    {
        // Create knife if Trunk is alive!
        if (GameObject.FindObjectOfType<TrunkController>().health > 1)
            Instantiate(knife, transform.position, Quaternion.identity);
    }
}
