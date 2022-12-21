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
    public int maxBullet = 30;
    public int bulletPerShot = 1;
    [SyncVar]
    private int _currentBullet;
    public int currentBullet {
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

    private void ReduceCurrentBullet(int count) {

        _currentBullet -= count;

    }

}