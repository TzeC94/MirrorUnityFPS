using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LookAtPickup : MonoBehaviour
{
    public static LookAtPickup instance;

    private Transform playerCamera;

    [Header("Look At")]
    public int range = 20;
    public LayerMask layerToLook;
    private RaycastHit[] lookAtHitColliders = new RaycastHit[20];
    private GameObject currentLookAtTarget;
    private bool actionAdded = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if(MyNetworkManager.instance.mode != NetworkManagerMode.ServerOnly) {

            if (instance == null) {

                instance = this;

            }

            while (Camera.main == null) {

                yield return null;

            }

            playerCamera = Camera.main.transform;

        }
    }

    private void OnDestroy() {

        instance = null;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (MyNetworkManager.instance.mode != NetworkManagerMode.ServerOnly) {

            var lookedTarget = LookAtUpdate();
            SetLookAtTarget(lookedTarget);

        }
    }

    /// <summary>
    /// Raycast and check the look at target
    /// </summary>
    /// <returns></returns>
    GameObject LookAtUpdate() {

        if(playerCamera == null) {
            return null;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        var hitObject = RayTracer.RaycastNonAllocNearest(ray, range, layerToLook);

        if(hitObject == null) {

            return null;

        } else {

            var baseScript = hitObject.GetComponent<BaseScriptNetwork>() as PickupBase;
            return baseScript && baseScript.canPickUp ? null : hitObject;

        }

    }

    /// <summary>
    /// Set the look at target
    /// </summary>
    /// <param name="lookAtTarget"></param>
    void SetLookAtTarget(GameObject lookAtTarget) {

        if (lookAtTarget == currentLookAtTarget)
            return;

        if(lookAtTarget != null && lookAtTarget != currentLookAtTarget) {

            currentLookAtTarget = lookAtTarget;

            if(actionAdded == false) {

                var baseScript = currentLookAtTarget.GetComponentInParent<BaseScriptNetwork>();

                PlayerInputInstance.instance.actionOne_Action = baseScript.GetActions(NetworkClient.localPlayer.netId)[0].action;

                actionAdded = true;

            }

        } else {

            if (actionAdded) {

                PlayerInputInstance.instance.actionOne_Action = null;
                currentLookAtTarget = null;
                actionAdded = false;

            }
        }

    }
}
