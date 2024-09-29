using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    private LootManager lootManager; // Reference to LootManager

    private void Start()
    {
        // Find the LootManager in the scene
        lootManager = FindObjectOfType<LootManager>();
    }

    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            Loot droppedItems = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItems;
        }
        Debug.Log("No loot dropped");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot droppedItem = GetDroppedItem();
        if (droppedItem != null)
        {
            // Instantiate the loot in the world
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;

            // Assign the lootItem data to the CollectibleItem script
            lootGameObject.GetComponent<CollectibleItem>().lootItem = droppedItem;

            // Remove the force application to make the loot stand still
            // float dropForce = 300f;
            // Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            // lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
        }
    }

}
