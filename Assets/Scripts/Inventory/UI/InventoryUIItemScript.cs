using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUIItemScript : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public Image ui_Icon;
    public int itemIndex;
    public uint ownerID;

    public void Setup(Item item, int itemIndex, uint ownerID) {

        if (this.item == item)
            return;

        if (item == null) {

            //Need remove the icon if is available
            this.item = null;
            itemIndex = -1;
            ui_Icon.sprite = null;

            return;

        }

        this.item = item;
        this.itemIndex = itemIndex;
        this.ownerID = ownerID;

        ResourceManage.LoadAsset<Sprite>(item.itemData.itemIcon, LoadComplete);

    }

    private void LoadComplete(Sprite itemSprite) {

        ui_Icon.sprite = itemSprite;

    }

    #region IPointerClickHandler

    public void OnPointerClick(PointerEventData eventData) {
        
        if(eventData.button == PointerEventData.InputButton.Right) {

            if(item != null) {

                //Remove it
                GameManagerBase.instance.DropItemFromInventory(itemIndex, item, ownerID);

            }else {

                Debug.Log("Right Click on nothing");

            }
            
        }

    }

    #endregion

}
