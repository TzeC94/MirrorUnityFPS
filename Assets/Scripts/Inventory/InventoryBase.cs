using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class InventoryBase : NetworkBehaviour
{
    [SerializeField]
    protected int inventoryMax = 20;

    [SerializeField]
    public readonly SyncList<Item> collectedItems = new SyncList<Item>();

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
    public void AddToInventory(Item newItemData) {

        //Try fill into empty slot if is possible
        if(collectedItems.Count > 0) {

            for(int i = 0; i < collectedItems.Count;i++) {

                if (collectedItems[i] == null) {

                    collectedItems[i] = newItemData;
                    return;
                }

            }

        }
        
        collectedItems.Add(newItemData);

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
