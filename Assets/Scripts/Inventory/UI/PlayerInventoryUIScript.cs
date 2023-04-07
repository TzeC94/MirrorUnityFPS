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

    public void InitializeInventory(int slot, InventoryBase inventoryContainer){

        maxSlot = slot;

        FillInventory(inventoryContainer, GameManagerBase.LocalPlayer.netId);

    }

    public void InitializeWeaponEquip(InventoryBase inventoryContainer) {

        weapon_EquipSlot.Initialize(0, inventoryContainer, GameManagerBase.LocalPlayer.netId);

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
                itemSlot.Setup(localPlayer.inventory.collectedItems[i]);

            }
        }

    }

    private void PopulateEquip() {

        var localPlayer = GameManagerBase.LocalPlayer;

        if (localPlayer != null) {

            //The held slot
            var playerHeld = localPlayer.gameObject.GetComponent<BaseHeld>();
            if (playerHeld != null) {

                //Get Item Data
                var itemData = playerHeld.FindItemInInventory(0);
                weapon_EquipSlot.Setup(itemData);

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
