using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDummy : BaseCombatScript
{
    private Rigidbody rigidBody;

    public override void Initialize() {

        base.Initialize();

        rigidBody = GetComponent<Rigidbody>();

    }

    public override void OnKilled(HitInfo hitInfo) {

        base.OnKilled(hitInfo);

        if (isServer) {

            rigidBody.isKinematic = false;

            //Push a bit to fallback
            var hitDirection = transform.position - hitInfo.attackerPos;
            rigidBody.AddForce(hitDirection.normalized, ForceMode.Impulse);
        }

    }

    public override void RPCOnKilled(HitInfo hitInfo) {

        base.RPCOnKilled(hitInfo);

        rigidBody.isKinematic = false;

    }

}