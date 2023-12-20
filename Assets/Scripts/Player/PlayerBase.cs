using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(BaseHeld))]
public partial class PlayerBase : BaseCombatScript, IAnimator {

    [Header("Naming")]
    public string entityName;

    [Header("Inventory")]
    public PlayerInventory inventory;
    public BaseHeld playerHeld;

    [Header("Weapon")]
    public Transform weaponHoldingRoot;
    public Transform localWeaponHoldingRoot;

    public override void OnStartClient() {

        base.OnStartClient();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate() {

        base.FixedUpdate();

        if(isServer) {

            

        }

    }

    #region IAnimator Interface

    public NetworkBehaviour MyNetworkBehaviour() {

        return this;

    }

    public virtual Vector3 Velocity() {

        return Vector3.zero;

    }

    public virtual bool OnGround() {

        return true;

    }

    #endregion

    public override void OnKilled(HitInfo hitInfo) {

        base.OnKilled(hitInfo);

        //Dead animation??


    }

#if UNITY_EDITOR

    protected override void OnValidate() {

        base.OnValidate();

        if (inventory == null) {

            inventory = GetComponent<PlayerInventory>();

        }

        if (playerHeld == null) {

            playerHeld = GetComponent<BaseHeld>();

        }

    }

#endif

}