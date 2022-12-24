using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class InventoryBase : NetworkBehaviour {

    [SerializeField]
    protected int inventoryMax = 20;

    public readonly SyncList<Item> collectedItems = new SyncList<Item>(new Item());

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    public override void OnStartClient() {

        base.OnStartClient();
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

    public Item FindItemInInventory(int itemIndex) {

        if(collectedItems.Count > itemIndex) {

            return collectedItems[itemIndex];

        }

        return null;

    }

    public virtual void OnInventoryChanged(SyncList<Item>.Operation op, int itemIndex, Item oldItem, Item newItem) {

        

    }

}
