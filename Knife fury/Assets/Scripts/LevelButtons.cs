using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour
{
    public Button[] levelButtons;

    void Start()
    {
        UpdateLevelButtons();
    }

    private void UpdateLevelButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNumber = i + 1; // Button index starts at 0 but levels start at 1
            bool isUnlocked = GameController.IsLevelUnlocked(levelNumber);
            levelButtons[i].interactable = isUnlocked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If you need to update buttons dynamically, call UpdateLevelButtons() here
    }
}
