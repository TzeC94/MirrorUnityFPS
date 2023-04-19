using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BaseNPC
{
    Vector3 velocity;

    public override Vector3 Velocity() {

        velocity.z = navAgent.velocity.magnitude;

        return velocity;

    }

    public override bool OnGround() {

        return true;

    }
}
