using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InventoryBase : NetworkBehaviour
{
    [SerializeField]
    protected int inventoryMax = 20;

    [SerializeField]
    protected List<ItemData> collectedItems = new List<ItemData>(20);

#if UNITY_EDITOR

    private void OnValidate() {
        
        if(collectedItems.Count != inventoryMax){

            collectedItems.Clear();

            for(int i = 0; i < inventoryMax; i++){

                collectedItems.Add(null);

            }
        }
    }

#endif

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
