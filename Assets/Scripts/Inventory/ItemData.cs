using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public string itemDescription;
    public string itemIcon;
    public int itemIndex;   //The item index that in the network object list, require to setup through Custom/Setup All Item Data

    public uint defaultQuantity = 1;
}
