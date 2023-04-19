using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets {

    public partial class FirstPersonController {

        public override Vector3 Velocity() {

            return sc_InputMove;

        }

        public override bool OnGround() {

            return Grounded;

        }

    }

}