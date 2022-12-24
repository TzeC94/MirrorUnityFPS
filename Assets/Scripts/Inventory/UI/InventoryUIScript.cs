using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIScript : UIBase
{
    [Header("UI")]
    public Transform itemSlotParent;
    public GameObject itemPrefab;

    public int maxSlot = 10;

    protected List<InventoryUIItemScript> itemSlotList = new List<InventoryUIItemScript>();

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

    }

    public override void Close() {

        base.Close();

    }

    public virtual void FillInventory(){

        //Destroy the list if not match
        if(itemSlotParent.childCount != maxSlot){

            itemSlotList.Clear();

            foreach (Transform childTrans in itemSlotParent){

                Destroy(childTrans.gameObject);

            }
        }

        for(int i = 0; i < maxSlot; i++){

            var uiObject = GameCore.Instantiate(itemPrefab, itemSlotParent);
            var itemCompoment = uiObject.GetComponent<InventoryUIItemScript>();
            itemCompoment.Initialize();
            itemSlotList.Add(itemCompoment);

        }

    }

}