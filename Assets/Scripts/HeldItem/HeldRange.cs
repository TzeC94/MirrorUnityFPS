using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HeldRange : HeldBase
{
    [Header("Projectile")]
    public ProjectileInfoSO projectileInfo;
    private Ray ray;
    private RaycastHit[] hit;

    [Header("Fire")]
    public Transform fire_FirePoint;

    [Header("Bullet")]
    public uint maxBullet = 30;
    public uint bulletPerShot = 1;
    public string bulletItemID;
    [SyncVar]
    private uint _currentBullet;
    public uint currentBullet {
        get { return _currentBullet; }
    }
    public bool enoughBullet {
        get { return _currentBullet > 0; }
    }

    public virtual void Start() {

        if (isServer) {

            _currentBullet = maxBullet;

            //Initialize the Raycast if needed
            if(projectileInfo.type == ProjectileInfoSO.ProjectileType.raycast) {
                ray = new Ray();
            }
        }
        
    }

    [Server]
    public override void FireHeld() {

        if (enoughBullet) {

            if(projectileInfo.type == ProjectileInfoSO.ProjectileType.spawn) {

                GameCore.NetworkInstantiate(projectileInfo.projectilePrefab, fire_FirePoint.position, fire_FirePoint.rotation);

            } else {

                ray.origin = fire_FirePoint.position;
                ray.direction = fire_FirePoint.forward;
                var hitCount = RayTracer.RaycastNonAlloc(ref ray, ref hit, projectileInfo.distance, projectileInfo.layerHit);

                if(hitCount> 0) {

                    //Always target the first hit only
                    AttackHelper.Attack(gameObject, hit[0].transform.gameObject, projectileInfo.damageSO);

                }

            }

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
        var inventory = ownerObject.inventory;

        var bulletItem = inventory.FindItemInInventory(bulletItemID);

        if(bulletItem != null) {

            var maxReloadableBullet = bulletItem.quantity > maxBullet ? maxBullet : bulletItem.quantity;

            _currentBullet = maxReloadableBullet;

            //Need to remove the quantity from inventory
            inventory.RemoveQuantityFromInventory(maxReloadableBullet, bulletItem);

        }

    }

}