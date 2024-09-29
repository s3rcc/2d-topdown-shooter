using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public Loot lootItem; // The reference to the Loot item (ScriptableObject)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ki?m tra xem ng??i ch?i c� va ch?m v?i item kh�ng
        if (collision.CompareTag("Player"))
        {
            // Th�m item ?� nh?t v�o LootManager
            LootManager.Instance.CollectLoot(lootItem);

            // H?y item ?� nh?t
            Destroy(gameObject);
        }
    }
}