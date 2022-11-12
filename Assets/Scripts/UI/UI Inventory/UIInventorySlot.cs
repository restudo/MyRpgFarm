using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera mainCamera;
    private Transform parentItem;
    private GameObject draggedItem;

    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private UIInventoryBar inventoryBar = null;
    [SerializeField] private GameObject itemPrefab = null;
    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;

    void Start()
    {
        mainCamera = Camera.main;
        parentItem = GameObject.FindGameObjectWithTag(Tags.itemsParentTranform).transform;
    }

    private void DropSelectedItemAtMousePosition()
    {
        if (itemDetails != null)
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, +10));

            // create item from prefab at mouse position
            GameObject itemGameObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
            Item item = itemGameObject.GetComponent<Item>();
            item.itemCode = itemDetails.itemCode;

            // remove item from player inventory
            InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.itemCode);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            // disable player input
            PlayerController.Instance.DisablePlayerInputAndResetMovement();

            // instantiate gameobject from inventory bar ui
            draggedItem = Instantiate(inventoryBar.InventoryBarDraggedItem, inventoryBar.transform);

            // get image
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // follow mouse position
        if (draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // destroy gameobject as dragged item
        Destroy(draggedItem);

        //if drag ends over inventory bar, get item drag is over and swap them
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
        {

        }
        //else attempt to drop the item if it can be drop
        else
        {
            if (itemDetails.canBeDropped)
            {
                DropSelectedItemAtMousePosition();
            }
        }

        PlayerController.Instance.EnablePlayerInput();

    }
}
