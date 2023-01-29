using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[System.Serializable]
public class Item : IEqualityComparer<Item>
#if UNITY_EDITOR
    , IEditorInitialize
#endif
{
    [SerializeField]
    private ItemData itemData;
    public ItemData ItemData { 
        get {
            if(itemData == null) {
                itemData = ResourceManage.preloadedGameplayAssets[itemDataAddress] as ItemData;
            }

            return itemData;
        } 
    }
    [HideInInspector]
    public string itemDataAddress;
    public uint quantity;

#if UNITY_EDITOR
    public void OnEditorInitialize() {

        var myItemAddress = AssetDatabase.GetAssetPath(ItemData);
        if (itemDataAddress != myItemAddress) {

            itemDataAddress = myItemAddress;
            
        }

    }
#endif

    public Item() {

    }

    public Item(Item oldItemData) {

        this.itemData = oldItemData.itemData;
        this.quantity = oldItemData.quantity;

    }

    public Item(ItemData itemData) {

        this.itemData = itemData;
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

}