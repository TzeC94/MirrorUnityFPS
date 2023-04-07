using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(BaseHeld))]
public partial class PlayerBase : BaseCombatScript {

    [Header("Inventory")]
    public PlayerInventory inventory;
    public BaseHeld playerHeld;

    [Header("Weapon")]
    public Transform weaponHoldingRoot;
    public Transform localWeaponHoldingRoot;

    public override void OnStartClient() {

        base.OnStartClient();

        InitializeModel();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate() {

        base.FixedUpdate();

        if(isServer) {

            TPS_AnimatorUpdate();

        }

    }

#if UNITY_EDITOR

    public virtual void OnValidate() {

        if (inventory == null) {

            inventory = GetComponent<PlayerInventory>();

        }

        if (playerHeld == null) {

            playerHeld = GetComponent<BaseHeld>();

        }

    }

#endif

}