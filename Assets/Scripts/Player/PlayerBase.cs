using Mirror;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : FirstPersonController {

    [Header("Inventory")]
    public PlayerInventory inventory;

    [Header("Weapon")]
    public Transform weaponHoldingRoot;

    [Header("Default")]
    public GameObject weaponDefault;

    // Start is called before the first frame update
    public override void Start()
    {
        if (isLocalPlayer) {

            //Register yourself with Game Manager
            GameManagerBase.LocalPlayer = this;

            //Spawn your default Weapon
            Cmd_SpawnDefaultWeapon();
        }
        
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    #region Default Weapon

    [Command]
    private void Cmd_SpawnDefaultWeapon() {

        var weaponSpawned = Instantiate(weaponDefault);
        NetworkServer.Spawn(weaponSpawned, gameObject);

    }

    #endregion

}