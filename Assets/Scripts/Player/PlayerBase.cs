using Mirror;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBase : FirstPersonController {

    [Header("Inventory")]
    public PlayerInventory inventory;

    [Header("Weapon")]
    public Transform weaponHoldingRoot;

    [Header("Default")]
    public GameObject weaponDefault;

    // Start is called before the first frame update
    public override void OnStartClient() 
    {
        base.OnStartClient();

        if (isLocalPlayer) {

            //Register yourself with Game Manager
            GameManagerBase.LocalPlayer = this;

            //Spawn your default Weapon
            Cmd_SpawnDefaultWeapon();
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    #region Default Weapon

    [Command]
    private void Cmd_SpawnDefaultWeapon() {

        var spawnedObject = GameObject.Instantiate(weaponDefault);
        var heldBase = spawnedObject.GetComponent<HeldBase>();
        heldBase.parentNetID = netIdentity.netId;
        NetworkServer.Spawn(spawnedObject);

    }

    #endregion

}