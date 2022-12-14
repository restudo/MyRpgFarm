
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    private int[] selectedInventoryItem;    // the index of the array is the inventory List
                                            // from the inventoriLocation enum,
                                            // and the value is the capacity of that inventory List
    private Dictionary<int, ItemDetails> itemDetailsDictionary;

    public List<InventoryItem>[] inventoryLists;

    [HideInInspector]
    // the index of the array is the inventory list (from the InventoryLocation enum),
    // and the value is the capacity of that inventory list
    public int[] inventoryListCapacityIntArray;

    [SerializeField] private SO_ItemLists itemLists = null;

    protected override void Awake()
    {
        base.Awake();

        // Create Inventory List
        CreateInventoryLists();

        //Create item details dictionary
        CreateItemDetailsDictionary();

        // Initialise selected Inventory Item array
        selectedInventoryItem = new int[(int)InventoryLocation.count];
        for (int i = 0; i < selectedInventoryItem.Length; i++)
        {
            selectedInventoryItem[i] = -1;
        }
    }

    void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for (int i = 0; i < (int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }

        // Initialise Inventory List capacity array
        inventoryListCapacityIntArray = new int[(int)InventoryLocation.count];

        // Initialise Player Inventory List Capacity
        inventoryListCapacityIntArray[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
    }

    /// <summary>
    /// Populates the ItemDetailsDictionary from the ScriptableObject item list
    /// </summary>
    void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();

        foreach (ItemDetails itemDetails in itemLists.itemDetails)
        {
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }
    }

    /// <summary>
    /// Add an item to the inventory list for the inventoryLocation and then destroy the gameObject to delete
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameObjectToDelete)
    {
        AddItem(inventoryLocation, item);
        Destroy(gameObjectToDelete);
    }

    /// <summary>
    /// Add an item to the inventory list for the inventoryLocation
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, Item item)
    {
        int itemCode = item.itemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        // Check if inventory already contains the item
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        // if (inventoryList[(int)inventoryLocation].itemQuantity == maxStack)
        // {
        //     AddItemAtPosition(inventoryList, itemCode);
        // }
        if (itemPosition != -1)
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }

        // send event that Inventory has ben updated
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }

    /// <summary>
    /// Add an item to the end of inventory
    /// </summary>
    public void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem();



        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryList.Add(inventoryItem);

        // DebugPrintInventoryList(inventoryList);
    }

    /// <summary>
    /// Add an item to the end of inventory
    /// /// </summary>
    public void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[position].itemQuantity + 1;
        inventoryItem.itemQuantity = quantity;
        inventoryItem.itemCode = itemCode;
        inventoryList[position] = inventoryItem;
    }

    /// <summary>
    /// Find if an item code is already int the inventory. Return the item position
    /// in the inventory list, or -1 if the item not in the inventory
    /// </summary>
    public int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].itemCode == itemCode)
            {
                return i;
            }

        }
        return -1;

    }

    /// <summary>
    /// Return the itemDetails (from SO_ItemList) for the itemcode, or null if the itemCode doesn't exist
    /// </summary>
    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetails;
        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
        {
            return itemDetails;
        }
        else
        {
            return null;
        }

    }

    /// <summary>
    /// Remove an item from the inventory, and create a gameObject at the position it was dropped
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCode"></param>
    public void RemoveItem(InventoryLocation player, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)player];

        // check if inventory already contains the item
        int itemPosition = FindItemInInventory(player, itemCode);

        if (itemPosition != -1)
        {
            RemoveItemAtPosition(inventoryList, itemCode, itemPosition);
        }

        // send event that inventory has been updated
        EventHandler.CallInventoryUpdatedEvent(player, inventoryLists[(int)player]);
    }

    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int itemPosition)
    {
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[itemPosition].itemQuantity - 1;

        if (quantity > 0)
        {
            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemCode = itemCode;
            inventoryList[itemPosition] = inventoryItem;
        }
        else
        {
            inventoryList.RemoveAt(itemPosition);
        }
    }

    /// <summary>
    /// Swap item at fromItem index with item at toItem index in inventoryLocation inventory List
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="fromItem"></param>
    /// <param name="toItem"></param>
    public void SwapInventoryItem(InventoryLocation inventoryLocation, int fromItem, int toItem)
    {
        // if fromItem index and toItem index are within the bounds of the list, not the same, and greater than
        // or equal to zero
        if (fromItem < inventoryLists[(int)inventoryLocation].Count && toItem < inventoryLists[(int)inventoryLocation].Count
            && fromItem != toItem && fromItem >= 0 && toItem >= 0)
        {
            InventoryItem fromInventoryItem = inventoryLists[(int)inventoryLocation][fromItem];
            InventoryItem toInventoryItem = inventoryLists[(int)inventoryLocation][toItem];

            inventoryLists[(int)inventoryLocation][toItem] = fromInventoryItem;
            inventoryLists[(int)inventoryLocation][fromItem] = toInventoryItem;

            // send event that inventory has been update
            EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
        }
    }

    /// <summary>
    /// Set the selected inventory item for inventoryLocation to itemCode
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="itemCode"></param>
    public void SetSelectedInventoryItem(InventoryLocation inventoryLocation, int itemCode)
    {
        selectedInventoryItem[(int)inventoryLocation] = itemCode;
    }

    /// <summary>
    /// Clear the selected inventory item for inventoryLocation
    /// </summary>
    /// <param name="inventoryLocation"></param>
    public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        selectedInventoryItem[(int)inventoryLocation] = -1;
    }
}
