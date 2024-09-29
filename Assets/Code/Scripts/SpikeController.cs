using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeController : MonoBehaviour
{
    public Tilemap tilemap; // Tham chi?u ??n Tilemap ch?a spike
    public Tile spikeTileNoGai; // Tile spike không có gai
    public Tile spikeTileCoGai; // Tile spike có gai
    public int damageAmount = 10; // L??ng sát th??ng gây ra cho ng??i ch?i

    private void Start()
    {
        // ??m b?o r?ng Tilemap ?ã ???c g?n
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with a spike");

            Vector3 hitPosition = Vector3.zero;

            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x;
                hitPosition.y = hit.point.y;
                Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);

                // In ra vị trí tile và tile hiện tại
                Debug.Log("Tile Position: " + tilePosition);
                Debug.Log("Current Tile: " + tilemap.GetTile(tilePosition));

                // Kiểm tra xem tile tại vị trí va chạm có phải là spike không có gai không
                if (tilemap.GetTile(tilePosition) == spikeTileNoGai)
                {
                    // Đổi tile sang hình có gai
                    tilemap.SetTile(tilePosition, spikeTileCoGai);
                    Debug.Log("Changed tile to spike with spikes.");

                    // Gây sát thương cho người chơi
                    TriggerSpike(collision.gameObject);
                }
            }
        }
    }


    private void TriggerSpike(GameObject player)
    {
        // G?i hàm gây sát th??ng cho ng??i ch?i (n?u player có script PlayerHealth)
        Player playerHealth = player.GetComponent<Player>();
        if (playerHealth != null)
        {
            playerHealth.Hit(damageAmount); // Gây sát th??ng
        }

        // B?n có th? thêm hi?u ?ng âm thanh ho?c ho?t ?nh t?i ?ây
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 hitPosition = Vector3.zero;

            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x;
                hitPosition.y = hit.point.y;
                Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);

                // Khi ng??i ch?i r?i kh?i tile, ??i l?i tile không có gai
                if (tilemap.GetTile(tilePosition) == spikeTileCoGai)
                {
                    tilemap.SetTile(tilePosition, spikeTileNoGai);
                }
            }
        }
    }
}
