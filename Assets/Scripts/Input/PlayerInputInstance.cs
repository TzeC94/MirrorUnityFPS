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
                CursorVisibility(false);

            } else {

                PlayerInventoryUIScript.instance.Open();
                CursorVisibility(true);
            }

            UpdateInputLock();

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

    public void OnConsoleCommand(InputValue value) {

        if (value.isPressed) {

            if (ConsoleCommandUI.Instance.isOpen == false) {

                ConsoleCommandUI.Instance.Open();
                CursorVisibility(true);

            } else {

                ConsoleCommandUI.Instance.Close();
                CursorVisibility(false);

            }

            UpdateInputLock();

        }

    }

    private void UpdateInputLock() {

        cursorInputForLook = ConsoleCommandUI.Instance.isOpen || PlayerInventoryUIScript.instance.isOpen;

    }

    public void CursorVisibility(bool visible) {

        Cursor.lockState = visible? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;

    }
}
