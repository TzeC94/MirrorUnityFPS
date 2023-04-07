using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BaseHeld : InventoryBase
{
    private HeldBase _currentHeld;
    public HeldBase currentHeld { get { return _currentHeld; } }

    //For Local
    private HeldLocal _currentHeldLocal;

    private const int defaultWeaponIndex = 0;

    [Header("Default")]
    public ItemData heldDefault;

    public override void InitializeCollectedItem() {
        //SyncList only able to initialize on server side, let server handle the initialization
        for (int i = 0; i < inventoryMax; i++) {

            var item = new Item(heldDefault);
            collectedItems.Add(item);

        }
    }

    public override IEnumerator InitializeUI() {

        while (PlayerInventoryUIScript.instance == null) {

            yield return null;

        }

        PlayerInventoryUIScript.instance.InitializeWeaponEquip(this);

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(isClient && isLocalPlayer){

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

    private void FireCallback() {

        RPC_FireSuccess(netIdentity.connectionToClient);

    }

    [TargetRpc]
    private void RPC_FireSuccess(NetworkConnection target) {

        if(_currentHeldLocal != null) {

            _currentHeldLocal.AnimFire();
            _currentHeldLocal.PlayerEffect(HeldBase.EffectType.Fire);

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

            _currentHeld.ServerReload();

        }

    }

    private void ReloadCallback() {

        RPC_ReloadSuccess(netIdentity.connectionToClient);

    }

    [TargetRpc]
    private void RPC_ReloadSuccess(NetworkConnection target) {

        if(_currentHeldLocal != null) {

            _currentHeldLocal.AnimReload();

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

            if (isLocalPlayer && _currentHeldLocal) {

                GameCore.Destroy(_currentHeldLocal.gameObject);
                _currentHeldLocal = null;

            }

        } else {

            if (isServer) {

                if (oldItem != null && _currentHeld != null) {

                    NetworkServer.Destroy(_currentHeld.gameObject);

                }

                SpawnHeldItem(newItem);

            }

            if (isLocalPlayer) {
                
                if(_currentHeldLocal != null) {

                    GameCore.Destroy(_currentHeldLocal.gameObject);
                    _currentHeldLocal = null;

                }

                SpawnLocalHeldItem(newItem);

            }

        }

        if (isLocalPlayer && PlayerInventoryUIScript.instance != null && PlayerInventoryUIScript.instance.isOpen) {

            PlayerInventoryUIScript.instance.RefreshEquip();

        }

    }

    [Server]
    private void SpawnHeldItem(Item itemToSpawn) {

        //Need check is weapon before spawn
        if(itemToSpawn.GetItemData().itemType == ItemData.ItemType.Weapon) {

            var spawnedObject = GameObject.Instantiate(itemToSpawn.GetItemData().itemHeldPrefab);
            var heldBase = spawnedObject.GetComponent<HeldBase>();
            heldBase.parentNetID = netIdentity.netId;

            if(netIdentity.connectionToClient != null) {

                heldBase.fireCallback = FireCallback;

                if (heldBase is HeldRange heldRange) {

                    heldRange.reloadCallback = ReloadCallback;
                }

            }

            NetworkServer.Spawn(spawnedObject, netIdentity.connectionToClient);

        }

    }

    [Client]
    private void SpawnLocalHeldItem(Item itemToSpawn) {
        
        //Need check is weapon before spawn
        if (itemToSpawn.GetItemData().itemType == ItemData.ItemType.Weapon) {

            var localObject = itemToSpawn.GetItemData().itemHeldPrefab_Local;
            var spawnedObject = GameCore.Instantiate(localObject, GetComponent<PlayerBase>().localWeaponHoldingRoot);
            //Reset Local
            spawnedObject.transform.localPosition = Vector3.zero;
            _currentHeldLocal = spawnedObject.GetComponent<HeldLocal>();

        }

    }

}