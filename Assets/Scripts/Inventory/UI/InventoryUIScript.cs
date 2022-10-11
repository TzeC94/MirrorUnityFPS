using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIScript : UIBase
{
    [Header("UI")]
    public Transform itemSlotParent;
    public GameObject itemPrefab;

    public int maxSlot = 10;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public override void Open() {

        base.Open();

        FillInventory();

    }

    public override void Close() {

        base.Close();

    }

    public virtual void FillInventory(){

        for(int i = 0; i < maxSlot; i++){

            Instantiate(itemPrefab, itemSlotParent);

        }

    }

}