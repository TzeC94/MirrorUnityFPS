using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUIScript : InventoryUIScript
{
    public static PlayerInventoryUIScript instance;

    // Start is called before the first frame update
    public override void Start()
    {
        if(instance != null){

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
