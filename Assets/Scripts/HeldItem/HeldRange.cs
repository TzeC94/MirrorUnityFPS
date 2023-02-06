using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public abstract class HeldRange : HeldBase
{
    [Header("Projectile")]
    public ProjectileInfoSO projectileInfo;

    [Header("Fire")]
    public Transform fire_FirePoint;
    private HeldFirePointOverride fire_OverridePoint;

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

    [Header("Reload")]
    public float reload_Duration = 1f;
    public Action reloadCallback;

    public override void Start() {

        base.Start();

        if (isServer) {

            _currentBullet = maxBullet;

        }

        fire_OverridePoint = GetComponentInParent<HeldFirePointOverride>();
        
    }

    [Server]
    public override void FireHeld() {

        if (enoughBullet) {

            if(projectileInfo.type == ProjectileInfoSO.ProjectileType.spawn) {

                GameCore.NetworkInstantiate(projectileInfo.projectilePrefab, fire_FirePoint.position, fire_FirePoint.rotation);

            } else {

                RaycastHit hitInfo;

                var fireTransform = fire_OverridePoint ? fire_OverridePoint.fireOverridePoint : fire_FirePoint;

                var hitObject = RayTracer.RaycastNonAllocNearest(new Ray(fireTransform.position, fireTransform.forward), out hitInfo, projectileInfo.distance, projectileInfo.layerHit);

                if(hitObject != null) {

                    //Always target the first hit only
                    AttackHelper.Attack(gameObject, hitObject, hitInfo.point, projectileInfo.damageSO);

                    //Spawn the hit decal
                    PlayHitDecal(hitInfo.collider, hitInfo.point, Quaternion.Euler(hitInfo.normal));

                }

            }

            AnimFire();
            ServerPlayerEffect(EffectType.Fire);
            ReduceCurrentBullet(bulletPerShot);

            fireCallback?.Invoke();

        }

    }

    private void ReduceCurrentBullet(uint count) {

        _currentBullet -= count;

    }

    /// <summary>
    /// To reload the weapon
    /// </summary>
    public override void ServerReload() {

        //Check whether inventory have this item
        var inventory = ownerObject.inventory;

        var bulletItem = inventory.FindItemInInventory(bulletItemID);

        if(bulletItem != null) {

            var maxReloadableBullet = bulletItem.quantity > maxBullet ? maxBullet : bulletItem.quantity;

            StartCoroutine(ReloadGun(inventory, maxReloadableBullet, bulletItem));

            AnimReload();

            reloadCallback?.Invoke();
        }

    }


    [Server]
    private IEnumerator ReloadGun(PlayerInventory inventory, uint maxReloadBullet, Item bulletItem) {

        yield return new WaitForSeconds(reload_Duration);

        _currentBullet = maxReloadBullet;

        //Need to remove the quantity from inventory
        inventory.RemoveQuantityFromInventory(maxReloadBullet, bulletItem);

    }

}