using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemType { General, Weapon}

    public string itemID;
    public string itemName;
    public string itemDescription;
    public string itemIcon;
    public int itemIndex;   //The item index that in the network object list, require to setup through Custom/Setup All Item Data
    public ItemType itemType = ItemType.General;
    public GameObject itemHeldPrefab;
    public GameObject itemHeldPrefab_Local; //Local player model of this

    [Header("Quantity")]
    public bool stackable = false;
    public uint defaultQuantity = 1;
    public uint maxStack = 26;
    
}
