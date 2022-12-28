using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;

public class InventoryBase : NetworkBehaviour {

    public enum InventoryType { main = 0, held = 1 };

    [SerializeField]
    private InventoryType _inventoryType = InventoryType.main;
    public InventoryType inventoryType { get { return _inventoryType; } }

    [SerializeField]
    protected int inventoryMax = 20;

    public readonly SyncList<Item> collectedItems = new SyncList<Item>(new Item());

    // Start is called before the first frame update
    public virtual void Start()
    {
        collectedItems.Callback += OnInventoryChanged;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    [Server]
    public void AddToInventory(Item newItem) {

        //Try fill into empty slot or stack if is possible
        if(collectedItems.Count > 0) {

            var newItemData = newItem.itemData;

            //Loop the Item in inventory first to stack first
            if (newItemData.stackable) {

                for (int i = 0; i < collectedItems.Count; i++) {

                    var currentItem = collectedItems[i];

                    if (currentItem != null) {

                        if (currentItem.itemData.itemID == newItemData.itemID && currentItem.itemData.stackable && currentItem.EnoughToStack(newItem.quantity)) {

                            //Add to stack
                            Item newModItem = new Item(currentItem);
                            newModItem.quantity += newItem.quantity;
                            collectedItems[i] = newModItem;
                            return;
                        }

                    }

                }

            }

            for (int i = 0; i < collectedItems.Count; i++) {

                var currentItem = collectedItems[i];

                if (currentItem == null) {

                    collectedItems[i] = newItem;
                    return;

                }

            }

        }
        
        collectedItems.Add(newItem);

    }

    [Server]
    public bool RemoveFromInventory(int itemIndex) {

        if (collectedItems[itemIndex] != null) {

            collectedItems[itemIndex] = null;

            return true;
        }

        return false;
    }

    [Server]
    public void PutItemAtIndex(int index, Item item) {

        collectedItems[index] = item;

    }

    [Server]
    public void RemoveQuantityFromInventory(uint quantityToRemove, Item targetItem) {

        for(int i = 0; i < collectedItems.Count; i++) {

            var cItem = collectedItems[i];

            if(cItem == targetItem) {

                //If empty need remove this item from inventory
                if(cItem.quantity - quantityToRemove == 0) {

                    RemoveFromInventory(i);
                    return;

                } else {

                    Item newItemData = new Item(cItem);
                    newItemData.quantity -= quantityToRemove;
                    collectedItems[i] = newItemData;
                    return;

                }

            }

        }

    }

    public Item FindItemInInventory(int itemIndex) {

        if(collectedItems.Count > itemIndex) {

            return collectedItems[itemIndex];

        }

        return null;

    }

    public Item FindItemInInventory(string itemID, bool reorderIfStackable = true) {

        List<Item> collectedItem = new List<Item>();

        for(int i = 0; i < collectedItems.Count; i++) {

            var cItem = collectedItems[i];

            if (cItem.itemData.itemID == itemID) {

                collectedItem.Add(cItem);

            }

        }

        //Mean we find something
        if(collectedItem.Count > 0) {

            //Reorder if is stackable
            if (collectedItem[0].itemData.stackable && reorderIfStackable) {

                collectedItem = collectedItem.OrderByDescending(x => x.quantity).ToList();

            }

            return collectedItem[0];
        }

        return null;

    }

    public virtual void OnInventoryChanged(SyncList<Item>.Operation op, int itemIndex, Item oldItem, Item newItem) {

        

    }

}
