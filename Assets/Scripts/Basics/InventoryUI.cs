using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    
    public List<Image> inventorySlots; // Assign the 5 slots in the Inspector

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Ensure all slots are hidden at the start
        foreach (var slot in inventorySlots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    public void AddItemToInventory(Sprite itemIcon)
    {
        StartCoroutine(WaitForCollectionAnimation(itemIcon));
    }

    private IEnumerator WaitForCollectionAnimation(Sprite itemIcon)
    {
        yield return new WaitForSeconds(Management_HUD.Instance.animationDuration + 0.5f); // Wait for UI animation to finish
        
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (!inventorySlots[i].gameObject.activeSelf) // Find the first hidden slot
            {
                inventorySlots[i].sprite = itemIcon;
                inventorySlots[i].gameObject.SetActive(true); // Enable the slot
                yield break; // Stop coroutine properly
            }
        }
    }
}
