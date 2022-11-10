using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBar : MonoBehaviour
{
    [SerializeField] private Sprite blankSprite = null;
    [SerializeField] private UIInventorySlot[] inventorySlot = null;


    private RectTransform rectTransform;
    private bool _isInventoryBarPositionBottom = true;
    public bool isInventoryBarPositionBottom { get => _isInventoryBarPositionBottom; set => _isInventoryBarPositionBottom = value; }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        EventHandler.InventoryUpdatedEvent += InventoryUpdated;
    }

    void OnDisable()
    {
        EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
    }

    void Update()
    {
        //Switch inventory bar position depend on the player position
        SwitchInventoryBarPosition();
    }

    void ClearInventorySlot()
    {
        if (inventorySlot.Length > 0)
        {
            // loop trough inventory slots and update with blank sprite
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                inventorySlot[i].inventorySlotImage.sprite = blankSprite;
                inventorySlot[i].textMeshProUGUI.text = "";
                inventorySlot[i].itemDetails = null;
                inventorySlot[i].itemQuantity = 0;
            }
        }
    }

    void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        if (inventoryLocation == InventoryLocation.player)
        {
            ClearInventorySlot();

            if (inventorySlot.Length > 0 && inventoryList.Count > 0)
            {
                // loop trough inventory slots and update with corresponding inventory list item
                for (int i = 0; i < inventorySlot.Length; i++)
                {
                    if (i < inventoryList.Count)
                    {
                        int itemCode = inventoryList[i].itemCode;

                        // itemDetail itemDetails = InventoryManager.Instance.itemList.itemDetails.Find(x => x.itemCode == itemCode)
                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

                        if (itemDetails != null)
                        {
                            // add image and details to inventory slot
                            inventorySlot[i].inventorySlotImage.sprite = itemDetails.itemSprite;
                            inventorySlot[i].textMeshProUGUI.text = inventoryList[i].itemQuantity.ToString();
                            inventorySlot[i].itemDetails = itemDetails;
                            inventorySlot[i].itemQuantity = inventoryList[i].itemQuantity;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    void SwitchInventoryBarPosition()
    {
        Vector3 playerViewPortPosition = PlayerController.Instance.GetPlayerViewPortPosition();

        if (playerViewPortPosition.y > 0.15f && isInventoryBarPositionBottom == false)
        {
            // transform.position = new Vector3(tranform.position.x, 7.5f, 0f); this was changed to control the recttranform see below
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0f, 25f);

            isInventoryBarPositionBottom = true;
        }
        else if (playerViewPortPosition.y <= 0.15f && isInventoryBarPositionBottom == true)
        {
            // transform.position = new Vector3(tranform.position.x, mainCamera.pixelHeight - 120f, 0f); this was changed to control the recttranform see below
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -25f);

            isInventoryBarPositionBottom = false;
        }
    }
}
