using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using MyBox;

[System.Serializable]
public class Item : IEqualityComparer<Item>
#if UNITY_EDITOR
    , IEditorInitialize
#endif
{
    [SerializeField]
    private ItemData itemData = null;
    [ReadOnly]
    public string itemDataAddress;
    public uint quantity;

#if UNITY_EDITOR
    public void OnEditorInitialize() {

        var myItemAddress = AssetDatabase.GetAssetPath(itemData);
        if (itemDataAddress != myItemAddress) {

            itemDataAddress = myItemAddress;
            
        }

    }
#endif

    public Item() {

    }

    public Item(Item oldItemData) {

        this.itemData = oldItemData.itemData;
        itemDataAddress = oldItemData.itemDataAddress;
        this.quantity = oldItemData.quantity;

    }

    public Item(ItemData itemData) {

        this.itemData = itemData;
        itemDataAddress = itemData.itemDataAddress;
        quantity = itemData.defaultQuantity;

    }

    public bool EnoughToStack(uint nextQuantity) {

        return quantity + nextQuantity <= itemData.maxStack;

    }

    public bool Equals(Item x, Item y) {

        return x == y && x.quantity == y.quantity;

    }

    public int GetHashCode(Item obj) {

        return obj.GetHashCode();

    }

    public ItemData GetItemData() {

        if (itemData == null) {
            itemData = ResourceManage.GetPreloadGameplayData<ItemData>(itemDataAddress);
        }

        return itemData;

    }

}