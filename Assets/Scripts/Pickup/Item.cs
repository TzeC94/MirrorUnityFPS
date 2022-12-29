using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : IEqualityComparer<Item> {

    public ItemData itemData;
    public uint quantity;

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