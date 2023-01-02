using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHeld : InventoryBase
{
    private HeldBase _currentHeld;
    public HeldBase currentHeld { get { return _currentHeld; } }

    private const int defaultWeaponIndex = 0;

    [Header("Default")]
    public ItemData heldDefault;

    public override void InitializeCollectedItem() {
        //SyncList only able to initialize on server side, let server handle the initialization
        for (int i = 0; i < inventoryMax; i++) {

            var item = new Item(heldDefault);
            collectedItems.Add(new Item(heldDefault));
            //SpawnHeldItem(item);
        }
    }

    public override IEnumerator InitializeUI() {

        while (PlayerInventoryUIScript.instance == null) {

            yield return null;

        }

        PlayerInventoryUIScript.instance.InitializeWeaponEquip(this);

    }

    // Update is called once per frame
    public void Update()
    {
        if(isClient){

            Client_FirePressed();
            Client_ReloadPressed();

        }

        if(isServer){

            Server_FirePressed();

        }
    }

    public void SetCurrentHeld(HeldBase heldObject) {

        _currentHeld = heldObject;

    }

    #region Fire Button

    [Client]
    void Client_FirePressed(){

        if(PlayerInputInstance.instance.fire){

            //To Prevent Spam command to server, we do a safety check here
            if(_currentHeld != null && _currentHeld.canFire 
                && (_currentHeld.holdFire || PlayerInputInstance.instance.firePressed)) {

                CmdFirePressed();
                
            }

        }

    }

    bool s_FirePressed = false;

    [Command]
    void CmdFirePressed(){

        s_FirePressed = true;

    }

    [Server]
    void Server_FirePressed(){

        if(s_FirePressed){

            //Do Something
            _currentHeld?.Fire();

            s_FirePressed = false;

        }

    }

    #endregion

    #region Reload

    [Client]
    void Client_ReloadPressed() {

        if (PlayerInputInstance.instance.reload) {

            Cmd_Reload();

        }

    }

    [Command]
    void Cmd_Reload() {

        if(_currentHeld != null) {

            if (_currentHeld is HeldRange heldRange) {

                heldRange.ServerReload();

            }

        }

    }

    #endregion

    public override void OnInventoryChanged(SyncList<Item>.Operation op, int itemIndex, Item oldItem, Item newItem) {

        //IF new is null no item
        if (newItem == null ) {

            if (isServer) {

                if (_currentHeld != null) {

                    NetworkServer.Destroy(_currentHeld.gameObject);

                }

            }

            _currentHeld = null;

        } else {

            if (isServer) {

                if (oldItem != null && _currentHeld != null) {

                    NetworkServer.Destroy(_currentHeld.gameObject);

                }

                SpawnHeldItem(newItem);

            }

        }

        if (isLocalPlayer && PlayerInventoryUIScript.instance != null && PlayerInventoryUIScript.instance.isOpen) {

            PlayerInventoryUIScript.instance.RefreshEquip();

        }

    }

    private void SpawnHeldItem(Item itemToSpawn) {

        //Need check is weapon before spawn
        if(itemToSpawn.itemData.itemType == ItemData.ItemType.Weapon) {

            var spawnedObject = GameObject.Instantiate(itemToSpawn.itemData.itemHeldPrefab);
            var heldBase = spawnedObject.GetComponent<HeldBase>();
            heldBase.parentNetID = netIdentity.netId;
            NetworkServer.Spawn(spawnedObject);

        }

    }

}