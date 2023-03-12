using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldMelee : HeldBase {

    [Header("Melee Attack")]
    [Tooltip("Transform point where we use to check the attack")]
    public Transform attackCheckPoint;
    public Vector3 attackSize;
    public LayerMask attackMask;
    [Tooltip("How many seconds until we want to check the the hit, make sure this value smaller than fireInterval")]
    public float waitDurationToCheck = 1f;
    public int damage = 10;

    public override void FireHeld() {

        Invoke(nameof(CheckHit), waitDurationToCheck);

    }

    protected virtual void CheckHit() {

        //Hit box check
        var target = RayTracer.OverlapBox(attackCheckPoint, attackSize, attackMask);

        //Do Something
        var baseCombatScript = target.gameObject.GetComponent<BaseCombatScript>();
        if (baseCombatScript) {

            HitInfo hitInfo = new HitInfo() {

                damage = this.damage,
                attackerPos = transform.position,
                hitPoint = target.ClosestPoint(attackCheckPoint.position)

            };

            baseCombatScript.OnHit(hitInfo);

        }

    }

}