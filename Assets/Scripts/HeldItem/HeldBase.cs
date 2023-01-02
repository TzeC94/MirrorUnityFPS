using UnityEngine;
using Mirror;
using MyBox;

public abstract partial class HeldBase : NetworkBehaviour
{
    [HideInInspector] public PlayerBase ownerObject;

    [Header("Parent")]
    [SyncVar(hook = nameof(Reparent))]
    [ReadOnly]
    public uint parentNetID;

    [Header("Fire Rate")]
    public float fireInterval = 1;
    public bool holdFire = false;
    [SyncVar]
    [ReadOnly]
    public bool canFire = true;

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

    [Server]
    public void Fire() {

        if (canFire) {

            FireHeld();

            canFire = false;

            //Enter the cooldown
            Invoke(nameof(FireCooldownFinish), fireInterval);
        }

    }

    [Server]
    void FireCooldownFinish() {

        canFire = true;

    }

    public abstract void FireHeld();

}