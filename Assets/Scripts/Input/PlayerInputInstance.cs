using StarterAssets;
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

    public override void Start() {
        base.Start();

        if (_instance == null)
            _instance = this;

        PlayerInputInstance._playerInput = GetComponent<PlayerInput>();
    }
}
