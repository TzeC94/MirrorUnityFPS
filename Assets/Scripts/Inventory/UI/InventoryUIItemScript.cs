using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Mirror;

public partial class InventoryUIItemScript : MonoBehaviour, IPointerClickHandler
{
    private Item item = null;
    public Image ui_Icon;
    public int itemIndex;
    public uint ownerID;
    public TextMeshProUGUI itemAmount;

    //Reference to inventory base
    public InventoryBase inventoryContainer;

    [Client]
    public void Initialize(int defaultIndex, InventoryBase container) {

        this.itemAmount.gameObject.SetActive(false);
        this.itemIndex = defaultIndex;
        this.inventoryContainer = container;

    }

    [Client]
    public void Setup(Item item, uint ownerID) {

        if (this.item == item && item != null) {

            if (item.itemData.stackable) {

                this.itemAmount.text = item.quantity.ToString();

            }

            return;
        }

        if (item == null) {

            //Need remove the icon if is available
            this.item = null;
            ui_Icon.sprite = null;
            this.itemAmount.gameObject.SetActive(false);

            return;

        }

        this.item = item;
        this.ownerID = ownerID;
        this.itemAmount.text = item.quantity.ToString();
        this.itemAmount.gameObject.SetActive(true);

        ResourceManage.LoadAsset<Sprite>(item.itemData.itemIcon, LoadComplete);

    }

    [Client]
    private void LoadComplete(Sprite itemSprite) {

        ui_Icon.sprite = itemSprite;

    }

    #region IPointerClickHandler

    [Client]
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
