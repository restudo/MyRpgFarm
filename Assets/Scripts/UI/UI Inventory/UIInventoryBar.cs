using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBar : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool _isInventoryBarPositionBottom = true;
    public bool isInventoryBarPositionBottom {get => _isInventoryBarPositionBottom; set=> _isInventoryBarPositionBottom = value; }

    void Awake(){
        rectTransform = GetComponent<RectTransform>();
    }

    void Update(){
        //Switch inventory bar position depend on the player position
        SwitchInventoryBarPosition();
    }

    void SwitchInventoryBarPosition(){
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
        else if(playerViewPortPosition.y <= 0.15f && isInventoryBarPositionBottom == true)
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
