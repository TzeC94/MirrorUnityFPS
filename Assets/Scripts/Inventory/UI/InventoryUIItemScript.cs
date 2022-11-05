using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUIItemScript : MonoBehaviour, IPointerClickHandler
{
    public ItemData itemData;
    public Image ui_Icon;

    public void Setup() {

        ResourceManage.LoadAsset<Sprite>(itemData.itemIcon, LoadComplete);

    }

    private void LoadComplete(Sprite itemSprite) {

        ui_Icon.sprite = itemSprite;

    }

    #region IPointerClickHandler

    public void OnPointerClick(PointerEventData eventData) {
        
        if(eventData.button == PointerEventData.InputButton.Right) {

            if(itemData != null) {

                Debug.Log($"Right Click on {itemData.itemName}");

            }else {

                Debug.Log("Right Click on nothing");

            }
            

        }

    }

    #endregion

}
