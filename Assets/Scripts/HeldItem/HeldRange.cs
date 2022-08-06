using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HeldRange : HeldBase
{
    public GameObject prefab_Projectile;   //Projectile Game Object

    [Header("Fire")]
    public Transform fire_FirePoint;

    public override void Fire() {

        base.Fire();

        var projectileObject = Instantiate(prefab_Projectile, fire_FirePoint.position, Quaternion.identity);
        NetworkServer.Spawn(projectileObject, ownerObject);

    }

}