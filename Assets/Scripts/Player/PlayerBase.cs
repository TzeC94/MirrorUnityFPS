using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : FirstPersonController {

    [Header("Inventory")]
    public PlayerInventory inventory;

    // Start is called before the first frame update
    public override void Start()
    {
        if (isLocalPlayer) {

            //Register yourself with Game Manager
            GameManagerBase.LocalPlayer = this;

        }
        
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

}