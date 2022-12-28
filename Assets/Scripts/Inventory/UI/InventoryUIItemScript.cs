using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryUIItemScript : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public Image ui_Icon;
    public int itemIndex;
    public uint ownerID;
    public TextMeshProUGUI itemAmount;

    //Reference to inventory base
    public InventoryBase inventoryContainer;

    public void Initialize() {

        this.itemAmount.gameObject.SetActive(false);

    }

    public void Setup(Item item, int itemIndex, uint ownerID, InventoryBase container) {

        if (this.item == null)
            return;

        if (this.item == item) {

            if (item.itemData.stackable) {

                this.itemAmount.text = item.quantity.ToString();

            }

            return;
        }

        if (item == null) {

            //Need remove the icon if is available
            this.item = null;
            itemIndex = -1;
            ui_Icon.sprite = null;
            this.itemAmount.gameObject.SetActive(false);

            return;

        }

        this.item = item;
        this.itemIndex = itemIndex;
        this.ownerID = ownerID;
        this.itemAmount.text = item.quantity.ToString();
        this.itemAmount.gameObject.SetActive(true);
        this.inventoryContainer = container;

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
                GameManagerBase.instance.DropItemFromInventory(itemIndex, item, ownerID, inventoryContainer.inventoryType);

            }else {

                Debug.Log("Right Click on nothing");

            }
            
        }

    }

    #endregion

}
