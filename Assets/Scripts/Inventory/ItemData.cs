using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemType { General = 0, Weapon = 1}

    public string itemID;
    public string itemName;
    public string itemDescription;
    public string itemIcon;
    public int itemIndex;   //The item index that in the network object list, require to setup through Custom/Setup All Item Data
    public ItemType itemType = ItemType.General;
    public string itemDataAddress;
    public GameObject itemHeldPrefab;
    public GameObject itemHeldPrefab_Local; //Local player model of this

    [Header("Quantity")]
    public bool stackable = false;
    public uint defaultQuantity = 1;
    public uint maxStack = 26;

#if UNITY_EDITOR
    public void OnValidate() {

        var myAddress = AssetDatabase.GetAssetPath(this);
        if (itemDataAddress != myAddress) {
            itemDataAddress = myAddress; 
        }

    }
#endif
}
