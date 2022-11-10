
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
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

        // DebugPrintInventoryList(inventoryList);
    }

    // void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    // {
    //     foreach (InventoryItem inventoryItem in inventoryList)
    //     {
    //         Debug.Log("Item Desctiption : " + InventoryManager.Instance.GetItemDetails(inventoryItem.itemCode).itemDescription +
    //          "  Item Quantity : " + inventoryItem.itemQuantity);
    //     }
    //     Debug.Log("=====================================");
    // }

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
}
