using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets {

    public partial class FirstPersonController {

        protected override void TPS_AnimatorUpdate() {

            UpdateAnimatorValue(sc_InputMove.y, sc_InputMove.x, Grounded);

            base.TPS_AnimatorUpdate();

        }

    }

}