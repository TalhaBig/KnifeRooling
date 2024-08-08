using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text levelText;

    void Update()
    {
        levelText.text = "Level " + GameController.currentLevel;
    }
}
