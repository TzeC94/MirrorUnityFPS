using UnityEngine;
using UnityEngine.EventSystems;

public partial class InventoryUIItemScript : IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {

    public static InventoryUIItemScript dragTargetUIItem;
    private static GameObject dragTarget;
    private static Vector3 dragTarget_OriPos;

    public void OnBeginDrag(PointerEventData eventData) {

        if (item != null) {

            dragTargetUIItem = this;
            dragTarget = ui_Icon.transform.gameObject;
            dragTarget_OriPos = ui_Icon.transform.position;
            
        }

    }

    public void OnDrag(PointerEventData eventData) {
        
        if(dragTarget == ui_Icon.transform.gameObject) {

            dragTarget.transform.position = eventData.position;

        }

    }

    public void OnDrop(PointerEventData eventData) {

        //IF move within same inventory
        if (dragTargetUIItem.inventoryContainer == inventoryContainer) {

            inventoryContainer.CmdMoveItemFromToIndex(dragTargetUIItem.itemIndex, itemIndex);

        } else {    // Cross Inventory

            dragTargetUIItem.inventoryContainer.CmdMoveItemFromToInventory(dragTargetUIItem.itemIndex,
                ownerID, inventoryContainer.inventoryType, itemIndex);

        }

    }

    public void OnEndDrag(PointerEventData eventData) {

        if(dragTargetUIItem != null) {

            dragTargetUIItem = null;
            dragTarget = null;
            ui_Icon.transform.position = dragTarget_OriPos;

        }

    }

}