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

    public override void Open() {

        base.Open();

        PopulateItem();

    }

    public void FillInventory(int slot){

        maxSlot = slot;

        FillInventory();

    }

    /// <summary>
    /// To populate the item you owned on UI
    /// </summary>
    private void PopulateItem() {

        var localPlayer = GameManagerBase.LocalPlayer;

        if (localPlayer != null) {

            int totalCount = localPlayer.inventory.collectedItems.Count;

            for(int i = 0; i < totalCount; i++) {

                var itemSlot = itemSlotList[i];
                itemSlot.Setup(localPlayer.inventory.collectedItems[i], i, localPlayer.netId);

            }

        }

    }

    public void RefreshItem() {

        PopulateItem();

    }

}
