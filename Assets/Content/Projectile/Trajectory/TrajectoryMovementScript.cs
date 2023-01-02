using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryMovementScript : ProjectileMovementBase
{
    public override void Start() {
        
        base.Start();

        if (isServer) {
            MoveProjectile();
        }
    }

    public override void FixedUpdate() {
        
        //Override this to not run the thing inside

    }

    public override void AddForce(Vector3 direction) {
        
        rigidBody.AddForce(direction * moveSpeed, ForceMode.Impulse);

    }
}