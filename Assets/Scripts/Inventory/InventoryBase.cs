using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;

public abstract class InventoryBase : BaseScriptNetwork {

    public enum InventoryType { main = 0, held = 1 };

    [SerializeField]
    private InventoryType _inventoryType = InventoryType.main;
    public InventoryType inventoryType { get { return _inventoryType; } }

    [SerializeField]
    protected int inventoryMax = 20;

    public readonly SyncList<Item> collectedItems = new SyncList<Item>(new Item());

    // Start is called before the first frame update
    public override void Start() {

        base.Start();

        collectedItems.Callback += OnInventoryChanged;

        if (isServer) {

            //Delay before we do something
            Invoke(nameof(InitializeCollectedItem), 0.3f);

        }
    }

    public virtual void InitializeCollectedItem() {

        //SyncList only able to initialize on server side, let server handle the initialization
        for (int i = 0; i < inventoryMax; i++) {

            collectedItems.Add(null);

        }

    }

    public override void OnStartLocalPlayer() {

        StartCoroutine(InitializeUI());

    }

    public abstract IEnumerator InitializeUI();

    #region Move or Add or Delete item in inventory

    [Server]
    public void AddToInventory(Item newItem) {

        //Try fill into empty slot or stack if is possible
        if (collectedItems.Count > 0) {

            var newItemData = newItem.GetItemData();

            //Loop the Item in inventory first to stack first
            if (newItemData.stackable) {

                for (int i = 0; i < collectedItems.Count; i++) {

                    var currentItem = collectedItems[i];

                    if (currentItem != null) {

                        if (currentItem.GetItemData().itemID == newItemData.itemID && currentItem.GetItemData().stackable && currentItem.EnoughToStack(newItem.quantity)) {

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

        for (int i = 0; i < collectedItems.Count; i++) {

            var cItem = collectedItems[i];

            if (cItem == targetItem) {

                //If empty need remove this item from inventory
                if (cItem.quantity - quantityToRemove == 0) {

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

    /// <summary>
    /// To move item from old Index to new Index on Server
    /// </summary>
    /// <param name="oldIndex"></param>
    /// <param name="newIndex"></param>
    [Command]
    public void CmdMoveItemFromToIndex(int oldIndex, int newIndex) {

        var item1 = FindItemInInventory(oldIndex);
        var item2 = FindItemInInventory(newIndex);

        //Swap
        if (item2 != null) {

            PutItemAtIndex(oldIndex, item2);

        } else {

            RemoveFromInventory(oldIndex);

        }

        PutItemAtIndex(newIndex, item1);

    }

    /// <summary>
    /// To Move Item from this Inventory to new Inventory
    /// </summary>
    [Command]
    public void CmdMoveItemFromToInventory(int oldIndex, uint targetNetID, InventoryType inventoryType, int newIndex) {

        //Copy my current data and remove from this inventory
        var item1 = FindItemInInventory(oldIndex);

        //Find the new inventory
        NetworkIdentity targetPlayerNI;

        if (NetworkServer.spawned.TryGetValue(targetNetID, out targetPlayerNI)) {

            var inventory = targetPlayerNI.gameObject.GetComponents<InventoryBase>();

            for (int i = 0; i < inventory.Length; i++) {

                var cInventory = inventory[i];

                if (cInventory.inventoryType != inventoryType)
                    continue;

                //Swap
                var item2 = cInventory.FindItemInInventory(newIndex);
                if (item2 != null) {

                    PutItemAtIndex(oldIndex, item2);

                }else {

                    RemoveFromInventory(oldIndex);

                }

                cInventory.PutItemAtIndex(newIndex, item1);
                break;

            }

        }
    }

    #endregion

    #region Finding

    public Item FindItemInInventory(int itemIndex) {

        if (collectedItems.Count > itemIndex) {

            return collectedItems[itemIndex];

        }

        return null;

    }

    public Item FindItemInInventory(string itemID, bool reorderIfStackable = true) {

        List<Item> collectedItem = new List<Item>();

        for (int i = 0; i < collectedItems.Count; i++) {

            var cItem = collectedItems[i];

            if (cItem != null && cItem.GetItemData().itemID == itemID) {

                collectedItem.Add(cItem);

            }

        }

        //Mean we find something
        if (collectedItem.Count > 0) {

            //Reorder if is stackable
            if (collectedItem[0].GetItemData().stackable && reorderIfStackable) {

                collectedItem = collectedItem.OrderByDescending(x => x.quantity).ToList();

            }

            return collectedItem[0];
        }

        return null;

    }

    #endregion

    #region Callback

    public abstract void OnInventoryChanged(SyncList<Item>.Operation op, int itemIndex, Item oldItem, Item newItem);

    #endregion

}