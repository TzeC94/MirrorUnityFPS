using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerBase
{
    // Start is called before the first frame update
    public override void OnStartLocalPlayer() {

        base.OnStartLocalPlayer();

        //Register yourself with Game Manager
        GameManagerBase.LocalPlayer = this;
    }
}
