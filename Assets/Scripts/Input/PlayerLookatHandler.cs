using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerLookatHandler : MonoBehaviour
{
    public static PlayerLookatHandler instance;

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
        if(instance == null) {

            instance = this;

        }

        if(Camera.main == null) {

            yield return null;

        }

        playerCamera = Camera.main.transform;

    }

    private void OnDestroy() {

        instance = null;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var lookedTarget = LookAtUpdate();
        SetLookAtTarget(lookedTarget);
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

        var hitCount = RayTracer.RaycastNonAlloc(ref ray, ref lookAtHitColliders, range, layerToLook);

        if(hitCount <= 0) {
            return null;
        }

        float distance = range * 1.1f;
        GameObject currentHitTarget = null;

        //Loop through and check 1 by 1
        //Can't sort due to non alloc raycast
        for (int i = 0; i < hitCount; i++) {

            var lookAtTarget = lookAtHitColliders[i];

            if (lookAtTarget.collider.gameObject.GetComponent<BaseScript>()) {

                //Check distance
                if (lookAtTarget.distance < distance) {

                    //Update to this target
                    currentHitTarget = lookAtTarget.collider.gameObject;

                    distance = lookAtTarget.distance;
                }
            }
        }

        return currentHitTarget;
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

                PlayerInputInstance.instance.actionOne_Action += LookAtAction;

                actionAdded = true;

            }

        } else {

            if (actionAdded) {

                PlayerInputInstance.instance.actionOne_Action -= LookAtAction;
                currentLookAtTarget = null;
                actionAdded = false;

            }
        }

    }

    /// <summary>
    /// Action when player press teh action 1 button
    /// </summary>
    public void LookAtAction() {

        var baseComponent = currentLookAtTarget.GetComponent<BaseScript>();

        if (baseComponent is PickupBase) {

            var itemBase = baseComponent as PickupBase;

            if (itemBase.canPickUp) {

                //Add to player inventory
                GameManagerBase.instance.PickupItemToInventory(itemBase.netId, NetworkClient.localPlayer.netId);
            }

        }
    }
}
