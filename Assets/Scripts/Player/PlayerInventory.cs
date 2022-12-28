using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : InventoryBase
{
    //Singleton
    public static PlayerInventory instance;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if(!isLocalPlayer)
            return;

        if(instance == null){

            instance = this;

        }

        StartCoroutine(InitializeUI());

    }

    IEnumerator InitializeUI(){

        while(PlayerInventoryUIScript.instance == null){

            yield return null;

        }

        PlayerInventoryUIScript.instance.FillInventory(inventoryMax);

    }

    private void OnDestroy() {

        instance = null;

    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void OnInventoryChanged(SyncList<Item>.Operation op, int itemIndex, Item oldItem, Item newItem) {

        //IF the UI is open, then we need to refresh it
        if (isLocalPlayer && PlayerInventoryUIScript.instance.isOpen) {

            PlayerInventoryUIScript.instance.RefreshItem();

        }

    }

}
