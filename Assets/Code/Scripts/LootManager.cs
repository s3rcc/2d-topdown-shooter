using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootManager : MonoBehaviour
{
    public GameObject lootBarPanel;        // The UI panel where loot icons are displayed
    public GameObject lootIconPrefab;      // Prefab for loot icons

    private Dictionary<string, int> collectedLoot = new Dictionary<string, int>();
    private Dictionary<string, GameObject> lootIconUI = new Dictionary<string, GameObject>();

    public static LootManager Instance { get; private set; } // Singleton pattern

    private void Awake()
    {
        // Ensure the LootManager is a singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectLoot(Loot lootItem)
    {
        string lootName = lootItem.lootName;

        if (collectedLoot.ContainsKey(lootName))
        {
            collectedLoot[lootName]++;
            UpdateLootUI(lootName);
        }
        else
        {
            collectedLoot[lootName] = 1;
            CreateLootIcon(lootItem);
        }
    }

    private void CreateLootIcon(Loot lootItem)
    {
        // Instantiate a new loot icon and add it to the UI
        GameObject newLootIcon = Instantiate(lootIconPrefab, lootBarPanel.transform);
        newLootIcon.GetComponent<Image>().sprite = lootItem.lootSprite;

        // Add the text to display the loot count
        Text countText = newLootIcon.GetComponentInChildren<Text>();
        countText.text = "x1";

        // Store reference to the loot icon UI
        lootIconUI[lootItem.lootName] = newLootIcon;
    }

    private void UpdateLootUI(string lootName)
    {
        if (lootIconUI.ContainsKey(lootName))
        {
            // Update the loot icon's count in the UI
            Text countText = lootIconUI[lootName].GetComponentInChildren<Text>();
            countText.text = "x" + collectedLoot[lootName].ToString();
        }
    }
}
