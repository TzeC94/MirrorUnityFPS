using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public string itemIcon;
}
