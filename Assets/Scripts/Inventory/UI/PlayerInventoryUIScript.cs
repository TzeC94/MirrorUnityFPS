using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUIScript : InventoryUIScript
{
    public static PlayerInventoryUIScript instance;

    [Header("Player Equipment")]
    public InventoryUIItemScript weapon_EquipSlot;

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
        PopulateEquip();
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
                itemSlot.Setup(localPlayer.inventory.collectedItems[i], i, localPlayer.netId, localPlayer.inventory);

            }
        }

    }

    private void PopulateEquip() {

        var localPlayer = GameManagerBase.LocalPlayer;

        if (localPlayer != null) {

            //The held slot
            var playerHeld = localPlayer.gameObject.GetComponent<PlayerHeld>();
            if (playerHeld != null) {

                var currentHeldData = playerHeld.currentHeld == null ? null : playerHeld.currentHeld.itemData;
                weapon_EquipSlot.Setup(currentHeldData, 0, localPlayer.netId, playerHeld);

            }

        }

    }

    public void RefreshItem() {

        PopulateItem();

    }

    public void RefreshEquip() {

        PopulateEquip();

    }

}
