using StarterAssets;
using System;
using System.Diagnostics;
using UnityEngine;
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

    private bool _reload;
    public bool reload {

        get {
            if (_reload) {
                var oldValue = _reload;
                _reload = false;
                return oldValue;
            } else {
                return false;
            }
            
        }

    }

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
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            } else {

                PlayerInventoryUIScript.instance.Open();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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

    public void OnReload(InputValue value) {

        _reload = value.isPressed;

    }
}
