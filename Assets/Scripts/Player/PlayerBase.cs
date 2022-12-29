using StarterAssets;
using UnityEngine;

public class PlayerBase : FirstPersonController {

    [Header("Inventory")]
    public PlayerInventory inventory;
    public PlayerHeld playerHeld;

    [Header("Weapon")]
    public Transform weaponHoldingRoot;

    // Start is called before the first frame update
    public override void OnStartLocalPlayer() {

        base.OnStartLocalPlayer();

        //Register yourself with Game Manager
        GameManagerBase.LocalPlayer = this;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

}