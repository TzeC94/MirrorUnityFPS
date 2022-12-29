using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class HeldBase : ItemBase
{
    [HideInInspector] public PlayerBase ownerObject;

    [SyncVar(hook = nameof(Reparent))]
    public uint parentNetID;

    public override void OnStartServer() {

        base.OnStartServer();

        Reparent(0, this.parentNetID);

    }

    private void Reparent(uint oldParentNetID, uint newParentNetID) {

        NetworkIdentity targetNetIdentity;
        bool found = false;

        found = (isServer) ? NetworkServer.spawned.TryGetValue(newParentNetID, out targetNetIdentity) : NetworkClient.spawned.TryGetValue(parentNetID, out targetNetIdentity);

        if (found) {

            var clientObject = targetNetIdentity.gameObject;

            if (clientObject != null) {

                ownerObject = clientObject.GetComponent<PlayerBase>();

                var playerBase = clientObject.GetComponent<PlayerBase>();

                if (playerBase != null) {

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