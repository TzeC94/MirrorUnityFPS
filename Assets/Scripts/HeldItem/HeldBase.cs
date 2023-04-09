using UnityEngine;
using Mirror;
using MyBox;
using System;

public abstract partial class HeldBase : BaseScriptNetwork, IHeld
{
    public enum EffectType { Fire }

    [HideInInspector] public PlayerBase ownerObject;

    [Header("Parent")]
    [SyncVar(hook = nameof(Reparent))]
    [ReadOnly]
    public uint parentNetID;
    [Tooltip("Name of the attach point")]
    public string heldAttachPoint = "grab point";

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

                //Get!!!
                HeldAttachmentPoint[] attachmentPoints = clientObject.GetComponentsInChildren<HeldAttachmentPoint>();

                foreach(var attachmentPoint in attachmentPoints) {

                    if(attachmentPoint.pointName == heldAttachPoint) {

                        gameObject.transform.SetParent(attachmentPoint.transform, false);

                        var playerHeld = clientObject.GetComponent<BaseHeld>();

                        if (playerHeld != null) {

                            playerHeld.SetCurrentHeld(this);

                        }

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

    /// <summary>
    /// To handle the attack action
    /// </summary>
    public abstract void FireHeld();

    /// <summary>
    /// To reload the weapon
    /// </summary>
    [Server]
    public virtual bool ServerReload() {

        return false;

    }

    [Client]
    public void SetArtActive(bool active) {

        foreach(var art in artObjects) {
            art.SetActive(active);
        }

    }

}