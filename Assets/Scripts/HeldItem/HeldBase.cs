using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class HeldBase : ItemBase
{
    [HideInInspector] public GameObject ownerObject;

    public override void Start() {

        base.Start();

        //Reparent to player weapon root
        var clientConnection = netIdentity.connectionToClient;

        if (clientConnection != null) {

            var clientObject = clientConnection.identity.gameObject;

            if(clientObject != null) {

                ownerObject = clientObject;

                var playerBase = clientObject.GetComponent<PlayerBase>();

                if(playerBase != null) {

                    gameObject.transform.SetParent(playerBase.weaponHoldingRoot, false);

                    var playerHeld = clientObject.GetComponent<PlayerHeld>();

                    if (playerHeld != null) {

                        playerHeld.SetCurrentHeld(this);

                    }

                }
                
            }

        }

    }

    public virtual void Fire(){
    }

}