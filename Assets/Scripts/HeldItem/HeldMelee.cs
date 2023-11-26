using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeldMelee : HeldBase {

    [Header("Melee Attack")]
    [Tooltip("Transform point where we use to check the attack")]
    public Transform attackCheckPoint;
    public Vector3 attackSize;
    public LayerMask attackMask;
    [Tooltip("How many seconds until we want to check the the hit, make sure this value smaller than fireInterval")]
    public float waitDurationToCheck = 1f;
    public int damage = 10;

    [Server]
    public override void FireHeld() {

        AnimFire();
        fireCallback?.Invoke();

        Invoke(nameof(CheckHit), waitDurationToCheck);

    }

    protected virtual void CheckHit() {

        //Hit box check
        var target = RayTracer.OverlapBox(attackCheckPoint, attackSize, attackMask);

        if (target == null)
            return;

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

#if UNITY_EDITOR

    protected void OnDrawGizmosSelected() {

        if(attackCheckPoint != null) {

            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(attackCheckPoint.position, attackCheckPoint.rotation, attackCheckPoint.lossyScale);
            Gizmos.DrawCube(Vector3.zero, attackSize);

        }

    }

#endif

}