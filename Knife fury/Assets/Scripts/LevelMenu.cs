using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenuController : MonoBehaviour
{
    public Button[] levelButtons; // Array of buttons for levels
    private SwipeController swipeController;

    private void Start()
    {
        swipeController = FindObjectOfType<SwipeController>();

        UpdateLevelButtons();
    }

    private void UpdateLevelButtons()
    {
        int currentPage = swipeController.GetCurrentPage(); // Assuming SwipeController has a method GetCurrentPage
        int stageIndex = currentPage; // Each page corresponds to a stage

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1; // Button index starts at 0 but levels start at 1
            int levelNumber = (stageIndex - 1) * 10 + levelIndex;
            bool isUnlocked = GameController.IsLevelUnlocked(levelNumber);

           // levelButtons[i].interactable = isUnlocked;

            if (isUnlocked)
            {
                int level = levelIndex;
                levelButtons[i].onClick.RemoveAllListeners();
                levelButtons[i].onClick.AddListener(() => OnLevelButtonClick(level));
            }
        }
    }

    private void OnLevelButtonClick(int levelIndex)
    {
        int currentPage = swipeController.GetCurrentPage(); // Assuming SwipeController has a method GetCurrentPage
        int stageIndex = currentPage; // Each page corresponds to a stage
        int levelWithinStage = levelIndex; // Button index corresponds to the level within the stage

        Debug.Log($"Button Clicked: Level {levelIndex}, Stage {stageIndex}, LevelWithinStage {levelWithinStage}");

        // Set current level and stage using static method
        GameController.SetLevel(levelWithinStage, stageIndex);

        // Load the selected scene
        SceneManager.LoadScene("Scene" + stageIndex);
    }
}