using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InventoryBase : NetworkBehaviour
{
    [SerializeField]
    protected int inventoryMax = 20;

    [SerializeField]
    public readonly SyncList<ItemData> collectedItems = new SyncList<ItemData>();

    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public void AddToInventory(ItemData newItemData) {

        collectedItems.Add(newItemData);

    }
}
