using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BaseNPC
{
    protected override void TPS_AnimatorUpdate() {

        //Set the value here
        UpdateAnimatorValue(navAgent.velocity.magnitude, 0f, true);

        base.TPS_AnimatorUpdate();

    }
}
