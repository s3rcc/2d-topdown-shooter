using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public Loot lootItem; // The reference to the Loot item (ScriptableObject)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ki?m tra xem ng??i ch?i có va ch?m v?i item không
        if (collision.CompareTag("Player"))
        {
            // Thêm item ?ã nh?t vào LootManager
            LootManager.Instance.CollectLoot(lootItem);

            // H?y item ?ã nh?t
            Destroy(gameObject);
        }
    }
}