using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUIItemScript : MonoBehaviour, IPointerClickHandler
{
    public ItemData itemData;
    public Image ui_Icon;
    public int itemIndex;
    public uint ownerID;

    public void Setup(ItemData itemData, int itemIndex, uint ownerID) {

        if (this.itemData == itemData)
            return;

        if (itemData == null) {

            //Need remove the icon if is available
            this.itemData = null;
            itemIndex = -1;
            ui_Icon.sprite = null;

            return;

        }

        this.itemData = itemData;
        this.itemIndex = itemIndex;
        this.ownerID = ownerID;

        ResourceManage.LoadAsset<Sprite>(itemData.itemIcon, LoadComplete);

    }

    private void LoadComplete(Sprite itemSprite) {

        ui_Icon.sprite = itemSprite;

    }

    #region IPointerClickHandler

    public void OnPointerClick(PointerEventData eventData) {
        
        if(eventData.button == PointerEventData.InputButton.Right) {

            if(itemData != null) {

                //Remove it
                GameManagerBase.instance.DropItemFromInventory(itemIndex, itemData, ownerID);

            }else {

                Debug.Log("Right Click on nothing");

            }
            
        }

    }

    #endregion

}
