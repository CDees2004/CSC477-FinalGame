using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string itemName; // The name of the item
    public Sprite itemIcon; // The icon for the UI
    public AudioClip collectSound; // Sound when collected

    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            InventoryUI.Instance.AddItemToInventory(itemIcon);

            // Play collection sound
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            // Play UI animation
            UIManager.Instance.PlayItemCollectedAnimation(itemIcon);

            // Destroy the collectible object
            Destroy(gameObject);
        }
    }
}
