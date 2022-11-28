using StarterAssets;
using System;
using System.Diagnostics;
using UnityEngine.InputSystem;

public class PlayerInputInstance : StarterAssetsInputs
{
    private static PlayerInputInstance _instance;

    public static PlayerInput _playerInput;

    public static PlayerInputInstance instance{
        get{
            return _instance;
        }
    }

    public Action actionOne_Action;

    public override void Start() {

        base.Start();

        if (_instance == null)
            _instance = this;

        PlayerInputInstance._playerInput = GetComponent<PlayerInput>();
    }

    public void OnInventory(InputValue value) {

        if (value.isPressed) {

            if (PlayerInventoryUIScript.instance.isOpen) {

                PlayerInventoryUIScript.instance.Close();

            } else {

                PlayerInventoryUIScript.instance.Open();

            }

            //Lock the input base on inventory
            cursorInputForLook = PlayerInventoryUIScript.instance.isOpen == false;

        }

    }

    public void OnActionOne(InputValue value) {

        if (value.isPressed) {

            actionOne_Action?.Invoke();

        }

    }
}
