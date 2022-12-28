using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHeld : InventoryBase
{
    private HeldBase _currentHeld;
    public HeldBase currentHeld { get { return _currentHeld; } }

    private const int defaultWeaponIndex = 0;

    public override void OnStartServer() {

        base.OnStartServer();

        for (int i = 0; i < inventoryMax; i++) {

            collectedItems.Add(null);

        }

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

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
        PutItemAtIndex(defaultWeaponIndex, heldObject.itemData);

    }

    #region Fire Button

    [Client]
    void Client_FirePressed(){

        if(PlayerInputInstance.instance.fire){

            CmdFirePressed();

            PlayerInputInstance.instance.fire = false;
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

        base.OnInventoryChanged(op, itemIndex, oldItem, newItem);

        if (newItem == null) {

            //IF new is null mean remove, then destroy current
            if (isServer) {
                NetworkServer.Destroy(_currentHeld.gameObject);
            }
                
            _currentHeld = null;

            if (isLocalPlayer && PlayerInventoryUIScript.instance.isOpen) {

                PlayerInventoryUIScript.instance.RefreshEquip();

            }
            
        }

    }

}