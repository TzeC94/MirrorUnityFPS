using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDropAreaScript : MonoBehaviour, IDropHandler
{
    [Client]
    public void OnDrop(PointerEventData eventData) {

        //Drop Item Behaviour Here
        var dragItem = InventoryUIItemScript.dragTargetUIItem;

        GameManagerBase.instance.DropItemFromInventory(dragItem.itemIndex,
            dragItem.item,
            dragItem.ownerID,
            dragItem.inventoryContainer.inventoryType);

    }

}
