using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HeldRange : HeldBase
{
    public GameObject prefab_Projectile;   //Projectile Game Object

    [Header("Fire")]
    public Transform fire_FirePoint;

    [Header("Bullet")]
    public uint maxBullet = 30;
    public uint bulletPerShot = 1;
    public virtual string bulletItemID { get; }
    [SyncVar]
    private uint _currentBullet;
    public uint currentBullet {
        get { return _currentBullet; }
    }
    public bool enoughBullet {
        get { return _currentBullet > 0; }
    }

    public override void Start() {

        base.Start();

        if (isServer) {

            _currentBullet = maxBullet;

        }
        
    }

    public override void Fire() {

        if (enoughBullet) {

            base.Fire();

            GameCore.NetworkInstantiate(prefab_Projectile, fire_FirePoint.position, fire_FirePoint.rotation);

            ReduceCurrentBullet(bulletPerShot);

        }

    }

    private void ReduceCurrentBullet(uint count) {

        _currentBullet -= count;

    }

    /// <summary>
    /// To reload the weapon
    /// </summary>
    [Server]
    public void ServerReload() {

        //Check whether inventory have this item
        var inventory = ownerObject.GetComponent<InventoryBase>();

        var bulletItem = inventory.FindItemInInventory(bulletItemID);

        var maxReloadableBullet = bulletItem.quantity > maxBullet ? maxBullet : bulletItem.quantity;

        _currentBullet = maxReloadableBullet;

        //Need to remove the quantity from inventory
        inventory.RemoveQuantityFromInventory(maxReloadableBullet, bulletItem);

    }

}