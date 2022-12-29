using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : InventoryBase
{
    //Singleton
    public static PlayerInventory instance;

    // Start is called before the first frame update
    public override void OnStartLocalPlayer() {

        if (instance == null) {

            instance = this;

        }

        base.OnStartLocalPlayer();
    }

    public override IEnumerator InitializeUI(){

        while(PlayerInventoryUIScript.instance == null){

            yield return null;

        }

        PlayerInventoryUIScript.instance.InitializeInventory(inventoryMax, this);

    }

    private void OnDestroy() {

        instance = null;

    }

    public override void OnInventoryChanged(SyncList<Item>.Operation op, int itemIndex, Item oldItem, Item newItem) {

        //IF the UI is open, then we need to refresh it
        if (isLocalPlayer && PlayerInventoryUIScript.instance.isOpen) {

            PlayerInventoryUIScript.instance.RefreshItem();

        }

    }

}