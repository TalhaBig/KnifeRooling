using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeManager : MonoBehaviour
{
    public GameObject iconKnifePrefab; // Assign the IconKnife prefab in the Inspector
    public Transform knifePanel; // Assign the panel in the Inspector

    private List<Image> knifeIcons = new List<Image>();
    private int currentKnifeIndex = 0;

    public void InitializeKnives(int totalKnives)
    {
        // Clear any existing icons
        foreach (Transform child in knifePanel)
        {
            Destroy(child.gameObject);
        }

        knifeIcons.Clear();
        currentKnifeIndex = 0;

        // Initialize knife icons
        for (int i = 0; i < totalKnives; i++)
        {
            GameObject icon = Instantiate(iconKnifePrefab, knifePanel);
            knifeIcons.Add(icon.GetComponent<Image>());
        }
    }

    public bool UseKnife()
    {
        if (currentKnifeIndex < knifeIcons.Count)
        {
            knifeIcons[currentKnifeIndex].color = Color.gray; // Change to a darker shade
            currentKnifeIndex++;
            return true;
        }
        return false;
    }

    public bool AreKnivesAvailable()
    {
        return currentKnifeIndex < knifeIcons.Count;
    }
}
