using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUIScript : InventoryUIScript
{
    public static PlayerInventoryUIScript instance;

    public override void Init() {

        base.Init();

        if (instance == null) {

            instance = this;

        }

    }

    private void OnDestroy() {

        instance = null;

    }

    public void FillInventory(int slot){

        maxSlot = slot;

        FillInventory();

    }

}
