using UnityEngine;
using Mirror;
using MyBox;
using System;

public abstract partial class HeldBase : NetworkBehaviour, IHeld
{
    public enum EffectType { Fire }

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
    public Action fireCallback;

    [Header("Art")]
    public GameObject[] artObjects;

    public override void OnStartServer() {

        base.OnStartServer();

        Reparent(0, this.parentNetID);

    }

    public override void OnStartClient() {

        base.OnStartClient();

        //Need to turn off if is local player
        if (ownerObject.isLocalPlayer) {

            SetArtActive(false);

        }

    }

    private void Reparent(uint oldParentNetID, uint newParentNetID) {

        NetworkIdentity targetNetIdentity;
        bool found = false;

        found = (isServer) ? NetworkServer.spawned.TryGetValue(parentNetID, out targetNetIdentity) : NetworkClient.spawned.TryGetValue(newParentNetID, out targetNetIdentity);

        if (found) {

            var clientObject = targetNetIdentity.gameObject;

            if (clientObject != null) {

                ownerObject = clientObject.GetComponent<PlayerBase>();

                if (ownerObject != null) {

                    gameObject.transform.SetParent(ownerObject.weaponHoldingRoot, false);

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

    /// <summary>
    /// To reload the weapon
    /// </summary>
    [Server]
    public virtual void ServerReload() {

        

    }

    [Client]
    public void SetArtActive(bool active) {

        foreach(var art in artObjects) {
            art.SetActive(active);
        }

    }

}