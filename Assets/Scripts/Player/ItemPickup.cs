
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();

        if (item != null)
        {
            // Get item details
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.itemCode);

            // if item can be picked up
            if (itemDetails.canBePickedUp == true)
            {
                // Add item to inventory
                InventoryManager.Instance.AddItem(InventoryLocation.player, item, collision.gameObject);
            }
        }
    }
}
