using StarterAssets;
using UnityEngine;

public partial class PlayerBase : BaseCombatScript {

    [Header("Inventory")]
    public PlayerInventory inventory;
    public PlayerHeld playerHeld;

    [Header("Weapon")]
    public Transform weaponHoldingRoot;
    public Transform localWeaponHoldingRoot;

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

}