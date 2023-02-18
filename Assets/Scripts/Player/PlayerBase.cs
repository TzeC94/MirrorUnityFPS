using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(PlayerHeld))]
public partial class PlayerBase : BaseCombatScript {

    [Header("Inventory")]
    public PlayerInventory inventory;
    public PlayerHeld playerHeld;

    [Header("Weapon")]
    public Transform weaponHoldingRoot;
    public Transform localWeaponHoldingRoot;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

}